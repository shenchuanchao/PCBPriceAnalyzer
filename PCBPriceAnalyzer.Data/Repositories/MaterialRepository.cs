using Microsoft.EntityFrameworkCore;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Data.Repositories
{
    /// <summary>
    /// 材料仓储实现
    /// </summary>
    public class MaterialRepository : Repository<Material>, IMaterialRepository
    {
        public MaterialRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Material> GetByNameAsync(string name)
        {
            return await _dbSet.FirstOrDefaultAsync(m => m.Name == name);
        }

        public async Task<IEnumerable<Material>> GetByTypeAsync(MaterialType type)
        {
            return await _dbSet.Where(m => m.Type == type).ToListAsync();
        }
    }

}
