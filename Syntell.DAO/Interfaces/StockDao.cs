using Syntell.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntell.DAO.Interfaces
{
    public interface StockDao
    {
        StockData GetCurrentStockValues();
        StockData GetStockValues(DateTime date);
    }
}
