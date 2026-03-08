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
    /// 材料仓储接口
    /// </summary>
    public interface IMaterialRepository : IRepository<Material>
    {
        // 可以在此添加 Material 特有的查询方法
        Task<Material> GetByNameAsync(string name);
        Task<IEnumerable<Material>> GetByTypeAsync(MaterialType type);
    }

}
