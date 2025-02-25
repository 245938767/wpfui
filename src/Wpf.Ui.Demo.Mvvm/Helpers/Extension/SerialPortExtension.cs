using System.IO.Ports;
using Wpf.Ui.Demo.Mvvm.Models;

namespace Wpf.Ui.Demo.Mvvm.Helpers.Extension;

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
        if (serialPortModel.PortName != null)
        {
            serialPort.PortName = serialPortModel.PortName;
        }

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
           var readCount= port.Read(buff, 0, count);
            if (readCount > 0)
            {
                receiveData.Invoke(buff);
            }
        };
    }

    public static void SendHexCRC(this SerialPort serialPort, string hexString, object? lockObject)
    {
        if (!serialPort.IsOpen)
        {
            return;
        }

        if (lockObject == null)
        {
            var checkCrc = hexString + CRCModelHelper.ToModbusCRC16(hexString);
            var sendBuffer = CRCModelHelper.StringToHexByte(checkCrc);
            serialPort.Write(sendBuffer, 0, sendBuffer.Length);
            return;
        }

        lock (lockObject)
        {
            var checkCrc = hexString + CRCModelHelper.ToModbusCRC16(hexString);
            var sendBuffer = CRCModelHelper.StringToHexByte(checkCrc);
            serialPort.Write(sendBuffer, 0, sendBuffer.Length);
        }
    }

    public static void SendStringMsg(this SerialPort serialPort, string msg, object? lockObject)
    {
        if (!serialPort.IsOpen)
        {
            return;
        }

        if (lockObject == null)
        {
            serialPort.Write(msg);
            return;
        }

        lock (lockObject)
        {
            serialPort.Write(msg);
        }
    }

    public static void ClosePort(this SerialPort serialPort, object? lockObject)
    {
        if (lockObject == null)
        {
            serialPort.Close();
        }
        else
        {
            lock (lockObject)
            {
                serialPort.Close();
            }
        }
    }

    public static bool OpenPort(this SerialPort serialPort)
    {
        if (serialPort.IsOpen)
        {
            return false;
        }

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
}