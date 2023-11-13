using System.ComponentModel;

namespace Wpf.Ui.Demo.Mvvm.Helpers;

public static class EnumExtension
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
    /// <summary>
    /// 获得枚举类型中<see cref="DescriptionAttribute"/>的内容
    /// </summary>
    /// <param name="val">需要获取的枚举类型</param>
    /// <returns><see cref="DescriptionAttribute"/>内的信息</returns>
    public static string ToDescription(this Enum val)
    {
        var type = val.GetType();

        var memberInfo = type.GetMember(val.ToString());

        var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

        return attributes.Length != 1
            ?
            //如果没有定义描述，就把当前枚举值的对应名称返回
            val.ToString()
            : (attributes.Single() as DescriptionAttribute)?.Description ?? "";
    }
}