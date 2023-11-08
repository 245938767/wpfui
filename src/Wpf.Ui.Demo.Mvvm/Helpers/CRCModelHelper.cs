using System.Text;

public class CRCModelHelper
{
    #region CRC16

    public static byte[] CRC16(byte[] data)
    {
        int len = data.Length;
        if (len > 0)
        {
            ushort crc = 0xFFFF;

            for (int i = 0; i < len; i++)
            {
                crc = (ushort)(crc ^ data[i]);
                for (int j = 0; j < 8; j++)
                {
                    crc = (crc & 1) != 0 ? (ushort)(crc >> 1 ^ 0xA001) : (ushort)(crc >> 1);
                }
            }

            byte hi = (byte)((crc & 0xFF00) >> 8); //高位置
            byte lo = (byte)(crc & 0x00FF); //低位置

            return new byte[] { hi, lo };
        }

        return new byte[] { 0, 0 };
    }

    #endregion

    #region ToCRC16

    public static string ToCRC16(string content)
    {
        return ToCRC16(content, Encoding.UTF8);
    }

    public static string ToCRC16(string content, bool isReverse)
    {
        return ToCRC16(content, Encoding.UTF8, isReverse);
    }

    public static string ToCRC16(string content, Encoding encoding)
    {
        return ByteToString(CRC16(encoding.GetBytes(content)), true);
    }

    public static string ToCRC16(string content, Encoding encoding, bool isReverse)
    {
        return ByteToString(CRC16(encoding.GetBytes(content)), isReverse);
    }

    public static string ToCRC16(byte[] data)
    {
        return ByteToString(CRC16(data), true);
    }

    public static string ToCRC16(byte[] data, bool isReverse)
    {
        return ByteToString(CRC16(data), isReverse);
    }

    #endregion

    #region ToModbusCRC16

    public static string ToModbusCRC16(string s)
    {
        return ToModbusCRC16(s, true);
    }

    public static string ToModbusCRC16(string s, bool isReverse)
    {
        return ByteToString(CRC16(StringToHexByte(s)), isReverse);
    }

    public static string ToModbusCRC16(byte[] data)
    {
        return ToModbusCRC16(data, true);
    }

    public static string ToModbusCRC16(byte[] data, bool isReverse)
    {
        return ByteToString(CRC16(data), isReverse);
    }

    #endregion

    #region ByteToString

    public static string ByteToString(byte[] arr, bool isReverse)
    {
        try
        {
            byte hi = arr[0], lo = arr[1];
            return Convert.ToString(isReverse ? hi + lo * 0x100 : hi * 0x100 + lo, 16).ToUpper().PadLeft(4, '0');
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string ByteToString(byte[] arr)
    {
        try
        {
            return ByteToString(arr, true);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion

    #region StringToHexString

    public static string StringToHexString(string str)
    {
        StringBuilder s = new StringBuilder();
        foreach (short c in str.ToCharArray())
        {
            s.Append(c.ToString("X4"));
        }

        return s.ToString();
    }

    #endregion

    #region StringToHexByte

    private static string ConvertChinese(string str)
    {
        StringBuilder s = new StringBuilder();
        foreach (short c in str.ToCharArray())
        {
            if (c <= 0 || c >= 127)
            {
                s.Append(c.ToString("X4"));
            }
            else
            {
                s.Append((char)c);
            }
        }

        return s.ToString();
    }

    private static string FilterChinese(string str)
    {
        StringBuilder s = new StringBuilder();
        foreach (short c in str.ToCharArray())
        {
            if (c > 0 && c < 127)
            {
                s.Append((char)c);
            }
        }

        return s.ToString();
    }

    /// <summary>
    /// 字符串转16进制字符数组
    /// </summary>
    /// <param name="hex"></param>
    /// <returns></returns>
    public static byte[] StringToHexByte(string str)
    {
        return StringToHexByte(str, false);
    }

    /// <summary>
    /// 字符串转16进制字符数组
    /// </summary>
    /// <param name="str"></param>
    /// <param name="isFilterChinese">是否过滤掉中文字符</param>
    /// <returns></returns>
    public static byte[] StringToHexByte(string str, bool isFilterChinese)
    {
        string hex = isFilterChinese ? FilterChinese(str) : ConvertChinese(str);

        //清除所有空格
        hex = hex.Replace(" ", "");
        //若字符个数为奇数，补一个0
        hex += hex.Length % 2 != 0 ? "0" : "";

        byte[] result = new byte[hex.Length / 2];
        for (int i = 0, c = result.Length; i < c; i++)
        {
            result[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }

        return result;
    }

    #endregion

    #region CheckCRC

    public static byte[]? CheckCrc(byte[]? bytes)
    {
        if (bytes == null || bytes.Length < 2)
        {
            return null;
        }

        var checkCrc = new[] { bytes[bytes.Length - 2], bytes[bytes.Length - 1] };
        var checkData = new byte[bytes.Length - 2];
        var returnData = new byte[bytes.Length - 5];

        for (var i = 0; i < bytes.Length - 2; i++)
        {
            checkData[i] = bytes[i];
        }

        for (var i = 3; i < bytes.Length - 2; i++)
        {
            returnData[i - 3] = bytes[i];
        }

        var crc16 = CRC16(checkData);
        var q = from crc in crc16 join checkCRC in checkCrc on crc equals checkCRC select crc;
        var flag = crc16.Length == checkCrc.Length && q.Count() == crc16.Length;
        return flag ? returnData : null;
    }

    #endregion

    #region 工装数据转换

    /// <summary>
    /// 转换Byte数组为Float
    /// </summary>
    /// <param name="bytes">转换的数据</param>
    /// <returns></returns>
    public static List<float> TranlationByteForFloat(byte[] bytes)
    {
        var outInfo = BitConverter.ToString(CheckCrc(bytes) ?? Array.Empty<byte>());
        return TranlationHexToFloatDataList(outInfo.Length > 0 ? outInfo.Split('-').ToList() : null, bytes.Length);
    }

    /// <summary>
    /// 把16进制的数据转换为Float
    /// </summary>
    /// <param name="datas">需要转换的数据</param>
    /// <param name="receiveCount">接收的原始数据位包括校验位</param>
    private static List<float> TranlationHexToFloatDataList(List<string>? datas, int receiveCount)
    {
        if (datas == null)
        {
            return new List<float>();
        }

        var listFloat = new List<float>();
        var count = 0;
        for (var i = 0; i < (receiveCount - 5) / 4; i++)
        {
            var hexStringList = new List<string>();
            var c1 = datas[count++];
            var c2 = datas[count++];
            var c3 = datas[count++];
            var c4 = datas[count++];

            // 数据交换
            hexStringList.Add(c3);
            hexStringList.Add(c4);
            hexStringList.Add(c1);
            hexStringList.Add(c2);
            listFloat.Add(ConvertFloat(String.Join(String.Empty, hexStringList), true));
        }
        return listFloat;
    }

    private static float ConvertFloat(string data, bool isReverse = true)
    {
        var dataByte = StringToHexByte(data);

        // 当前为左高右低，装换为左低右高
        var f = BitConverter.ToSingle(isReverse ? dataByte.Reverse().ToArray() : dataByte, 0);

        return f;
    }
#endregion
}