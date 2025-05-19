using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace PersonalFinanceManager.Services
{
    class CurrencyConverterService
    {
        private readonly String BASE_URI = "https://openexchangerates.org/";
        private readonly String API_VERSION = "v7";

        private readonly String API_KEY = "41218199bbc44e1b8f28b29d3d9e4258";


        public CurrencyConverterService() { }

        public Decimal GetCurrencyExchange(String localCurrency, String foreignCurrency)
        {
            var code = $"{localCurrency}_{foreignCurrency}";
            var newRate = FetchSerializedData(code);
            return newRate;
        }

        private Decimal FetchSerializedData(String code)
        {
            var url = $"{BASE_URI}/api/{API_VERSION}/convert?q={code}&compact=ultra";
            var webClient = new WebClient();
            var jsonData = String.Empty;

            var conversionRate = 1.0m;
            try
            {
                jsonData = webClient.DownloadString(url);

                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(jsonData);
                conversionRate = jsonObject[code];


            }
            catch (Exception) { }

            return conversionRate;
        }
    }
}
