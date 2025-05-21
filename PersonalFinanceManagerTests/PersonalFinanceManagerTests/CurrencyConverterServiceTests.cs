using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using PersonalFinanceManager.Services;
namespace PersonalFinanceManagerTests
{
    [TestClass]
public class CurrencyConverterServiceTests
{
    private Mock<CurrencyConverterService> _mockConverter;
    private CurrencyConverterService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockConverter = new Mock<CurrencyConverterService> { CallBase = true };
        _service = _mockConverter.Object;
    }

    [TestMethod]
    public void GetCurrencyExchange_ValidCurrencies_ReturnsCorrectRate()
    {
        // Arrange
        var expectedRate = 27.5m;
        var testData = new Dictionary<string, decimal> { { "USD_UAH", expectedRate } };

        _mockConverter.Protected()
            .Setup<string>("DownloadString", ItExpr.IsAny<string>())
            .Returns(JsonConvert.SerializeObject(testData));

        // Act
        var result = _service.GetCurrencyExchange("USD", "UAH");

        // Assert
        Assert.AreEqual(expectedRate, result);
    }

    [TestMethod]
    public void GetCurrencyExchange_ApiFailure_ReturnsDefaultRate()
    {
        // Arrange
        _mockConverter.Protected()
            .Setup<string>("DownloadString", ItExpr.IsAny<string>())
            .Throws(new WebException());

        // Act
        var result = _service.GetCurrencyExchange("USD", "EUR");

        // Assert
        Assert.AreEqual(1.0m, result);
    }
}
}
