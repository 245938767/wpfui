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
            port.Read(buff, 0, count);
            if (count == 0)
            {
                receiveData.Invoke(buff);
            }
        };
    }

    public static void SendHexCRC(this SerialPort serialPort, string hexString, object? lockObject)
    {
        if (lockObject == null)
        {
            var checkCrc = hexString + CRCModelHelper.ToModbusCRC16(hexString);
            var sendBuffer = CRCModelHelper.StringToHexByte(checkCrc);
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
            serialPort.Write(sendBuffer, 0, sendBuffer.Length);
            return;
        }

        lock (lockObject)
        {
            var checkCrc = hexString + CRCModelHelper.ToModbusCRC16(hexString);
            var sendBuffer = CRCModelHelper.StringToHexByte(checkCrc);
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
            serialPort.Write(sendBuffer, 0, sendBuffer.Length);
        }
    }

    public static void SendStringMsg(this SerialPort serialPort, string msg, object? lockObject)
    {
        if (lockObject == null)
        {
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
            serialPort.Write(msg);
            return;
        }

        lock (lockObject)
        {
            serialPort.DiscardInBuffer();
            serialPort.DiscardOutBuffer();
            serialPort.Write(msg);
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