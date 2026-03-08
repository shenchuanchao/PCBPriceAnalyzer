using PCBPriceAnalyzer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Interfaces
{
    public interface IGoldFuturesService
    {
        /// <summary>
        /// 导入CSV文件中的黄金期货数据，解析后存储到数据库中
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Task ImportFromCsvAsync(string filePath);
        Task<IEnumerable<GoldFuturesRecord>> GetAllRecordsAsync();
        // 其他查询方法...
    }
}
