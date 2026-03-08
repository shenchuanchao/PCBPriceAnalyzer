using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCBPriceAnalyzer.Models.Entities;
using System.Collections.ObjectModel;

namespace PCBPriceAnalyzer.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableObject? _currentViewModel;

        // 导航项集合
        public ObservableCollection<NavigationItem> NavigationItems { get; } = new();

        public MainViewModel(
            DashboardViewModel dashboardVM,
            PriceHistoryViewModel priceHistoryVM,
            PredictionViewModel predictionVM,
            DataSourceViewModel dataSourceVM,
            SettingsViewModel settingsVM)
        {
            // 初始化导航项
            NavigationItems.Add(new NavigationItem("仪表盘", dashboardVM));
            NavigationItems.Add(new NavigationItem("历史价格", priceHistoryVM));
            NavigationItems.Add(new NavigationItem("价格预测", predictionVM));
            NavigationItems.Add(new NavigationItem("数据源管理", dataSourceVM));
            NavigationItems.Add(new NavigationItem("设置", settingsVM));

            // 默认选中第一个
            NavigateToCommand.Execute(NavigationItems[0]);
        }

        [RelayCommand]
        private void NavigateTo(NavigationItem? item)
        {
            if (item != null)
                CurrentViewModel = item.ViewModel;
        }
    }

    // 辅助类：导航项
    public class NavigationItem
    {
        public string DisplayName { get; }
        public ObservableObject ViewModel { get; }

        public NavigationItem(string displayName, ObservableObject viewModel)
        {
            DisplayName = displayName;
            ViewModel = viewModel;
        }
    }
}
