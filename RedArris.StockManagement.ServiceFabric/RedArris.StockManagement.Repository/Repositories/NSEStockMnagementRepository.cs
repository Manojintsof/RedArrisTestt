using RedArris.StockManagement.Entity;
using RedArris.StockManagement.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RedArris.StockManagement.Repository.Repositories
{
   public class NSEStockMnagementRepository: IStockManagerRepository
    {
               

        public async Task<List<DailyReturn>> GetHistoricData(string symbol, DateTime stDate, DateTime endDate)
        {
          throw new NotImplementedException();

        }

        public Task<List<Alpha>> GetAlpha(string symbol, DateTime stDate, DateTime endDate, string bmSymbol)
        {
            throw new NotImplementedException();
        }

    }
}
