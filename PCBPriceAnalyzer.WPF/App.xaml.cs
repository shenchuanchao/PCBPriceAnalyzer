using CBPriceAnalyzer.ML;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PCBPriceAnalyzer.Data;
using PCBPriceAnalyzer.Data.Repositories;
using PCBPriceAnalyzer.Data.UnitOfWork;
using PCBPriceAnalyzer.Services.Implementations;
using PCBPriceAnalyzer.Services.Interfaces;
using PCBPriceAnalyzer.WPF.Services;
using PCBPriceAnalyzer.WPF.ViewModels;
using PCBPriceAnalyzer.WPF.Views;
using System;
using System.Windows;

namespace PCBPriceAnalyzer.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider _serviceProvider;

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();
            //每次启动时自动迁移数据库
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                // 自动应用未完成的迁移
                dbContext.Database.Migrate();
                // 初始化数据库种子数据
                await SeedData.InitializeAsync(dbContext);
            }

            // 获取 MainWindow 并显示
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // 注册DbContext（单例或作用域）
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=priceanalyzer.db"), ServiceLifetime.Scoped);

            // 注册仓储和工作单元
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            // 注册机器学习模型
            services.AddSingleton<GoldPricePredictor>();

            // 注册服务
            services.AddScoped<IPriceService, PriceService>();
            services.AddScoped<IPredictionService, PredictionService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<ISettingsService, SettingsService>();
            services.AddScoped<IDataSourceService, DataSourceService>();
            services.AddScoped<IGoldFuturesService, GoldFuturesService>();

            // 注册ViewModel
            services.AddSingleton<MainWindow>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<PriceHistoryViewModel>();
            services.AddTransient<PredictionViewModel>();
            services.AddTransient<SettingsViewModel>();
            services.AddTransient<DataSourceViewModel>();
            services.AddSingleton<MainViewModel>(); // 主视图模型作为单例
            // 导航服务等
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IDialogService, DialogService>();

            // 注册WPF窗口
            services.AddSingleton<MainWindow>();
        }
    }

}
