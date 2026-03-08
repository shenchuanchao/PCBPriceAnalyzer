using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PCBPriceAnalyzer.Models.Settings;
using PCBPriceAnalyzer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.WPF.ViewModels
{
    public partial class SettingsViewModel : ObservableObject
    {
        private readonly ISettingsService _settingsService;

        [ObservableProperty] private string _theme;
        [ObservableProperty] private int _autoRefreshInterval;
        [ObservableProperty] private int _defaultMaterialId;
        [ObservableProperty] private bool _enablePriceAlert;
        [ObservableProperty] private decimal _priceAlertThreshold;
        [ObservableProperty] private bool _isLoading;

        public SettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            LoadSettingsCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadSettingsAsync()
        {
            IsLoading = true;
            var settings = await _settingsService.LoadSettingsAsync();
            Theme = settings.Theme;
            AutoRefreshInterval = settings.AutoRefreshInterval;
            DefaultMaterialId = settings.DefaultMaterialId;
            EnablePriceAlert = settings.EnablePriceAlert;
            PriceAlertThreshold = settings.PriceAlertThreshold;
            IsLoading = false;
        }

        [RelayCommand]
        private async Task SaveSettingsAsync()
        {
            var settings = new AppSettings
            {
                Theme = Theme,
                AutoRefreshInterval = AutoRefreshInterval,
                DefaultMaterialId = DefaultMaterialId,
                EnablePriceAlert = EnablePriceAlert,
                PriceAlertThreshold = PriceAlertThreshold
            };
            await _settingsService.SaveSettingsAsync(settings);
            // 可添加提示保存成功（如使用消息框或通知）
        }

    }
}
