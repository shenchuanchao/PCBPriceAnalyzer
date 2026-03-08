using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Models.Enums;
using PCBPriceAnalyzer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PCBPriceAnalyzer.WPF.ViewModels
{
    public partial class PredictionViewModel : ObservableObject
    {
        private readonly IPriceService _priceService;
        private readonly IPredictionService _predictionService;

        [ObservableProperty]
        private ObservableCollection<Material> _materials = new();

        [ObservableProperty]
        private Material _selectedMaterial;

        [ObservableProperty]
        private DateTime _predictionDate = DateTime.Now.AddDays(1); // 默认预测明天

        [ObservableProperty]
        private decimal _predictedPrice;

        [ObservableProperty]
        private bool _isLoading;

        // 历史价格数据（用于图表）
        [ObservableProperty]
        private double[] _historyPrices;

        [ObservableProperty]
        private double[] _historyDates; // 索引或日期序号

        // 预测值（用于图表）
        [ObservableProperty]
        private double[] _predictedPrices;

        [ObservableProperty]
        private double[] _predictedDates;

        public PredictionViewModel(IPriceService priceService, IPredictionService predictionService)
        {
            _priceService = priceService;
            _predictionService = predictionService;
            LoadMaterialsAsync();
        }

        [RelayCommand]
        private async Task LoadMaterialsAsync()
        {
            IsLoading = true;
            var materials = await _priceService.GetAllMaterialsAsync();
            Materials = new ObservableCollection<Material>(materials);
            // 默认选中金（如果有）
            SelectedMaterial = Materials.FirstOrDefault(m => m.Type == MaterialType.Gold);
            IsLoading = false;
        }

        partial void OnSelectedMaterialChanged(Material value)
        {
            // 当材料改变时，清空之前的结果
            PredictedPrice = 0;
            HistoryPrices = null;
            PredictedPrices = null;
        }

        [RelayCommand]
        private async Task PredictAsync()
        {
            if (SelectedMaterial == null)
            {
                MessageBox.Show("请先选择材料", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (PredictionDate <= DateTime.Now.Date)
            {
                MessageBox.Show("预测日期必须晚于今天", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;
            try
            {
                if (SelectedMaterial.Type == MaterialType.Gold)
                {
                    // 使用黄金预测服务
                    await PredictGoldAsync();
                }
                else
                {
                    // 其他材料暂用简单模拟（后续可扩展）
                    await PredictOtherMaterialAsync();
                }

                // 加载历史数据用于图表（最近30天）
                await LoadHistoryForChartAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"预测失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task PredictGoldAsync()
        {
            // 确保模型已训练（如果未训练则自动训练）
            if (!_predictionService.IsGoldModelTrained)
            {
                await _predictionService.TrainGoldModelAsync();
            }

            var price = await _predictionService.PredictGoldPriceAsync(PredictionDate);
            PredictedPrice = price;

            // 生成预测序列（未来7天）
            var futurePrices = new double[7];
            var futureDates = new double[7];
            for (int i = 0; i < 7; i++)
            {
                var date = PredictionDate.AddDays(i);
                futurePrices[i] = (double)await _predictionService.PredictGoldPriceAsync(date);
                futureDates[i] = i + 1; // 或使用日期序号
            }
            PredictedPrices = futurePrices;
            PredictedDates = futureDates;
        }

        private async Task PredictOtherMaterialAsync()
        {
            // 模拟：基于最近一次价格加上随机波动
            var latestPrice = await _priceService.GetLatestPriceAsync(SelectedMaterial.Id);
            if (latestPrice == null)
            {
                throw new Exception("无历史价格数据");
            }

            var random = new Random();
            var change = (decimal)(random.NextDouble() * 0.1 - 0.05); // -5% ~ +5%
            PredictedPrice = latestPrice.Price * (1 + change);

            // 模拟未来7天
            var prices = new double[7];
            var dates = new double[7];
            var basePrice = (double)latestPrice.Price;
            for (int i = 0; i < 7; i++)
            {
                var factor = 1 + (random.NextDouble() * 0.1 - 0.05);
                prices[i] = basePrice * factor;
                dates[i] = i + 1;
            }
            PredictedPrices = prices;
            PredictedDates = dates;
        }

        private async Task LoadHistoryForChartAsync()
        {
            if (SelectedMaterial == null) return;

            var endDate = DateTime.Now.Date;
            var startDate = endDate.AddDays(-30);
            var history = await _priceService.GetPriceHistoryAsync(SelectedMaterial.Id, startDate, endDate);
            var ordered = history.OrderBy(h => h.Date).ToList();

            HistoryPrices = ordered.Select(h => (double)h.Price).ToArray();
            // X 轴可以使用索引或日期字符串，这里简单用索引
            HistoryDates = Enumerable.Range(0, HistoryPrices.Length).Select(i => (double)i).ToArray();
        }


    }
}
