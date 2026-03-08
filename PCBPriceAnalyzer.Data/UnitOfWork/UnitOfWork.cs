using PCBPriceAnalyzer.Data.Repositories;
using PCBPriceAnalyzer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Data.UnitOfWork
{
    /// <summary>
    /// 工作单元实现（包含所有仓储实例）
    /// </summary>
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IMaterialRepository Materials { get; private set; }
        public IRepository<PriceRecord> PriceRecords { get; private set; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Materials = new MaterialRepository(_context);
            PriceRecords = new Repository<PriceRecord>(_context);
        }

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
