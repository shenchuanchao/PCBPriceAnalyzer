using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PCBPriceAnalyzer.Services.Interfaces;

namespace PCBPriceAnalyzer.WPF.ViewModels
{
    public partial class DashboardViewModel : ObservableObject
    {
        private readonly IPriceService _priceService;
        private double[] _priceHistory;
        public double[] PriceHistory
        {
            get => _priceHistory;
            private set => SetProperty(ref _priceHistory, value);
        }
        // 如果是非等间距 X 轴，可以添加日期位置数组
        private double[] _datePositions;
        public double[] DatePositions
        {
            get => _datePositions;
            private set => SetProperty(ref _datePositions, value);
        }

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _welcomeMessage = $"欢迎使用PCB原材料价格分析系统 - {DateTime.Now:yyyy年MM月dd日}";

        // 热门材料价格卡片
        [ObservableProperty]
        private ObservableCollection<PriceCard> _priceCards = new();

      

        // 涨幅排行榜
        [ObservableProperty]
        private ObservableCollection<PriceTrendItem> _topGainers = new();

        public DashboardViewModel(IPriceService priceService)
        {
            _priceService = priceService;
            LoadDataAsync();
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            if (IsLoading) return;
            IsLoading = true;

            try
            {
                // 并行加载各类数据
                await Task.WhenAll(
                    LoadPriceCardsAsync(),
                    LoadChartDataAsync(),
                    LoadTopGainersAsync()
                );
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadPriceCardsAsync()
        {
            // 模拟从服务获取关键材料最新价格
            // 实际应从 _priceService 获取
            var cards = new ObservableCollection<PriceCard>
            {
                new PriceCard { MaterialName = "铜", LatestPrice = 68900m, ChangePercent = 1.2m, Unit = "元/吨" },
                new PriceCard { MaterialName = "覆铜板", LatestPrice = 185m, ChangePercent = -0.5m, Unit = "元/张" },
                new PriceCard { MaterialName = "金", LatestPrice = 412.5m, ChangePercent = 0.8m, Unit = "元/克" },
                new PriceCard { MaterialName = "油墨", LatestPrice = 85m, ChangePercent = 0.0m, Unit = "元/公斤" }
            };
            PriceCards = cards;
        }

        private async Task LoadChartDataAsync()
        {
            // 模拟过去30天铜价走势
            var prices = new double[] { 68200, 68350, 68100, 68400, 68600, 68800, 68900, 68700, 68650, 68850,
                                 69000, 69100, 68950, 68800, 68750, 68600, 68450, 68300, 68250, 68400,
                                 68500, 68600, 68700, 68800, 68900, 68850, 68700, 68600, 68500, 68400 };
            PriceHistory = prices;

            // 生成等间距的 X 轴位置（0, 1, 2...）
            DatePositions = Enumerable.Range(0, prices.Length).Select(i => (double)i).ToArray();
        }

        private async Task LoadTopGainersAsync()
        {
            // 模拟涨幅榜
            var items = new ObservableCollection<PriceTrendItem>
            {
                new PriceTrendItem { MaterialName = "锡", Price = 245000m, WeekChange = 3.2m, MonthChange = 5.1m },
                new PriceTrendItem { MaterialName = "银", Price = 5.8m, WeekChange = 2.1m, MonthChange = 4.3m },
                new PriceTrendItem { MaterialName = "铜", Price = 68900m, WeekChange = 1.5m, MonthChange = 2.8m },
                new PriceTrendItem { MaterialName = "覆铜板", Price = 185m, WeekChange = -0.3m, MonthChange = 1.2m },
                new PriceTrendItem { MaterialName = "半固化片", Price = 32m, WeekChange = 0.5m, MonthChange = 1.0m }
            };
            TopGainers = items;
        }
    }

    // 辅助模型
    public class PriceCard
    {
        public string MaterialName { get; set; }
        public decimal LatestPrice { get; set; }
        public decimal ChangePercent { get; set; }
        public string Unit { get; set; }
        public string ChangeColor => ChangePercent >= 0 ? "Green" : "Red";
        public string ChangeSymbol => ChangePercent >= 0 ? "▲" : "▼";
    }

    public class PriceTrendItem
    {
        public string MaterialName { get; set; }
        public decimal Price { get; set; }
        public decimal WeekChange { get; set; }
        public decimal MonthChange { get; set; }
        public string WeekChangeColor => WeekChange >= 0 ? "Green" : "Red";
        public string MonthChangeColor => MonthChange >= 0 ? "Green" : "Red";

    }
}
