using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Data
{
    /// <summary>
    /// 创建一个工厂类，专门为设计时工具提供 AppDbContext 实例。这种方式更清晰，不会污染运行时代码。
    /// </summary>
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            // 这里的连接字符串应与您项目中的配置保持一致
            optionsBuilder.UseSqlite("Data Source=priceanalyzer.db");

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
