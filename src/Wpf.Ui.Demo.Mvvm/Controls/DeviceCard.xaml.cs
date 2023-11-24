// This Source Code Form is subject to the terms of the MIT License.
// If a copy of the MIT was not distributed with this file, You can obtain one at https://opensource.org/licenses/MIT.
// Copyright (C) Leszek Pomianowski and WPF UI Contributors.
// All Rights Reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Windows.ApplicationModel.Chat;

namespace Wpf.Ui.Demo.Mvvm.Controls
{
    /// <summary>
    /// DeviceCard.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceCard : UserControl
    {
        public DeviceCard()
        {
            InitializeComponent();
            TextChangeControl();
        }

        private static Color _redColor = (Color)ColorConverter.ConvertFromString("#FA8072");
        private static Color _greenColor = (Color)ColorConverter.ConvertFromString("#00FF00");
        LinearGradientBrush RedColor = new LinearGradientBrush(new GradientStopCollection { new GradientStop(_redColor, 0) });
        LinearGradientBrush GreenColor = new LinearGradientBrush(new GradientStopCollection { new GradientStop(_greenColor, 0) });

        public string PortText
        {
            get { return (string)GetValue(PortTextProperty); }
            set { SetValue(PortTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PortText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PortTextProperty =
            DependencyProperty.Register(nameof(PortText), typeof(string), typeof(DeviceCard), new PropertyMetadata("", new PropertyChangedCallback(ValueChanged)));



        public string ImageUrl
        {
            get { return (string)GetValue(ImageUrlProperty); }
            set { SetValue(ImageUrlProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageUrl.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageUrlProperty =
            DependencyProperty.Register(nameof(ImageUrl), typeof(string), typeof(DeviceCard), new PropertyMetadata("pack://application:,,,/Assets/WinUiGallery/Pressure.png", new PropertyChangedCallback(ValueChanged)));



        public string PressureText
        {
            get { return (string)GetValue(PressureTextProperty); }
            set { SetValue(PressureTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PressureText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PressureTextProperty =
            DependencyProperty.Register(nameof(PressureText), typeof(string), typeof(DeviceCard), new PropertyMetadata(null, new PropertyChangedCallback(ValueChanged)));



        public string TemperatureText
        {
            get { return (string)GetValue(TemperatureTextProperty); }
            set { SetValue(TemperatureTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TemperatureTet.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TemperatureTextProperty =
            DependencyProperty.Register(nameof(TemperatureText), typeof(string), typeof(DeviceCard), new PropertyMetadata(null, new PropertyChangedCallback(ValueChanged)));



        public bool Status
        {
            get { return (bool)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Status.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StatusProperty =
            DependencyProperty.Register(nameof(Status), typeof(bool), typeof(DeviceCard), new PropertyMetadata(false, new PropertyChangedCallback(ValueChanged)));




        public string DeviceNameText
        {
            get { return (string)GetValue(DeviceNameTextProperty); }
            set { SetValue(DeviceNameTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DeviceNameText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DeviceNameTextProperty =
            DependencyProperty.Register(nameof(DeviceNameText), typeof(string), typeof(DeviceCard), new PropertyMetadata("", new PropertyChangedCallback(ValueChanged)));




        public string PUtil
        {
            get { return (string)GetValue(PUtilProperty); }
            set { SetValue(PUtilProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PUtil.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PUtilProperty =
            DependencyProperty.Register(nameof(PUtil), typeof(string), typeof(DeviceCard), new PropertyMetadata("KPa"));



        public string TUtil
        {
            get { return (string)GetValue(TUtilProperty); }
            set { SetValue(TUtilProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TUtil.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TUtilProperty =
            DependencyProperty.Register(nameof(TUtil), typeof(string), typeof(DeviceCard), new PropertyMetadata("℃"));




        private static void ValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((DeviceCard)d).TextChangeControl();
        }

        private void TextChangeControl()
        {
            PressureUtil.SetCurrentValue(TextBlock.TextProperty, PUtil);
            TemperatureUtil.SetCurrentValue(TextBlock.TextProperty, TUtil);
            var uri = new Uri(ImageUrl, UriKind.Absolute);
            Image.SetCurrentValue(Image.SourceProperty, new BitmapImage(uri));
            Pressure.SetCurrentValue(TextBlock.TextProperty, PressureText);
            Port.SetCurrentValue(TextBlock.TextProperty, PortText);
            Temperature.SetCurrentValue(TextBlock.TextProperty, TemperatureText);
            DeviceName.SetCurrentValue(TextBlock.TextProperty, DeviceNameText);
            if (!Status)
            {
                StatusTag.SetCurrentValue(Shape.FillProperty, RedColor);
                StatusText.SetCurrentValue(TextBlock.TextProperty, "Disconnection");
            }
            else
            {
                StatusTag.SetCurrentValue(Shape.FillProperty, GreenColor);
                StatusText.SetCurrentValue(TextBlock.TextProperty, "Connection");
            }
        }
    }
}
