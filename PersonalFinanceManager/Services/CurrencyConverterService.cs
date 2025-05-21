using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace PersonalFinanceManager.Services
{
    public class CurrencyConverterService
    {
        private readonly String BASE_URI = "https://openexchangerates.org/";
        private readonly String API_VERSION = "v7";

        private readonly String API_KEY = "41218199bbc44e1b8f28b29d3d9e4258";


        public CurrencyConverterService() { }
        protected virtual WebClient CreateWebClient()
        {
            return new WebClient();
        }
        protected internal virtual string DownloadString(string url)
        {
            var webClient = CreateWebClient();
            return webClient.DownloadString(url);
        }
        public Decimal GetCurrencyExchange(String localCurrency, String foreignCurrency)
        {
            var code = $"{localCurrency}_{foreignCurrency}";
            var newRate = FetchSerializedData(code);
            return newRate;
        }

        private Decimal FetchSerializedData(String code)
        {
            var url = $"{BASE_URI}/api/{API_VERSION}/convert?q={code}&compact=ultra";
            var jsonData = String.Empty;
            var conversionRate = 1.0m;
            try
            {
                jsonData = DownloadString(url);
                var jsonObject = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(jsonData);
                conversionRate = jsonObject[code];
            }
            catch (Exception) { }
            return conversionRate;
        }
    }
}

