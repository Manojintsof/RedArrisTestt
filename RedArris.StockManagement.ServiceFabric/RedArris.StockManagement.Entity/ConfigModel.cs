using System;
using System.Collections.Generic;
using System.Text;

namespace RedArris.StockManagement.Entity
{
    public class ConfigModel
    {
        public string MinStartDate { get; set; }
        public string MaxEndDate { get; set; }

        public string IEXBaseURL { get; set; }
        public string IEXToken { get; set; }
    }
}
