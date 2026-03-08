using PCBPriceAnalyzer.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Services.Interfaces
{
    public interface ISettingsService
    {
        Task<AppSettings> LoadSettingsAsync();
        Task SaveSettingsAsync(AppSettings settings);
    }

}
