using CsvHelper;
using CsvHelper.Configuration;
using PCBPriceAnalyzer.Data.Repositories;
using PCBPriceAnalyzer.Data.UnitOfWork;
using PCBPriceAnalyzer.Models.Entities;
using PCBPriceAnalyzer.Services.Interfaces;
using System.Globalization;

namespace PCBPriceAnalyzer.Services.Implementations
{
    public class GoldFuturesService : IGoldFuturesService
    {
        private readonly IRepository<GoldFuturesRecord> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public GoldFuturesService(IRepository<GoldFuturesRecord> repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取所有黄金期货记录
        /// </summary>
        public async Task<IEnumerable<GoldFuturesRecord>> GetAllRecordsAsync()
        {
            return await _repository.GetAllAsync();
        }

        /// <summary>
        /// 从 CSV 文件导入数据
        /// </summary>
        /// <param name="filePath">CSV 文件路径</param>
        public async Task ImportFromCsvAsync(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("CSV 文件未找到", filePath);

            var records = new List<GoldFuturesRecord>();

            
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                // 在这里配置 NumberStyles 以支持科学计数法
                // 获取 decimal 类型的转换器选项，如果不存在则创建默认选项
                var typeConverterOptions = csv.Context.TypeConverterOptionsCache.GetOptions<decimal>();
                // 设置 NumberStyles 为 Any，这样就能解析 "1.23e-05" 这样的格式了
                typeConverterOptions.NumberStyles = NumberStyles.Any;
                // 读取标题行
                csv.Read();
                csv.ReadHeader();

                while (csv.Read())
                {
                    try
                    {
                        var record = new GoldFuturesRecord
                        {
                            Date = csv.GetField<DateTime>("date"),
                            Open = csv.GetField<decimal>("open"),
                            High = csv.GetField<decimal>("high"),
                            Low = csv.GetField<decimal>("low"),
                            Close = csv.GetField<decimal>("close"),
                            AdjClose = csv.GetField<decimal>("adj close"),
                            Volume = csv.GetField<long>("volume"),
                            Ma7 = csv.GetField<decimal>("ma_7"),
                            Ma30 = csv.GetField<decimal>("ma_30"),
                            Ma90 = csv.GetField<decimal>("ma_90"),
                            DailyReturn = csv.GetField<decimal>("daily_return"),
                            Volatility7 = csv.GetField<decimal>("volatility_7"),
                            Volatility30 = csv.GetField<decimal>("volatility_30"),
                            Rsi = csv.GetField<decimal>("rsi"),
                            Macd = csv.GetField<decimal>("macd"),
                            MacdSignal = csv.GetField<decimal>("macd_signal"),
                            BbUpper = csv.GetField<decimal>("bb_upper"),
                            BbLower = csv.GetField<decimal>("bb_lower")
                        };
                        records.Add(record);
                    }
                    catch (Exception ex)
                    {
                        // 记录解析失败的行（可扩展日志）
                        throw new Exception($"解析 CSV 行失败：行号 {csv.Parser.Row}，错误：{ex.Message}");
                    }
                }
            }

            // 批量插入（为避免逐条插入的性能问题，使用 AddRange 方式）
            // 注意：如果 IRepository 没有 AddRange 方法，可以改用 DbContext 直接操作
            // 这里假设 IRepository 没有 AddRange，我们使用循环 + 批量保存
            // 但为了性能，建议修改仓储或直接使用 _unitOfWork 中的 DbContext
            // 此处使用循环逐条添加，但数据量大时可能较慢，可自行优化
            foreach (var record in records)
            {
                await _repository.AddAsync(record);
            }
            await _unitOfWork.CompleteAsync();
        }
    }
}
