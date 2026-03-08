using PCBPriceAnalyzer.WPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PCBPriceAnalyzer.WPF.Views
{
    /// <summary>
    /// PriceHistoryView.xaml 的交互逻辑
    /// </summary>
    public partial class PriceHistoryView : UserControl
    {
        public PriceHistoryView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            Loaded += OnLoaded;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is PriceHistoryViewModel oldVm)
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            if (e.NewValue is PriceHistoryViewModel newVm)
                newVm.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateChart();
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PriceHistoryViewModel.ChartPrices) ||
                e.PropertyName == nameof(PriceHistoryViewModel.IsLoading))
            {
                // 数据变化时更新图表
                Dispatcher.Invoke(UpdateChart);
            }
        }

        private void UpdateChart()
        {
            if (!IsLoaded) return;
            if (DataContext is not PriceHistoryViewModel vm) return;
            if (vm.ChartPrices == null || vm.ChartPrices.Length == 0)
            {
                PricePlot.Plot.Clear();
                PricePlot.Refresh();
                return;
            }

            // ScottPlot 5.x 语法
            PricePlot.Plot.Clear();
            var signal = PricePlot.Plot.Add.Signal(vm.ChartPrices);
            signal.Label = "价格";
            signal.Color = ScottPlot.Colors.Blue;
            signal.LineWidth = 2;


            PricePlot.Plot.YLabel("价格 (元/单位)");
            PricePlot.Plot.XLabel("时间 (天)");
            PricePlot.Plot.Title($"{vm.SelectedMaterial?.Name} 价格走势");

            // 设置中文显示
            PricePlot.Plot.Font.Automatic(); // 自动选择支持中文的字体
            // 如果希望 X 轴显示日期标签，可以使用自定义刻度
            // 这里简单使用索引

            PricePlot.Plot.Axes.AutoScale();
            PricePlot.Refresh();
        }

    }
}
