using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Helpers;

public static class SerialPortExtension
{
    public delegate void ReceiveData(byte[] receivData);

    /// <summary>
    /// 更新端口
    /// </summary>
    /// <param name="serialPort">端口对象</param>
    /// <param name="serialPortModel">绑定的数据</param>
    public static void UpdateSerialPortModel(this SerialPort serialPort, SerialPortModel serialPortModel)
    {
        serialPort.PortName = serialPortModel.PortName;
        serialPort.StopBits = serialPortModel.StopBit;
        serialPort.BaudRate = serialPortModel.BaudRate;
        serialPort.DataBits = serialPortModel.DataBit;
    }

    public static void SetDataReceiveData(this SerialPort serialPort, ReceiveData receiveData)
    {
        serialPort.DataReceived += (e, o) =>
        {
            var port = (SerialPort)e;
            var count = port.BytesToRead;
            var buff = new byte[count];
            port.Read(buff, 0, count);
            if (count == 0)
            {
                receiveData.Invoke(buff);
            }
        };
    }

    public static void SendStringMsg(this SerialPort serialPort, string msg)
    {
        serialPort.DiscardInBuffer();
        serialPort.DiscardOutBuffer();
        serialPort.Write(msg);
    }

    public static bool OpenPort(this SerialPort serialPort)
    {
        if (!serialPort.IsOpen)
        {
            try
            {
                serialPort.Open();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        return true;
    }
}