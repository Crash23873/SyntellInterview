using Syntell.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Syntell.InterviewProject
{
    class Program
    {
        static void Main(string[] args)
        {
            var DAO = new Syntell.DAO.DAO(new Syntell.OpenExchangeRatesDAO.ServiceManager());

            var output = DAO.Stock.GetCurrentStockValues();

            Console.WriteLine("On " + DateTime.Now.ToString("dd 'of' MMMM yyyy") + " $" + 1 + " will buy you R " + output.Rates.ZAR);
            Console.WriteLine("On " + DateTime.Now.ToString("dd 'of' MMMM yyyy") + " $" + 1 + " will buy you £ " + output.Rates.GBP);
            
            Console.ReadLine();
        }
    }
}
