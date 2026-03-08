using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Entities
{
    /// <summary>
    /// 黄金期货实体类
    /// </summary>
    public class GoldFuturesRecord
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public decimal AdjClose { get; set; }
        public long Volume { get; set; }
        public decimal Ma7 { get; set; }
        public decimal Ma30 { get; set; }
        public decimal Ma90 { get; set; }
        public decimal DailyReturn { get; set; }
        public decimal Volatility7 { get; set; }
        public decimal Volatility30 { get; set; }
        public decimal Rsi { get; set; }
        public decimal Macd { get; set; }
        public decimal MacdSignal { get; set; }
        public decimal BbUpper { get; set; }
        public decimal BbLower { get; set; }
    }

}
