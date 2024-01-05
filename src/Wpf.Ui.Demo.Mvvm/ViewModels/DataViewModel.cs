// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using ClosedXML.Excel;
using System.Collections.ObjectModel;
using System.Windows.Media;
using Wpf.Ui.Controls;
using Wpf.Ui.Demo.Mvvm.Helpers;
using Wpf.Ui.Demo.Mvvm.Models;
using Wpf.Ui.Demo.Mvvm.Services;

namespace Wpf.Ui.Demo.Mvvm.ViewModels;

public partial class DataViewModel : ObservableObject, INavigationAware
{
    private readonly DSWorkwareService dSWorkwareService;

    private bool _isInitialized = false;
    [ObservableProperty]
    private ObservableCollection<HistoryData> _historyDatas=new ();
    [ObservableProperty]
    private int _selectIndex=-1;

    [ObservableProperty]
    private IEnumerable<DataColor> _colors;

    private readonly ISnackbarService _snackbarService;
    private readonly IContentDialogService _contentDialogService;



    public DataViewModel(DSWorkwareService dSWorkwareService, ISnackbarService snackbarService, IContentDialogService contentDialogService = null)
    {
        this.dSWorkwareService = dSWorkwareService;
        this._snackbarService = snackbarService;
        _contentDialogService = contentDialogService;
    }
    public void OnNavigatedTo()
    {
        if (!_isInitialized)
            InitializeViewModel();
    }

    public void OnNavigatedFrom() { }

    private void InitializeViewModel()
    {
 refreshData();

        _isInitialized = true;
    }
    private void refreshData() {
        //database
        List<DSWorkware>? dSWorkwares = dSWorkwareService.GetDsWorkwareList();
        HistoryDatas.Clear();
        if (dSWorkwares != null) {
            foreach (DSWorkware item in dSWorkwares)
            {
              var historyData=  new HistoryData();
                historyData.Id = item.id;
                historyData.IsCheck = item.IsCheck;
                historyData.CreateTime = item.CreateTime;
                historyData.DataCount = item.DSWorkwareItems.Count;
                historyData.TemperatureList= String.Join(",", item.DSWorkwareItems.GroupBy(o => o.StandardTemperature).Select(o => o.FirstOrDefault()).ToList().Select(o=>o.StandardTemperature.ToString()).ToList());
                historyData.PressureList = String.Join(",", item.DSWorkwareItems.GroupBy(o => o.StandardPressure).Select(o => o.FirstOrDefault()).Select(o => o.StandardPressure.ToString()).ToList());
                HistoryDatas.Add(historyData);
            }
        }

   
    }

    [RelayCommand]
    private async Task DeleteDataAsync(long id) {
        if (id != null && id >= 0)
        {
            ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
         new SimpleContentDialogCreateOptions()
         {
             Title = "删除确认",
             Content = $"是否删除, Id为:{id} 的数据",
             PrimaryButtonText = "确定",
             CloseButtonText = "取消",
         }
     );
            if (result == ContentDialogResult.Primary)
            {
                var delete = dSWorkwareService.DeleteWorkware(id);
                if (!delete)
                {

                }
            }
            refreshData();
        }
        else {
        
        }
   

    }

    [RelayCommand]
    private async Task ExportRealData(long id) {
      await  ExportExcel(id, true);
    }

    [RelayCommand]
    private async Task ExportNotRealData(long id) {
        await ExportExcel(id, false);

    }
    private async Task ExportExcel(long id,bool isRealData)
    {
        // check history data
        DSWorkware? dSWorkware = dSWorkwareService.GetNewsData(id);
        if (dSWorkware == null)
        {
            return;
        }

        // 提示用户History信息和时间
        ContentDialogResult result = await _contentDialogService.ShowSimpleDialogAsync(
          new SimpleContentDialogCreateOptions()
          {
              Title = "开始导出提醒",
              Content = $"是开始生成数据: 测试流程:{dSWorkware.ProcessFlowEnum.ToDescription()}, 创建时间为:{dSWorkware.CreateTime} ,是否检测完成:{dSWorkware.IsCheck}",
              PrimaryButtonText = "确定",
              CloseButtonText = "取消",
          }
      );
        if (result == ContentDialogResult.Primary)
        {
            // 继续生成Excel文件
            //循环设备信息
            var dsWorkwareItems = dSWorkware.DSWorkwareItems.GroupBy(o => o.Equipment)
                                                            .ToDictionary(o => o.Key, o => o.ToList());
            DateTime now = DateTime.Now;
            var times = now.Year.ToString() + now.Month.ToString() + now.Day.ToString() + now.Hour.ToString() + now.Minute.ToString() + now.Second.ToString();
            var exportPath = isRealData ? $"C:\\realXml\\{times}\\" : $"C:\\xml\\{times}\\";
            foreach (KeyValuePair<string, List<DSWorkwareItem>> item in dsWorkwareItems)
            {

                // 根据设备的循环生成对应的设备Excel报告
                using var xlWorkBook = new XLWorkbook();

                // 根据温度生成Sheet
                var temperatureGroup = item.Value.GroupBy(o => o.StandardTemperature).ToDictionary(o => o.Key, o => o.ToList());
                foreach (KeyValuePair<float, List<DSWorkwareItem>> temperatureItem in temperatureGroup)
                {
                    var xml = xlWorkBook.AddWorksheet(temperatureItem.Key.ToString());

                    // 循环压力数据
                    // 行标记
                    var pressureCount = 1;
                    foreach (DSWorkwareItem pressureItem in temperatureItem.Value)
                    {
                        // 列标记
                        var count = 1;
                        xml.Cell(count++, pressureCount).Value = pressureItem.StandardPressure;
                        foreach (DSWorkwareArea relaData in pressureItem.DSWorkwareAreas)
                        {
                            if (isRealData) {
                                xml.Cell(count++, pressureCount).Value = relaData.Pressure.ToString("#0.0000");
                            }
                            else
                            {
                                xml.Cell(count++, pressureCount).Value = ((relaData.Pressure - pressureItem.StandardPressure) / pressureItem.StandardPressure).ToString("#0.0000");

                            }
                        }

                        pressureCount++;
                    }
                }

                // save xmal
                var fileName = $"{exportPath}{item.Key}.xlsx";
                xlWorkBook.SaveAs(fileName);
            }
            _ = _contentDialogService.ShowSimpleDialogAsync(
    new SimpleContentDialogCreateOptions()
    {
        Title = "已生成提醒",
        Content = $"生成路径：{exportPath}",
        PrimaryButtonText = "确定",
        CloseButtonText = "取消",
    }
);
        }
    }
}
