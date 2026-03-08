using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.DTOs
{
    /// <summary>
    /// 价格记录数据传输对象，用于在 UI 层展示历史价格数据
    /// </summary>
    public class PriceRecordDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Unit { get; set; }      // 材料单位，如 "元/吨"
        public string Source { get; set; }    // 数据来源
    }

}
