using Microsoft.EntityFrameworkCore;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Data
{
    public static class SeedData
    {
        public static async Task InitializeAsync(AppDbContext context)
        {
            // 如果数据库尚未创建，则创建
            await context.Database.MigrateAsync();

            // 检查是否有材料数据
            if (!context.Materials.Any())
            {
                // 添加默认材料
                context.Materials.AddRange(
                    new Material { Name = "铜", Type = MaterialType.Copper, Unit = "元/吨", DataSource = "系统默认" },
                    new Material { Name = "覆铜板", Type = MaterialType.CCL, Unit = "元/张", DataSource = "系统默认" },
                    new Material { Name = "金", Type = MaterialType.Gold, Unit = "元/克", DataSource = "系统默认" },
                    new Material { Name = "油墨", Type = MaterialType.Ink, Unit = "元/公斤", DataSource = "系统默认" },
                    new Material { Name = "银", Type = MaterialType.Silver, Unit = "元/克", DataSource = "系统默认" },
                    new Material { Name = "锡", Type = MaterialType.Tin, Unit = "元/吨", DataSource = "系统默认" },
                    new Material { Name = "半固化片", Type = MaterialType.Prepreg, Unit = "元/张", DataSource = "系统默认" }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
