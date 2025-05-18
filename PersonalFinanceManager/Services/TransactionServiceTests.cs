using NUnit.Framework;
using PersonalFinanceManager.Models;
using PersonalFinanceManager.Services;
using Moq;
using PersonalFinanceManager.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using PersonalFinanceManager.Services.Interfaces;

namespace PersonalFinanceManager.Tests.Services
{
    [TestFixture]
    public class TransactionServiceTests
    {
        private TransactionService _service;
        private Mock<ApplicationDbContext> _mockContext;
        private Mock<DbSet<Transaction>> _mockTransactions;
        private List<Transaction> _transactions;

        [SetUp]
        public void Setup()
        {
            // Setup the test data
            _transactions = new List<Transaction>
            {
                new ExpenseTransaction { Id = 1, Amount = 50.0m },
                new ExpenseTransaction { Id = 2, Amount = 75.0m }
            };

            // Setup the mock DbSet
            _mockTransactions = new Mock<DbSet<Transaction>>();

            // Setup the DbSet to return our test data
            _mockTransactions.As<IQueryable<Transaction>>()
                .Setup(m => m.Provider)
                .Returns(_transactions.AsQueryable().Provider);
            _mockTransactions.As<IQueryable<Transaction>>()
                .Setup(m => m.Expression)
                .Returns(_transactions.AsQueryable().Expression);
            _mockTransactions.As<IQueryable<Transaction>>()
                .Setup(m => m.ElementType)
                .Returns(_transactions.AsQueryable().ElementType);
            _mockTransactions.As<IQueryable<Transaction>>()
                .Setup(m => m.GetEnumerator())
                .Returns(() => _transactions.GetEnumerator());

            // Setup the mock DbSet.Add method to add to our list
            _mockTransactions.Setup(m => m.Add(It.IsAny<Transaction>()))
                .Callback<Transaction>((transaction) => _transactions.Add(transaction));

            // Setup the mock context
            _mockContext = new Mock<ApplicationDbContext>();
            _mockContext.Setup(c => c.Transactions).Returns(_mockTransactions.Object);

            // Initialize the service with the mock context
            _service = new TransactionService(_mockContext.Object);
        }

        [Test]
        public void AddTransaction_IncreasesCount()
        {
            int initialCount = _transactions.Count;

            // Create a new transaction to add
            var transaction = new ExpenseTransaction
            {
                Amount = 100
            };

            // Add the transaction
            _service.AddTransaction(transaction);

            // Verify the count increased
            Assert.That(_transactions.Count, Is.EqualTo(initialCount + 1));
        }
    }
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;

        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaction>> GetTransactions()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task AddTransaction(Transaction transaction)
        {
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}