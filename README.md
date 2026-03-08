## 项目结构
``` text
PCBPriceAnalyzer/
├── PCBPriceAnalyzer.Models               # 实体、DTO、枚举
├── PCBPriceAnalyzer.Data                  # EF Core DbContext、Repository、迁移
├── PCBPriceAnalyzer.Services               # 业务逻辑、价格计算、预测服务接口
├── PCBPriceAnalyzer.ML                     # ML.NET 模型训练、预测引擎
├── PCBPriceAnalyzer.DataCollection          # 数据采集（爬虫/API定时任务）
├── PCBPriceAnalyzer.Common                  # 工具类、常量、日志扩展
├── PCBPriceAnalyzer.WPF                     # WPF 客户端（MVVM）
└── PCBPriceAnalyzer.Tests                   # 单元测试项目（xUnit）
```

1. PCBPriceAnalyzer.Models (类库 .NET 6/7/8)
用途：定义核心数据模型、数据库实体、DTO以及ML.NET所需的输入/输出类。

文件夹/文件结构：

``` text
/Entities
    - Material.cs          # 原材料（如覆铜板、铜箔、油墨）
    - PriceRecord.cs       # 历史价格记录
    - Category.cs          # 分类（板材、金属、油墨等）
    - Supplier.cs          # 供应商（可选）
/DTOs
    - PriceDto.cs          # 用于UI展示的价格数据传输对象
    - PricePredictionDto.cs # 预测结果DTO
/Enums
    - MaterialType.cs      # 材料类型枚举
    - TimeRange.cs         # 时间范围（日/周/月/年）
/ML
    - PriceInput.cs        # ML.NET 模型输入（特征：日期、历史价格等）
    - PriceOutput.cs       # ML.NET 模型输出（预测值、置信区间）
```
2. PCBPriceAnalyzer.Data (类库)
用途：封装数据库访问，使用 EF Core + SQLite。

NuGet包：

Microsoft.EntityFrameworkCore.Sqlite

Microsoft.EntityFrameworkCore.Tools (用于迁移)

文件夹/文件结构：

``` text
/AppDbContext.cs          # DbContext 主类
/Repositories
    - IRepository.cs       # 泛型仓储接口
    - Repository.cs        # 泛型仓储实现
    - IMaterialRepository.cs  # 材料仓储接口（可自定义方法）
    - MaterialRepository.cs   # 材料仓储实现
/UnitOfWork
    - IUnitOfWork.cs       # 工作单元接口
    - UnitOfWork.cs        # 工作单元实现（包含所有仓储实例）
/Migrations                # 自动生成的迁移文件
```

3. PCBPriceAnalyzer.Services (类库)
用途：业务逻辑层，提供价格涨幅计算、预测调用、报表生成等服务。

NuGet包：

CommunityToolkit.Mvvm (用于MVVM工具，但此处Services不需要，WPF才需要)

EPPlus (导出Excel)

iTextSharp (导出PDF)

文件夹/文件结构：

``` text
/Interfaces
    - IPriceService.cs          # 价格查询、涨幅计算
    - IPredictionService.cs     # 价格预测接口
    - IReportService.cs         # 报表导出
    - IDataImportService.cs     # 数据导入（手动录入）
/Implementations
    - PriceService.cs
    - PredictionService.cs      # 调用ML项目
    - ReportService.cs
    - DataImportService.cs
/DTOs
    - PriceTrendDto.cs          # 涨幅趋势结果（用于图表）
```

4. PCBPriceAnalyzer.ML (类库)
用途：封装ML.NET模型的训练、加载和预测逻辑。

NuGet包：

Microsoft.ML

Microsoft.ML.TimeSeries (如果使用时序预测)

文件夹/文件结构：

``` text
/Models
    - PricePredictionModel.cs   # 模型训练器/预测器
/TrainedModels
    - material_price_model.zip  # 训练好的模型文件（输出目录）
```

注意：预测服务通过 IPredictionService 接口对外提供，内部调用 PricePredictionModel。

5. PCBPriceAnalyzer.DataCollection (可以是控制台应用或后台服务)
用途：定时从网络采集原材料价格数据，存入数据库。

NuGet包：

HtmlAgilityPack (解析HTML)

PuppeteerSharp (处理动态页面)

Quartz.NET (定时任务)

文件结构：

``` text
/Collectors
    - ILmeCollector.cs          # 伦敦金属交易所采集接口
    - LmeCollector.cs            # 实现
    - ShfeCollector.cs           # 上海期货交易所
    - PcbIndustryNewsCollector.cs # 行业资讯网站
/Scheduler
    - DataCollectionJob.cs       # Quartz作业，调用各个采集器
/Program.cs                      # 主入口（如果是控制台应用）
```

典型实现：使用 HttpClient + HtmlAgilityPack 抓取表格数据，解析后调用 IPriceService 的导入方法保存。

6. PCBPriceAnalyzer.Common (类库)
用途：存放跨项目共享的工具类和常量。

文件：

``` text
/Extensions
    - DateTimeExtensions.cs      # 日期格式化、计算周期
    - DecimalExtensions.cs       # 百分比计算
/Helpers
    - FileHelper.cs              # 文件读写
    - LogHelper.cs               # 封装日志（可用ILogger接口）
/Constants
    - AppConstants.cs            # 数据库连接字符串、API密钥等（从配置文件读取）
```

7. PCBPriceAnalyzer.WPF (WPF应用程序)
用途：桌面客户端，提供用户界面和交互。

NuGet包：

CommunityToolkit.Mvvm (MVVM框架)

Microsoft.Extensions.DependencyInjection (依赖注入)

LiveCharts2 (图表)

Microsoft.EntityFrameworkCore.Design (用于迁移)

项目结构（遵循MVVM）：

``` text
/Views
    - MainWindow.xaml
    - DashboardView.xaml
    - PriceHistoryView.xaml
    - PredictionView.xaml
    - SettingsView.xaml
/ViewModels
    - MainViewModel.cs
    - DashboardViewModel.cs
    - PriceHistoryViewModel.cs
    - PredictionViewModel.cs
    - SettingsViewModel.cs
    - ViewModelBase.cs (继承 ObservableObject)
/Models (UI模型，可引用Entities/DTOs)
/Converters
    - PriceToColorConverter.cs   # 根据涨幅改变颜色
/Services (WPF特定的服务，如对话框、导航)
    - IDialogService.cs
    - DialogService.cs
    - INavigationService.cs
    - NavigationService.cs
/App.xaml
/App.xaml.cs (设置依赖注入容器)
```

8. PCBPriceAnalyzer.Tests (xUnit测试项目)
用途：对Services、ML、Data等关键逻辑进行单元测试。

引用：所有需要测试的项目，以及Moq (模拟仓储)。

测试类：

* PriceServiceTests.cs
* PredictionServiceTests.cs
* RepositoryTests.cs (使用内存数据库)



