using Syntell.DAO.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntell.DAO
{
    public class DAO 
    {
        public StockDao Stock { get; set; }

        public DAO(StockDao stockDao)
        {
            this.Stock = stockDao;
        }

    }
}
