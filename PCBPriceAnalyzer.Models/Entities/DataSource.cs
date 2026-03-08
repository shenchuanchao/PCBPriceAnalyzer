using PCBPriceAnalyzer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Entities
{
    /// <summary>
    /// 数据源实体
    /// </summary>
    public class DataSource
    {
        public int Id { get; set; }
        public string Name { get; set; }               // 数据源名称，如 "LME官网"
        public DataSourceType Type { get; set; }        // 类型
        public string Url { get; set; }                 // 地址（如果是API或网页）
        public string ApiKey { get; set; }              // API密钥（可选）
        public bool IsEnabled { get; set; } = true;     // 是否启用
        public int UpdateInterval { get; set; } = 1440; // 更新间隔（分钟），默认24小时
        public DateTime? LastFetchTime { get; set; }    // 最后采集时间
        public string? FetchStatus { get; set; }         // 上次采集状态（成功/失败）
        public string Description { get; set; }          // 描述
    }
}
