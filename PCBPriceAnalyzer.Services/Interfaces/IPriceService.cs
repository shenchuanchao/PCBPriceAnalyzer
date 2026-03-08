using PCBPriceAnalyzer.Models.DTOs;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Interfaces
{
    /// <summary>
    /// 价格查询、涨幅计算
    /// </summary>
    public interface IPriceService
    {
        Task<decimal> CalculatePriceIncreaseAsync(int materialId, TimeRange range);

        /// <summary>
        /// 获取所有材料列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Material>> GetAllMaterialsAsync();

        /// <summary>
        /// 获取指定材料在日期范围内的历史价格
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<IEnumerable<PriceRecordDto>> GetPriceHistoryAsync(int materialId, DateTime startDate, DateTime endDate);

        Task<PriceRecordDto?> GetLatestPriceAsync(int materialId);

    }
}
