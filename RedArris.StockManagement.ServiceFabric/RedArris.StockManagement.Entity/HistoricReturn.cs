using System;
using System.Collections.Generic;
using System.Text;

namespace RedArris.StockManagement.Entity
{
   public class HistoricReturn
    {

        public string date { get; set; }
        public double marketOpen { get; set; }
        public double marketClose{ get; set; }
        public double marketChangeOverTime { get; set; }
    }
}
