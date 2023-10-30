namespace Wpf.Ui.Demo.Mvvm.Helpers;

public static class EnumExtension
{
    public static IEnumerable<T> GetValues<T>()
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }
}