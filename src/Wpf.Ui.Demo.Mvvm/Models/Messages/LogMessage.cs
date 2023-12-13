using System.Windows.Controls;
using Wpf.Ui.Demo.Mvvm.Helpers;

namespace Wpf.Ui.Demo.Mvvm.Models;

/// <summary>
/// 日志消息结构对象
/// </summary>
public class LogMessage
{
    /// <summary>
    /// 发生消息设备唯一Id
    /// </summary>
    public DeviceTypeEnum Key { get; set; }

    /// <summary>
    /// 消息信息
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// 消息创建时间
    /// </summary>
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 发送方向
    /// </summary>
    public MessageSendDirection SendDirection { get; set; }

    /// <summary>
    /// 输出定义格式
    /// </summary>
    /// <returns>Message</returns>
    public string GetOutMessage()
    {
        return $"{CreateTime} : {Message} \n";
    }
}
