using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PCBPriceAnalyzer.WPF.Converters
{
    /// <summary>
    /// 控制转换器
    /// </summary>
    public class NullToTextConverter : IValueConverter
    {
        public static readonly NullToTextConverter Instance = new NullToTextConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // 如果 value 为 null，返回 parameter（新增模式），否则返回 "编辑数据源"
            return value == null ? parameter : "编辑数据源";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
