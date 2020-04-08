using RedArris.StockManagement.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedArris.StockManagement.Repository.Interfaces
{
    public interface IStockManagerRepository
    {
        public Task<List<DailyReturn>> GetHistoricData(string symbol, DateTime stDate, DateTime endDate);
        public Task<List<Alpha>> GetAlpha(string symbol, DateTime stDate, DateTime endDate, string bmSymbol);

    }
}
