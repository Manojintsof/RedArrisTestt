using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RedArris.StockManagement.Entity;
using RedArris.StockManagement.Repository.Interfaces;
using System;
using System.Globalization;
using System.Net;
using System.Threading.Tasks;

namespace RedArris.StockManagement.API.Controllers
{
    [Route("api/StockManager")]
    [ApiController]
    public class StockManagerController : ControllerBase
    {
        #region Private variables
        private readonly IOptions<ConfigModel> _appSettings;
        private readonly ILogger<StockManagerController> _logger;
        private readonly IStockManagerRepository _repo;
        #endregion

        #region constructor
        public StockManagerController(ILogger<StockManagerController> logger, IStockManagerRepository repo, 
            IOptions<ConfigModel> config)
        {
            _appSettings = config;
            _logger = logger;
            _repo = repo;

        }

        #endregion

        #region Actions
        /// <summary>
        /// Get the historic price for a symbol for a period
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("historicData/{symbol}/{startDate?}/{endDate?}")]
        public async Task<IActionResult> HistoricData(string symbol, string startDate = null, string endDate = null)
        {
            var a = _appSettings.Value.MaxEndDate;

            DateTime stDate = new DateTime();
            DateTime thruDate = new DateTime();
            if (!ModelState.IsValid)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
            }


            DateTime dateToCompareFrom = DateTime.Parse(_appSettings.Value.MinStartDate.ToString() );
            DateTime dateToCompareTo = DateTime.Parse(_appSettings.Value.MaxEndDate);

            try
            {

                string startDateFormatted = string.Empty;
                string endDateFormatted = string.Empty;

                if (startDate != null && endDate != null)
                {
                    startDateFormatted = DateTime.ParseExact(startDate, "yyyyMMdd",
                    CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                    endDateFormatted = DateTime.ParseExact(endDate, "yyyyMMdd",
                    CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                    stDate = DateTime.Parse(startDateFormatted);
                    thruDate = DateTime.Parse(endDateFormatted);
                }
                else//startDate and/or endDate not provided
                {
                    stDate = new DateTime(DateTime.Now.Year, 1, 1);
                    thruDate = DateTime.Now;
                }

                if (stDate < dateToCompareFrom || thruDate > dateToCompareTo)
                {
                    throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
                }

                var result = await _repo.GetHistoricData(symbol, stDate, thruDate);
                return Ok(result);
            }
            catch (FormatException fe)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch
            {
                throw new Exception();
            }

        }


        /// <summary>
        /// Get the Alpha for symbol Vs Benchmark Symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="bmSymbol"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        [HttpGet("alpha/{symbol}/{bmSymbol}/{startDate?}/{endDate?}")]
        public async Task<IActionResult> Alpha(string symbol, string bmSymbol, string startDate = null, string endDate = null )
        {

            DateTime stDate = new DateTime();
            DateTime thruDate = new DateTime();
            if (!ModelState.IsValid)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
            }

            DateTime dateToCompareFrom = DateTime.Parse(_appSettings.Value.MinStartDate.ToString());
            DateTime dateToCompareTo = DateTime.Parse(_appSettings.Value.MaxEndDate);

            try
            {

                string startDateFormatted = string.Empty;
                string endDateFormatted = string.Empty;

                if (startDate != null && endDate != null)
                {
                    startDateFormatted = DateTime.ParseExact(startDate, "yyyyMMdd",
                    CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                    endDateFormatted = DateTime.ParseExact(endDate, "yyyyMMdd",
                    CultureInfo.InvariantCulture).ToString("yyyy/MM/dd");

                    stDate = DateTime.Parse(startDateFormatted);
                    thruDate = DateTime.Parse(endDateFormatted);
                }
                else//startDate and/or endDate not provided
                {
                    stDate = new DateTime(DateTime.Now.Year, 1, 1);
                    thruDate = DateTime.Now;
                }

                if (stDate < dateToCompareFrom || thruDate > dateToCompareTo)
                {
                    throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
                }

                var result = await _repo.GetAlpha(symbol, stDate, thruDate, bmSymbol);
                return Ok(result);
            }
            catch (FormatException fe)
            {
                throw new System.Web.Http.HttpResponseException(HttpStatusCode.BadRequest);
            }
            catch
            {
                throw new Exception();
            }
        }

        #endregion
    }
}