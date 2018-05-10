using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntell.Models.DTO
{
    public class StockData
    {
        public string Disclaimer    { get; set; }
        public string License       { get; set; }
        public DateTime Timestamp     { get; set; }
        public string BaseCountry   { get; set; }
        public CountryStockData Rates { get; set; }
    }
}
