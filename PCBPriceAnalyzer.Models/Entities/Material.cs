using PCBPriceAnalyzer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Entities
{
    /// <summary>
    /// 原材料（如覆铜板、铜箔、油墨）
    /// </summary>
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public MaterialType Type { get; set; }
        public string Unit { get; set; }          // 如 "元/公斤"
        public string DataSource { get; set; }    // 数据来源URL或API
        public ICollection<PriceRecord> PriceRecords { get; set; }
    }
}
