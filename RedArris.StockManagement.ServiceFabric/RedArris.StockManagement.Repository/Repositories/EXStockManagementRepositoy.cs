using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RedArris.StockManagement.Entity;
using RedArris.StockManagement.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedArris.StockManagement.Repository.Repositories
{
    public class EXStockManagementRepositoy : IStockManagerRepository
    {
        #region Private Variables
        private readonly IOptions<ConfigModel> _appSettings;
        #endregion

        #region Constructor

        public EXStockManagementRepositoy(IOptions<ConfigModel> config)
        {
            _appSettings = config;
        }
        #endregion

        #region public Methods
        /// <summary>
        /// Get History Data from IEX Repo
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<List<DailyReturn>> GetHistoricData(string symbol, DateTime stDate, DateTime endDate)
        {
            List<HistoricReturn> returnLst = new List<HistoricReturn>();
            List<DailyReturn> collectionReturn = new List<DailyReturn>();
            string baseURL = _appSettings.Value.IEXBaseURL + symbol + "/chart/date/";
            string token = _appSettings.Value.IEXToken;


            string dayStr = string.Empty;
            
            try
            {

                for (var day = stDate; day <= endDate; day = day.AddDays(1))
                {
                    string dayMonth = day.Month.ToString().Length == 1 ? "0" + day.Month.ToString() : day.Month.ToString();
                    string dayDay = day.Day.ToString().Length == 1 ? "0" + day.Day.ToString() : day.Day.ToString();

                    dayStr = day.Year.ToString() + dayMonth + dayDay;

                    using (var httpClient = new System.Net.Http.HttpClient())
                    {
                        using (var response = await httpClient.GetAsync(baseURL + dayStr + "?token=" + token))
                        {
                            string resnseStr = await response.Content.ReadAsStringAsync();
                            returnLst = JsonConvert.DeserializeObject<List<HistoricReturn>>(resnseStr);
                            HistoricReturn highestReturn = returnLst.OrderByDescending(i => i.marketClose).FirstOrDefault();

                            if (highestReturn != null)
                                collectionReturn.Add(new DailyReturn() { ReturnDate = day, ReturnValue = highestReturn.marketClose });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return collectionReturn;
        }

        /// <summary>
        /// Calculate the Alpha for given period 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="stDate"></param>
        /// <param name="endDate"></param>
        /// <param name="bmSymbol"></param>
        /// <returns></returns>
        public async Task<List<Alpha>> GetAlpha(string symbol, DateTime stDate, DateTime endDate, string bmSymbol)
        {
             List<DailyReturn> returnSymbol =  await GetHistoricData(symbol, stDate, endDate);
             List<DailyReturn> returnBenchMark= await GetHistoricData(bmSymbol, stDate, endDate);
            List<Alpha> alpha = new List<Alpha>();

            returnSymbol.ForEach(x =>
            {
                DailyReturn bmDaily = returnBenchMark.FirstOrDefault(y => y.ReturnDate == x.ReturnDate);
                double alphaValue = bmDaily != null ? x.ReturnValue - bmDaily.ReturnValue : x.ReturnValue;

                alpha.Add(new Alpha() { CalculateDate = x.ReturnDate, AlphaValue = alphaValue });

            });
            return alpha;
        }
        #endregion
    }
}
