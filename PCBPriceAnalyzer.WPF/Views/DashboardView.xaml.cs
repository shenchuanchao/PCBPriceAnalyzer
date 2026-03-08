using PCBPriceAnalyzer.WPF.ViewModels;
using System.Windows;
using System.Windows.Controls;
using ScottPlot;

namespace PCBPriceAnalyzer.WPF.Views
{
    /// <summary>
    /// DashboardView.xaml 的交互逻辑
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public DashboardView()
        {
            InitializeComponent();
            Loaded += DashboardView_Loaded;
            // 监听DataContext变化，以便在ViewModel更新时刷新图表
            DataContextChanged += DashboardView_DataContextChanged;
        }

        private void DashboardView_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateChart();
        }

        private void DashboardView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateChart();
        }

        private void UpdateChart()
        {
            if (!IsLoaded) return;

            if (DataContext is DashboardViewModel vm && vm.PriceHistory != null)
            {
                // ScottPlot 5.x 语法
                PricePlot.Plot.Clear();

                var signal = PricePlot.Plot.Add.Signal(vm.PriceHistory);
                signal.Label = "铜价走势";
                signal.Color = ScottPlot.Colors.Blue;
                signal.LineWidth = 2;

                PricePlot.Plot.YLabel("价格 (元/单位)");
                PricePlot.Plot.XLabel("时间（天）");
                PricePlot.Plot.Title("铜价近期走势");
                // ***** 关键：添加这一行进行自动字体设置 *****
                PricePlot.Plot.Font.Automatic();

                // 自动调整坐标轴范围
                PricePlot.Plot.Axes.AutoScale();

                PricePlot.Refresh();
            }
        }

    }
}
