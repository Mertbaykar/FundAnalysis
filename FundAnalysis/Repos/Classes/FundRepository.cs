using FundAnalysis.API.DbContexts;
using FundAnalysis.API.Repos.Bases;
using FundAnalysis.API.Repos.Interfaces;
using FundAnalysis.Models.Entities;
using Microsoft.EntityFrameworkCore;
using static FundAnalysis.API.Repos.Interfaces.IFundRepository;

namespace FundAnalysis.API.Repos.Classes
{
    public class FundRepository : RepositoryBase<Fund>, IFundRepository
    {
        public FundRepository(FundContext fundContext) : base(fundContext)
        {

        }

        #region Base Methods
        public async Task<FundPrice> GetCurrentFundPriceAsync(string fundCode)
        {
            if (await IsFundExists(fundCode))
            {
                var currentFundprice = await _fundContext.FundPrices.OrderByDescending(x => x.Date).FirstOrDefaultAsync(x=> x.Fund.Code == fundCode);
                if (currentFundprice == null)
                    throw new Exception("There is no any price info for " + fundCode);
                return currentFundprice;
            }
            throw new Exception("Fund is not found for " + fundCode);
        }

        public async Task<FundPrice> GetFundPriceByCodeAndDate(string fundCode, DateTime date)
        {
            return await _fundContext.FundPrices.FirstOrDefaultAsync(x => x.Fund.Code == fundCode && x.Date == date);
        }
        public decimal FindProfitPercent(decimal currentPrice, decimal previousPrice)
        {
            return ((currentPrice - previousPrice) / previousPrice) * 100;
        }
        public async Task<bool> IsFundExists(string fundCode)
        {
            return await _fundContext.Funds.FirstOrDefaultAsync(x => x.Code == fundCode) != null;
        }
        #endregion

        #region Profit Related Methods

        public async Task<List<string>> GetProfitsWithMessagesWithKnownPeriods(string fundCode)
        {
            try
            {
                List<string> messages = new List<string>();

                var dailyProfitMessage = await GetDailyProfitWithMessage(fundCode);
                var weeklyProfitMessage = await GetWeeklyProfitWithMessage(fundCode);
                var monthlyProfitMessage = await GetOneMonthlyProfitWithMessage(fundCode);
                var threeMonthlyProfitMessage = await GetThreeMonthlyProfitWithMessage(fundCode);

                messages.Add(dailyProfitMessage);
                messages.Add(weeklyProfitMessage);
                messages.Add(monthlyProfitMessage);
                messages.Add(threeMonthlyProfitMessage);

                return messages;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public async Task<string> GetProfitPercentByCodeAndPreviousDateWithMessage(string fundCode, PeriodType periodType, int amount)
        {
            string message = string.Empty;

            try
            {
                var profitPercent = await GetProfitPercentByCodeAndPreviousDate(fundCode, periodType, amount);

                if (periodType == PeriodType.Daily)
                {
                    message = "Profit for " + amount + " day(s) is ";

                }
                else if (periodType == PeriodType.Weekly)
                {
                    message = "Profit for " + amount + " week(s) is ";

                }
                else if (periodType == PeriodType.Monthly)
                {
                    message = "Profit for " + amount + " month(s) is ";

                }
                else if (periodType == PeriodType.Annually)
                {
                    message = "Profit for " + amount + " year(s) is ";

                }
                message += "%" + profitPercent.ToString("N6");
                return message;
                //return new ProfitDTO(message,)
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return message;
            }
        }

        public async Task<decimal> GetProfitPercentByCodeAndPreviousDate(string fundCode, PeriodType periodType, int amount)
        {
            var currentFundprice = await GetCurrentFundPriceAsync(fundCode);
            DateTime currentDate = currentFundprice.Date;
            // today is just to fill pastDate temporarily
            DateTime pastDate = DateTime.Today;

            #region Determine pastDate
            if (periodType == PeriodType.Daily)
            {
                pastDate = currentDate.AddDays(-amount);
            }
            else if (periodType == PeriodType.Weekly)
            {
                pastDate = currentDate.AddDays(-(amount * 7));

            }
            else if (periodType == PeriodType.Monthly)
            {
                pastDate = currentDate.AddMonths(-amount);

            }
            else if (periodType == PeriodType.Annually)
            {
                pastDate = currentDate.AddYears(amount);

            }
            #endregion

            var fundPriceOfPreviousDate = await GetFundPriceByCodeAndDate(fundCode, pastDate);

            if (fundPriceOfPreviousDate == null)
                throw new Exception("Fund price at " + pastDate + " is not found for " + fundCode);

            return FindProfitPercent(currentFundprice.Close, fundPriceOfPreviousDate.Close);
        }

        public async Task<decimal> GetDailyProfit(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDate(fundCode, PeriodType.Daily, 1);
        }
        public async Task<string> GetDailyProfitWithMessage(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDateWithMessage(fundCode, PeriodType.Daily, 1);
        }

        public async Task<decimal> GetWeeklyProfit(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDate(fundCode, PeriodType.Weekly, 1);
        }

        public async Task<string> GetWeeklyProfitWithMessage(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDateWithMessage(fundCode, PeriodType.Weekly, 1);
        }

        public async Task<decimal> GetOneMonthlyProfit(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDate(fundCode, PeriodType.Monthly, 1);
        }

        public async Task<string> GetOneMonthlyProfitWithMessage(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDateWithMessage(fundCode, PeriodType.Monthly, 1);
        }

        public async Task<decimal> GetThreeMonthlyProfit(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDate(fundCode, PeriodType.Monthly, 3);
        }

        public async Task<string> GetThreeMonthlyProfitWithMessage(string fundCode)
        {
            return await GetProfitPercentByCodeAndPreviousDateWithMessage(fundCode, PeriodType.Monthly, 3);
        }
        #endregion
    }
}
