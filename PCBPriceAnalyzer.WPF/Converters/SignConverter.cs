using System.Globalization;
using System.Windows.Data;

namespace PCBPriceAnalyzer.WPF.Converters
{
    /// <summary>
    /// 添加辅助转换器
    /// </summary>
    public class SignConverter : IValueConverter
    {
        public SignConverter() { }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is decimal d)
                return d >= 0 ? "▲" : "▼";
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}
