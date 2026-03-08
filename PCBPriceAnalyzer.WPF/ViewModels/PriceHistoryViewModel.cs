using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCBPriceAnalyzer.Models.DTOs;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Services.Interfaces;
using System.Collections.ObjectModel;

namespace PCBPriceAnalyzer.WPF.ViewModels
{
    public partial class PriceHistoryViewModel : ObservableObject
    {
        private readonly IPriceService _priceService;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<Material> _materials = new();

        [ObservableProperty]
        private Material _selectedMaterial;

        [ObservableProperty]
        private DateTime _startDate = DateTime.Now.AddMonths(-1); // 默认一个月前

        [ObservableProperty]
        private DateTime _endDate = DateTime.Now;

        [ObservableProperty]
        private ObservableCollection<PriceRecordDto> _priceRecords = new();

        // 用于图表的数组
        [ObservableProperty]
        private double[] _chartPrices;

        [ObservableProperty]
        private double[] _chartDates; // 可选的日期位置（用索引或时间戳）

        public PriceHistoryViewModel(IPriceService priceService)
        {
            _priceService = priceService;
            LoadMaterialsAsync();
        }

        [RelayCommand]
        private async Task LoadMaterialsAsync()
        {
            try
            {
                var materials = await _priceService.GetAllMaterialsAsync(); // 需要 IPriceService 实现此方法
                Materials = new ObservableCollection<Material>(materials);
                SelectedMaterial = Materials.FirstOrDefault();
            }
            catch (Exception ex)
            {
                // 实际项目中应使用日志或通知
                System.Diagnostics.Debug.WriteLine($"加载材料失败: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task LoadHistoryAsync()
        {
            if (SelectedMaterial == null) return;
            if (IsLoading) return;
            IsLoading = true;

            try
            {
                var records = await _priceService.GetPriceHistoryAsync(
                    SelectedMaterial.Id,
                    StartDate,
                    EndDate);

                PriceRecords = new ObservableCollection<PriceRecordDto>(records);

                // 准备图表数据
                ChartPrices = records.OrderBy(r => r.Date).Select(r => (double)r.Price).ToArray();
                // 如果希望 X 轴显示日期标签，可以生成对应的标签数组（用于 ScottPlot 的 ticks）
                // 这里简单使用索引作为 X 轴
                ChartDates = Enumerable.Range(0, ChartPrices.Length).Select(i => (double)i).ToArray();
            }
            catch (Exception ex)
            {
                // 错误处理
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 当选择材料变化时自动加载历史数据
        partial void OnSelectedMaterialChanged(Material value)
        {
            if (value != null)
                LoadHistoryAsync();
        }

        // 可选的：当日期变化时也自动加载（如果不需要自动，则保留手动查询按钮）
    }
}
