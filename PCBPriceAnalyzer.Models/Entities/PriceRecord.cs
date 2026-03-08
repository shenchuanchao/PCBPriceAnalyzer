using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Entities
{
    /// <summary>
    /// 历史价格记录
    /// </summary>
    public class PriceRecord
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public Material Material { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
        public string Source { get; set; }        // 具体来源（如LME官网）
    }
}
