using PCBPriceAnalyzer.Data.Repositories;
using PCBPriceAnalyzer.Data.UnitOfWork;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Implementations
{
    public class DataSourceService : IDataSourceService
    {
        private readonly IRepository<DataSource> _dataSourceRepo;
        private readonly IUnitOfWork _unitOfWork;

        public DataSourceService(IRepository<DataSource> dataSourceRepo, IUnitOfWork unitOfWork)
        {
            _dataSourceRepo = dataSourceRepo;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<DataSource>> GetAllDataSourcesAsync()
        {
            return await _dataSourceRepo.GetAllAsync();
        }

        public async Task<DataSource> GetDataSourceByIdAsync(int id)
        {
            return await _dataSourceRepo.GetByIdAsync(id);
        }

        public async Task AddDataSourceAsync(DataSource dataSource)
        {
            await _dataSourceRepo.AddAsync(dataSource);
            await _unitOfWork.CompleteAsync();
        }

        public async Task UpdateDataSourceAsync(DataSource dataSource)
        {
            _dataSourceRepo.Update(dataSource);
            await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteDataSourceAsync(int id)
        {
            var ds = await _dataSourceRepo.GetByIdAsync(id);
            if (ds != null)
            {
                _dataSourceRepo.Remove(ds);
                await _unitOfWork.CompleteAsync();
            }
        }

        // 模拟测试连接
        public async Task TestConnectionAsync(DataSource dataSource)
        {
            // 这里可以实现实际的连接测试逻辑（如发送HTTP请求）
            await Task.Delay(1000); // 模拟耗时
            // 可以抛出异常表示失败，或者更新状态
        }

        // 模拟采集数据
        public async Task FetchDataAsync(DataSource dataSource)
        {
            // 实际采集逻辑
            await Task.Delay(2000);
            dataSource.LastFetchTime = DateTime.Now;
            dataSource.FetchStatus = "成功";
            _dataSourceRepo.Update(dataSource);
            await _unitOfWork.CompleteAsync();
        }
    }

}
