using CBPriceAnalyzer.ML.Models;
using Microsoft.ML;
using PCBPriceAnalyzer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CBPriceAnalyzer.ML
{
    public class GoldPricePredictor
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;

        public GoldPricePredictor()
        {
            _mlContext = new MLContext(seed: 42);
        }

        /// <summary>
        /// 使用历史数据训练模型
        /// </summary>
        /// <param name="data">按时间排序的历史数据</param>
        public void Train(IEnumerable<GoldFuturesRecord> data)
        {
            // 转换为 IDataView
            var inputData = data.Select(r => new GoldFuturesInput
            {
                DateIndex = (float)(r.Date - data.First().Date).TotalDays,
                Open = (float)r.Open,
                High = (float)r.High,
                Low = (float)r.Low,
                Close = (float)r.Close,
                Volume = (float)r.Volume,
                Ma7 = (float)r.Ma7,
                Ma30 = (float)r.Ma30,
                Ma90 = (float)r.Ma90,
                DailyReturn = (float)r.DailyReturn,
                Volatility7 = (float)r.Volatility7,
                Volatility30 = (float)r.Volatility30,
                Rsi = (float)r.Rsi,
                Macd = (float)r.Macd,
                MacdSignal = (float)r.MacdSignal,
                BbUpper = (float)r.BbUpper,
                BbLower = (float)r.BbLower
            }).ToList();

            IDataView dataView = _mlContext.Data.LoadFromEnumerable(inputData);

            // 定义数据加载和特征管道
            var pipeline = _mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: nameof(GoldFuturesInput.Close))
                .Append(_mlContext.Transforms.Concatenate("Features",
                    nameof(GoldFuturesInput.DateIndex),
                    nameof(GoldFuturesInput.Open),
                    nameof(GoldFuturesInput.High),
                    nameof(GoldFuturesInput.Low),
                    nameof(GoldFuturesInput.Volume),
                    nameof(GoldFuturesInput.Ma7),
                    nameof(GoldFuturesInput.Ma30),
                    nameof(GoldFuturesInput.Ma90),
                    nameof(GoldFuturesInput.DailyReturn),
                    nameof(GoldFuturesInput.Volatility7),
                    nameof(GoldFuturesInput.Volatility30),
                    nameof(GoldFuturesInput.Rsi),
                    nameof(GoldFuturesInput.Macd),
                    nameof(GoldFuturesInput.MacdSignal),
                    nameof(GoldFuturesInput.BbUpper),
                    nameof(GoldFuturesInput.BbLower)))
                .Append(_mlContext.Regression.Trainers.FastTree());

            // 训练模型
            _model = pipeline.Fit(dataView);
        }

        /// <summary>
        /// 预测未来的价格
        /// </summary>
        public float Predict(GoldFuturesInput input)
        {
            if (_model == null) throw new InvalidOperationException("模型未训练或加载");
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<GoldFuturesInput, GoldFuturesPrediction>(_model);
            var prediction = predictionEngine.Predict(input);
            return prediction.Score;
        }

        public void SaveModel(string modelPath)
        {
            _mlContext.Model.Save(_model, null, modelPath);
        }

        public void LoadModel(string modelPath)
        {
            _model = _mlContext.Model.Load(modelPath, out _);
        }
    }
}
