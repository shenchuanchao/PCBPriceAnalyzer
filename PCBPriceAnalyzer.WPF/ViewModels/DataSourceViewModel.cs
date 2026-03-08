using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Win32;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Models.Enums;
using PCBPriceAnalyzer.Services.Interfaces;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace PCBPriceAnalyzer.WPF.ViewModels
{
    public partial class DataSourceViewModel : ObservableObject
    {
        private readonly IDataSourceService _dataSourceService;
        private readonly IGoldFuturesService _goldFuturesService;

        [ObservableProperty]
        private ObservableCollection<DataSource> _dataSources = new();

        [ObservableProperty]
        private DataSource _selectedDataSource;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isEditing;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private DataSourceType _type;

        [ObservableProperty]
        private string _url;

        [ObservableProperty]
        private string _apiKey;

        [ObservableProperty]
        private bool _isEnabled;

        [ObservableProperty]
        private int _updateInterval;

        [ObservableProperty]
        private string _description;

        // 用于下拉框的数据源类型列表
        public ObservableCollection<DataSourceType> DataSourceTypes { get; } = new(Enum.GetValues(typeof(DataSourceType)).Cast<DataSourceType>());
        /// <summary>
        /// 构造函数，注入数据源服务和黄金期货服务，并加载数据源列表
        /// </summary>
        /// <param name="dataSourceService"></param>
        /// <param name="goldFuturesService"></param>
        public DataSourceViewModel(IDataSourceService dataSourceService, IGoldFuturesService goldFuturesService)
        {
            _dataSourceService = dataSourceService;
            _goldFuturesService = goldFuturesService;
            LoadDataSourcesCommand.Execute(null);
        }

        [RelayCommand]
        private async Task LoadDataSourcesAsync()
        {
            IsLoading = true;
            var list = await _dataSourceService.GetAllDataSourcesAsync();
            DataSources = new ObservableCollection<DataSource>(list);
            IsLoading = false;
        }

        [RelayCommand]
        private void NewDataSource()
        {
            SelectedDataSource = null;
            Name = string.Empty;
            Type = DataSourceType.Website;
            Url = string.Empty;
            ApiKey = string.Empty;
            IsEnabled = true;
            UpdateInterval = 1440;
            Description = string.Empty;
            IsEditing = true;
        }

        [RelayCommand]
        private void EditDataSource(DataSource dataSource)
        {
            if (dataSource == null) return;
            SelectedDataSource = dataSource;
            Name = dataSource.Name;
            Type = dataSource.Type;
            Url = dataSource.Url;
            ApiKey = dataSource.ApiKey;
            IsEnabled = dataSource.IsEnabled;
            UpdateInterval = dataSource.UpdateInterval;
            Description = dataSource.Description;
            IsEditing = true;
        }

        [RelayCommand]
        private async Task SaveDataSourceAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("请输入数据源名称", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            IsLoading = true;
            if (SelectedDataSource == null) // 新增
            {
                var newDs = new DataSource
                {
                    Name = Name,
                    Type = Type,
                    Url = Url,
                    ApiKey = ApiKey,
                    IsEnabled = IsEnabled,
                    UpdateInterval = UpdateInterval,
                    Description = Description
                };
                await _dataSourceService.AddDataSourceAsync(newDs);
            }
            else // 编辑
            {
                SelectedDataSource.Name = Name;
                SelectedDataSource.Type = Type;
                SelectedDataSource.Url = Url;
                SelectedDataSource.ApiKey = ApiKey;
                SelectedDataSource.IsEnabled = IsEnabled;
                SelectedDataSource.UpdateInterval = UpdateInterval;
                SelectedDataSource.Description = Description;
                await _dataSourceService.UpdateDataSourceAsync(SelectedDataSource);
            }

            await LoadDataSourcesAsync();
            IsEditing = false;
            IsLoading = false;
        }

        [RelayCommand]
        private void CancelEdit()
        {
            IsEditing = false;
            SelectedDataSource = null;
        }

        [RelayCommand]
        private async Task DeleteDataSourceAsync(DataSource dataSource)
        {
            if (dataSource == null) return;
            var result = MessageBox.Show($"确定要删除数据源“{dataSource.Name}”吗？", "确认删除", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                IsLoading = true;
                await _dataSourceService.DeleteDataSourceAsync(dataSource.Id);
                await LoadDataSourcesAsync();
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task TestConnectionAsync(DataSource dataSource)
        {
            if (dataSource == null) return;
            IsLoading = true;
            try
            {
                await _dataSourceService.TestConnectionAsync(dataSource);
                MessageBox.Show("连接测试成功！", "测试结果", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"连接失败：{ex.Message}", "测试结果", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task FetchNowAsync(DataSource dataSource)
        {
            if (dataSource == null) return;
            IsLoading = true;
            try
            {
                await _dataSourceService.FetchDataAsync(dataSource);
                MessageBox.Show("采集任务已启动，请稍后查看结果。", "采集", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadDataSourcesAsync(); // 刷新状态
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"采集失败：{ex.Message}", "采集", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        // 添加导入命令
        [RelayCommand]
        private async Task ImportGoldDataAsync()
        {
            var dialog = new OpenFileDialog
            {
                Title = "选择黄金期货数据 CSV 文件",
                Filter = "CSV 文件 (*.csv)|*.csv|所有文件 (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                IsLoading = true;
                try
                {
                    // 执行导入（可能耗时，但已在服务内部异步）
                    await _goldFuturesService.ImportFromCsvAsync(dialog.FileName);
                    MessageBox.Show($"黄金期货数据导入成功！\n文件：{Path.GetFileName(dialog.FileName)}",
                                    "导入完成", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"导入失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }


    }
}
