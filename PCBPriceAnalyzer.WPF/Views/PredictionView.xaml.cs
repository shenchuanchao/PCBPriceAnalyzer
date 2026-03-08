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
    /// PredictionView.xaml 的交互逻辑
    /// </summary>
    public partial class PredictionView : UserControl
    {
        public PredictionView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue is PredictionViewModel oldVm)
                oldVm.PropertyChanged -= OnViewModelPropertyChanged;
            if (e.NewValue is PredictionViewModel newVm)
                newVm.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(PredictionViewModel.HistoryPrices) ||
                e.PropertyName == nameof(PredictionViewModel.PredictedPrices))
            {
                Dispatcher.Invoke(UpdateChart);
            }
        }

        private void UpdateChart()
        {
            if (DataContext is not PredictionViewModel vm) return;
            if (vm.HistoryPrices == null && vm.PredictedPrices == null)
            {
                PricePlot.Plot.Clear();
                PricePlot.Refresh();
                return;
            }

            PricePlot.Plot.Clear();

            // 绘制历史价格
            if (vm.HistoryPrices != null && vm.HistoryPrices.Length > 0)
            {
                var histSignal = PricePlot.Plot.Add.Signal(vm.HistoryPrices);
                histSignal.Label = "历史价格";
                histSignal.Color = ScottPlot.Colors.Blue;
                histSignal.LineWidth = 2;
            }

            // 绘制预测价格
            if (vm.PredictedPrices != null && vm.PredictedPrices.Length > 0)
            {
                // 预测值应该接在历史数据后面
                double[] xValues;
                if (vm.HistoryPrices != null)
                {
                    xValues = Enumerable.Range(vm.HistoryPrices.Length, vm.PredictedPrices.Length)
                                        .Select(i => (double)i).ToArray();
                }
                else
                {
                    xValues = Enumerable.Range(0, vm.PredictedPrices.Length).Select(i => (double)i).ToArray();
                }

                var predScatter = PricePlot.Plot.Add.Scatter(xValues, vm.PredictedPrices);
                predScatter.Label = "预测价格";
                predScatter.Color = ScottPlot.Colors.Red;
                predScatter.LineWidth = 2;
                predScatter.MarkerSize = 5;
            }

            PricePlot.Plot.XLabel("时间（天）");
            PricePlot.Plot.YLabel("价格");
            PricePlot.Plot.Title($"{vm.SelectedMaterial?.Name} 价格预测");
            // 设置字体（支持中文）
            PricePlot.Plot.Font.Automatic();

            PricePlot.Plot.ShowLegend();
            PricePlot.Plot.Axes.AutoScale();
            PricePlot.Refresh();
        }

    }
}
