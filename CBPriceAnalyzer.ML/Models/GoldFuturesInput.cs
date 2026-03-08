using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBPriceAnalyzer.ML.Models
{
    public class GoldFuturesInput
    {
        public float DateIndex { get; set; }        // 可选：将日期转换为连续索引
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }             // 目标变量
        public float Volume { get; set; }
        public float Ma7 { get; set; }
        public float Ma30 { get; set; }
        public float Ma90 { get; set; }
        public float DailyReturn { get; set; }
        public float Volatility7 { get; set; }
        public float Volatility30 { get; set; }
        public float Rsi { get; set; }
        public float Macd { get; set; }
        public float MacdSignal { get; set; }
        public float BbUpper { get; set; }
        public float BbLower { get; set; }
    }

    public class GoldFuturesPrediction
    {
        public float Score { get; set; } // 预测的 Close 价格
    }


}
