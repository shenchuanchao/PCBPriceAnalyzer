using PCBPriceAnalyzer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Interfaces
{
    public interface IDataSourceService
    {
        Task<IEnumerable<DataSource>> GetAllDataSourcesAsync();
        Task<DataSource> GetDataSourceByIdAsync(int id);
        Task AddDataSourceAsync(DataSource dataSource);
        Task UpdateDataSourceAsync(DataSource dataSource);
        Task DeleteDataSourceAsync(int id);
        Task TestConnectionAsync(DataSource dataSource);  // 测试连接/采集
        Task FetchDataAsync(DataSource dataSource);       // 手动触发采集
    }
}
