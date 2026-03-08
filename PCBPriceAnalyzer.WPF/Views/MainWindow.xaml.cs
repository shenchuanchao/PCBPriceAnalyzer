using PCBPriceAnalyzer.WPF.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace PCBPriceAnalyzer.WPF.Views
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel; // 注入的 MainViewModel
        }

        private void NavigationItem_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is NavigationItem item)
            {
                if (DataContext is MainViewModel vm)
                {
                    vm.NavigateToCommand.Execute(item);
                }
            }
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("PCB原材料价格智能分析软件 v1.0\n基于WPF + ML.NET", "关于");
        }


    }
}
