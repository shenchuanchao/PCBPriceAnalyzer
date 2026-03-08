using PCBPriceAnalyzer.Data.UnitOfWork;
using PCBPriceAnalyzer.Models.DTOs;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Models.Enums;
using PCBPriceAnalyzer.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace PCBPriceAnalyzer.Services.Implementations
{
    public class PriceService:IPriceService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PriceService(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public async Task<decimal> CalculatePriceIncreaseAsync(int materialId, TimeRange range)
        {
            // 查询历史价格，计算涨幅

            return 0;
        }

        /// <summary>
        /// 获取所有材料
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
        {
            var materials = await _unitOfWork.Materials.GetAllAsync();
            return materials;
        }
        /// <summary>
        /// 获取历史价格记录
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<IEnumerable<PriceRecordDto>> GetPriceHistoryAsync(int materialId, DateTime startDate, DateTime endDate)
        {
            var records = await _unitOfWork.PriceRecords.GetQueryable()
                .Include(r => r.Material)
                .Where(r => r.MaterialId == materialId && r.Date >= startDate && r.Date <= endDate)
                .OrderBy(r => r.Date)
                .ToListAsync();

            return records.Select(r => new PriceRecordDto
            {
                Id = r.Id,
                Date = r.Date,
                Price = r.Price,
                Unit = r.Material?.Unit ?? "",
                Source = r.Source
            });
        }

        public async Task<PriceRecordDto?> GetLatestPriceAsync(int materialId)
        {
            var latestRecord = await _unitOfWork.PriceRecords.GetQueryable()
                .Include(r => r.Material)
                .Where(r => r.MaterialId == materialId)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();

            if (latestRecord == null) return null;

            return new PriceRecordDto
            {
                Id = latestRecord.Id,
                Date = latestRecord.Date,
                Price = latestRecord.Price,
                Unit = latestRecord.Material?.Unit ?? "",
                Source = latestRecord.Source
            };
        }


    }
}
