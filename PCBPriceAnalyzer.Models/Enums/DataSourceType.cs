using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Enums
{
    /// <summary>
    /// 数据源类型枚举
    /// </summary>
    public enum DataSourceType
    {
        Website,        // 网页爬虫
        Api,            // API接口
        File            // 文件导入
    }
}
