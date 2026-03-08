using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Interfaces
{
    /// <summary>
    /// 价格预测接口
    /// </summary>
    public interface IPredictionService
    {
        bool IsGoldModelTrained { get; }
        Task TrainGoldModelAsync();
        Task<decimal> PredictGoldPriceAsync(DateTime forDate);

    }
}
