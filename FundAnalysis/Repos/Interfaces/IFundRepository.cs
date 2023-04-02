using FundAnalysis.API.Repos.Bases;
using FundAnalysis.Models.Entities;

namespace FundAnalysis.API.Repos.Interfaces
{
    public interface IFundRepository : IRepositoryBase<Fund>
    {
        public enum PeriodType
        {
            Daily,
            Weekly,
            Monthly,
            Annually
        }
        public decimal FindProfitPercent(decimal currentPrice, decimal previousPrice);
        public Task<FundPrice> GetFundPriceByCodeAndDate(string fundCode, DateTime date);
        public Task<FundPrice> GetCurrentFundPriceAsync(string fundCode);
        public Task<decimal> GetProfitPercentByCodeAndPreviousDate(string fundCode, PeriodType periodType, int amount);
        public Task<string> GetProfitPercentByCodeAndPreviousDateWithMessage(string fundCode, PeriodType periodType, int amount);
        public Task<List<string>> GetProfitsWithMessagesWithKnownPeriods(string fundCode);
        public Task<decimal> GetDailyProfit(string fundCode);
        public Task<string> GetDailyProfitWithMessage(string fundCode);
        public Task<decimal> GetWeeklyProfit(string fundCode);
        public Task<string> GetWeeklyProfitWithMessage(string fundCode);
        public Task<decimal> GetOneMonthlyProfit(string fundCode);
        public Task<string> GetOneMonthlyProfitWithMessage(string fundCode);
        public Task<decimal> GetThreeMonthlyProfit(string fundCode);
        public Task<string> GetThreeMonthlyProfitWithMessage(string fundCode);
        public Task<bool> IsFundExists(string fundCode);

    }
}
