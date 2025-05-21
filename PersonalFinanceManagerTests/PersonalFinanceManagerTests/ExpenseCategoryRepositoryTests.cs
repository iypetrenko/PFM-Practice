using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Consts;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class ExpenseCategoryRepositoryTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private Mock<DbSet<ExpenseCategory>> _mockDbSet;
        private List<ExpenseCategory> _data;
        private ExpenseCategoryRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize test data
            _data = new List<ExpenseCategory>
            {
                new ExpenseCategory { Id = 1, Name = "Groceries", MonthlyBudget = 300, UserId = 1 },
                new ExpenseCategory { Id = 2, Name = "Transportation", MonthlyBudget = 150, UserId = 1 },
                new ExpenseCategory { Id = 3, Name = "Entertainment", MonthlyBudget = 100, UserId = 2 }
            };

            // Set up the mock DbSet
            _mockDbSet = MockDbSet(_data);

            // Set up the mock context
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _mockContext.Setup(c => c.ExpenseCategories).Returns(_mockDbSet.Object);

            // Create repository with mock context
            _repository = new TestExpenseCategoryRepository(_mockContext.Object);

            // Set user ID for tests
            SessionInfo.UserId = 1;
        }

        private class TestExpenseCategoryRepository : ExpenseCategoryRepository
        {
            private readonly PersonalFinanceManagerContext _context;

            public TestExpenseCategoryRepository(PersonalFinanceManagerContext context)
            {
                _context = context;
            }

            protected override PersonalFinanceManagerContext GetContext()
            {
                return _context;
            }
        }

        private static Mock<DbSet<T>> MockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();

            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());

            return mockSet;
        }

        [TestMethod]
        public void AddExpenseCategory_ShouldAddNewCategory()
        {
            // Arrange
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _repository.AddExpenseCategory("Test Category", 200);

            // Assert
            Assert.IsTrue(result);
            _mockDbSet.Verify(m => m.Add(It.Is<ExpenseCategory>(c =>
                c.Name == "Test Category" &&
                c.MonthlyBudget == 200 &&
                c.UserId == SessionInfo.UserId)), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }


        [TestMethod]
        public void RemoveExpenseCategory_ShouldRemoveCategory()
        {
            // Arrange
            int categoryId = 1;
            ExpenseCategory capturedCategory = null;

            _mockDbSet.Setup(m => m.Attach(It.IsAny<ExpenseCategory>()))
                .Callback<ExpenseCategory>(c => capturedCategory = c);

            _mockDbSet.Setup(m => m.Remove(It.IsAny<ExpenseCategory>()))
                .Returns<ExpenseCategory>(c => c);

            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _repository.RemoveExpenseCategory(categoryId);

            // Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(capturedCategory);
            Assert.AreEqual(categoryId, capturedCategory.Id);
            _mockDbSet.Verify(m => m.Attach(It.IsAny<ExpenseCategory>()), Times.Once);
            _mockDbSet.Verify(m => m.Remove(It.IsAny<ExpenseCategory>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }
}