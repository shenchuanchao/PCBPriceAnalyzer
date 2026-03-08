using PCBPriceAnalyzer.Models.Settings;
using PCBPriceAnalyzer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Implementations
{
    public class SettingsService : ISettingsService
    {
        private readonly string _settingsFilePath;

        public SettingsService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var appFolder = Path.Combine(appDataPath, "PCBPriceAnalyzer");
            Directory.CreateDirectory(appFolder);
            _settingsFilePath = Path.Combine(appFolder, "settings.json");
            //生成的json文件路径：%LOCALAPPDATA%\PCBPriceAnalyzer
        }

        public async Task<AppSettings> LoadSettingsAsync()
        {
            if (!File.Exists(_settingsFilePath)) return new AppSettings();
            try
            {
                var json = await File.ReadAllTextAsync(_settingsFilePath);
                return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
            }
            catch { return new AppSettings(); }
        }

        public async Task SaveSettingsAsync(AppSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_settingsFilePath, json);
        }
    }
}
