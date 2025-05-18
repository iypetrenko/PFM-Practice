using NUnit.Framework;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using Moq; // Ensure this is included at the top of the file
using PersonalFinanceManager.Data;

namespace PersonalFinanceManager.Tests.Services
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private TransactionService _service = null!;
        private Mock<ApplicationDbContext> _mockContext = null!;

        [SetUp]
        public void Setup()
        {
            _mockContext = new Mock<ApplicationDbContext>();
            _service = new TransactionService(_mockContext.Object);
        }

        [Test]
        public void AddTransaction_IncreasesCount()
        {
            int initialCount = _service.GetTransactions().Count;

            // Use a concrete implementation of Transaction
            var transaction = new ExpenseTransaction
            {
                Amount = 100
            };

            _service.AddTransaction(transaction);
            Assert.That(_service.GetTransactions().Count, Is.EqualTo(initialCount + 1));
        }

    }
}
