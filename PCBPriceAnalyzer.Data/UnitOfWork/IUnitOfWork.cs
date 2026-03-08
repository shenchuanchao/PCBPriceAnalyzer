using PCBPriceAnalyzer.Data.Repositories;
using PCBPriceAnalyzer.Models.Entities;

namespace PCBPriceAnalyzer.Data.UnitOfWork
{
    /// <summary>
    /// 工作单元接口
    /// </summary>
    public interface IUnitOfWork: IDisposable
    {
        IMaterialRepository Materials { get; } 
        IRepository<PriceRecord> PriceRecords { get; }
        Task<int> CompleteAsync();

    }
}
