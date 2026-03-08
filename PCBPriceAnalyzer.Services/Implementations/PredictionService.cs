using CBPriceAnalyzer.ML;
using CBPriceAnalyzer.ML.Models;
using PCBPriceAnalyzer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Implementations
{
    /// <summary>
    /// 调用ML项目
    /// </summary>
    public class PredictionService:IPredictionService
    {
        private readonly IGoldFuturesService _goldService;
        private readonly GoldPricePredictor _predictor;
        private readonly string _modelPath = "gold_model.zip";
        private bool _isGoldModelTrained;
        public bool IsGoldModelTrained => _isGoldModelTrained;

        public PredictionService(IGoldFuturesService goldService)
        {
            _goldService = goldService;
            _predictor = new GoldPricePredictor();
            if (File.Exists(_modelPath))
                _predictor.LoadModel(_modelPath);
        }

        public async Task TrainGoldModelAsync()
        {
            var data = await _goldService.GetAllRecordsAsync();
            // 确保数据按日期排序
            data = data.OrderBy(r => r.Date);
            _predictor.Train(data);
            _predictor.SaveModel(_modelPath);
             _isGoldModelTrained = true;
        }

        public async Task<decimal> PredictGoldPriceAsync(DateTime forDate)
        {
            // 获取最新一天的数据作为输入特征
            var latest = (await _goldService.GetAllRecordsAsync())
                         .OrderByDescending(r => r.Date)
                         .FirstOrDefault();
            if (latest == null) throw new Exception("无数据");

            // 构造输入（日期索引可以基于最新日期计算差值）
            var firstDate = (await _goldService.GetAllRecordsAsync()).Min(r => r.Date);
            var input = new GoldFuturesInput
            {
                DateIndex = (float)(forDate - firstDate).TotalDays,
                Open = (float)latest.Open,
                High = (float)latest.High,
                Low = (float)latest.Low,
                Close = (float)latest.Close,
                Volume = (float)latest.Volume,
                Ma7 = (float)latest.Ma7,
                Ma30 = (float)latest.Ma30,
                Ma90 = (float)latest.Ma90,
                DailyReturn = (float)latest.DailyReturn,
                Volatility7 = (float)latest.Volatility7,
                Volatility30 = (float)latest.Volatility30,
                Rsi = (float)latest.Rsi,
                Macd = (float)latest.Macd,
                MacdSignal = (float)latest.MacdSignal,
                BbUpper = (float)latest.BbUpper,
                BbLower = (float)latest.BbLower
            };
            var score = _predictor.Predict(input);
            return (decimal)score;
        }
    }
}
