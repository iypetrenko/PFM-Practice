using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using PersonalFinanceManager.Services;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class CurrencyConverterServiceTests
    {
        private Mock<IWebClientWrapper> _mockWebClient;
        private TestableCurrencyConverterService _currencyConverterService;

        [TestInitialize]
        public void Initialize()
        {
            _mockWebClient = new Mock<IWebClientWrapper>();
            _currencyConverterService = new TestableCurrencyConverterService(_mockWebClient.Object);
        }

        [TestMethod]
        public void GetCurrencyExchange_WithValidCurrencies_ReturnsConversionRate()
        {
            // Arrange
            var localCurrency = "USD";
            var foreignCurrency = "EUR";
            var code = $"{localCurrency}_{foreignCurrency}";
            var conversionRate = 0.92m;

            var jsonResponse = JsonConvert.SerializeObject(new Dictionary<string, decimal>
            {
                { code, conversionRate }
            });

            _mockWebClient.Setup(c => c.DownloadString(It.IsAny<string>())).Returns(jsonResponse);

            // Act
            var result = _currencyConverterService.GetCurrencyExchange(localCurrency, foreignCurrency);

            // Assert
            Assert.AreEqual(conversionRate, result);
            _mockWebClient.Verify(c => c.DownloadString(It.Is<string>(url =>
                url.Contains(code) && url.Contains("convert"))), Times.Once);
        }

        [TestMethod]
        public void GetCurrencyExchange_WhenApiCallFails_ReturnsDefaultRate()
        {
            // Arrange
            var localCurrency = "USD";
            var foreignCurrency = "GBP";

            _mockWebClient.Setup(c => c.DownloadString(It.IsAny<string>())).Throws<WebException>();

            // Act
            var result = _currencyConverterService.GetCurrencyExchange(localCurrency, foreignCurrency);

            // Assert
            Assert.AreEqual(1.0m, result); // Should return default rate of 1.0
        }
    }

    // Interface for WebClient to make it mockable
    public interface IWebClientWrapper
    {
        string DownloadString(string address);
    }

    // WebClient wrapper that implements the interface
    public class WebClientWrapper : IWebClientWrapper
    {
        private readonly WebClient _webClient = new WebClient();

        public string DownloadString(string address)
        {
            return _webClient.DownloadString(address);
        }
    }

    // Testable version of the CurrencyConverterService
    public class TestableCurrencyConverterService : CurrencyConverterService
    {
        private readonly IWebClientWrapper _webClient;

        public TestableCurrencyConverterService(IWebClientWrapper webClient)
        {
            _webClient = webClient;
        }

        protected override WebClient CreateWebClient()
        {
            return null; // Not used in tests
        }

        protected override string DownloadString(string url)
        {
            return _webClient.DownloadString(url);
        }
    }
}