using Microsoft.EntityFrameworkCore;
using PCBPriceAnalyzer.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Data
{
    /// <summary>
    /// DbContext 主类
    /// </summary>
    public class AppDbContext : DbContext
    {
        // 添加带参数的构造函数，接收DbContextOptions
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Material> Materials { get; set; }
        public DbSet<PriceRecord> PriceRecords { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<GoldFuturesRecord> GoldFuturesRecords { get; set; }

        // 可保留 OnConfiguring 作为后备配置（如果未通过依赖注入提供选项）
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // 如果尚未配置（即没有通过 AddDbContext 传入选项），则使用默认 SQLite 连接字符串
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=priceanalyzer.db");
            }
        }
    }
}
