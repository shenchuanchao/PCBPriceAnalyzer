using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBPriceAnalyzer.Models.Settings
{
    public class AppSettings
    {
        public string Theme { get; set; } = "Light";
        public int AutoRefreshInterval { get; set; } = 5;
        public int DefaultMaterialId { get; set; } = 1;
        public bool EnablePriceAlert { get; set; } = false;
        public decimal PriceAlertThreshold { get; set; } = 100000;
    }

}
