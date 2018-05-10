using Syntell.DAO.Interfaces;
using Syntell.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Syntell.OpenExchangeRatesDAO
{
    public class ServiceManager : StockDao
    {
        private StockData requestData(string urlData)
        {
            var baseUrl = string.Format("http://openexchangerates.org/api/{0}?app_id=6de6a5cac9c84e1d9cb6b1135267cd46",urlData);

            try
            {
                HttpWebRequest request = WebRequest.Create(baseUrl) as HttpWebRequest;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format(
                    "Server error (HTTP {0}: {1}).",
                    response.StatusCode,
                    response.StatusDescription));

                DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(serviceResponse));
                object objResponse = jsonSerializer.ReadObject(response.GetResponseStream());
                   
                return convertResponseToDto(objResponse);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }

        public StockData convertResponseToDto(object input)
        {
            var responseData = input as serviceResponse;

            return new StockData
            {
                BaseCountry = responseData.baseCountry,
                Disclaimer = responseData.disclaimer,
                License = responseData.license,
                Timestamp = Syntell.Helpers.DateTimeHelpers.UnixTimeStampToDateTime(responseData.timestamp),
                Rates =  new CountryStockData {
                    AED = responseData.rates.AED,
                    AFN = responseData.rates.AFN,
                    ALL = responseData.rates.ALL,
                    AMD = responseData.rates.AMD,
                    GBP = 2,
                    YER = responseData.rates.YER,
                    ZAR = responseData.rates.ZAR,
                    ZMK = responseData.rates.ZMK,
                    ZWL = responseData.rates.ZWL
                }
            };

        }



        public StockData GetCurrentStockValues()
        {
            return requestData("latest.json");
        }

        public StockData GetStockValues(DateTime date)
        {
            return requestData(
                String.Format("/historical/{0:yyyy-MM-dd}.json",date)
            );
        }
    }

    [DataContract]
    public class serviceResponse
    {
        [DataMember(Name = "disclaimer")]
        public string disclaimer    { get; set; }

        [DataMember(Name = "license")]
        public string license       { get; set; }

        [DataMember(Name = "timestamp")]
        public double timestamp     { get; set; }

        [DataMember(Name = "base")]
        public string baseCountry   { get; set; }

        [DataMember(Name = "rates")]
        public ratesData rates     { get; set; }

        [DataContract]
        public class ratesData
        {
            [DataMember(Name = "AED")]
            public decimal AED {get; set;}

            [DataMember(Name = "AFN")]
            public decimal AFN {get; set;}

            [DataMember(Name = "ALL")]
            public decimal ALL {get; set;}

            [DataMember(Name = "AMD")]
            public decimal AMD {get; set;}

            [DataMember(Name = "YER")]
            public decimal YER {get; set;}

            [DataMember(Name = "ZAR")]
            public decimal ZAR {get; set;}

            [DataMember(Name = "ZMK")]
            public decimal ZMK {get; set;}

            [DataMember(Name = "ZWL")]
            public decimal ZWL {get; set;}            
        }

}
}
