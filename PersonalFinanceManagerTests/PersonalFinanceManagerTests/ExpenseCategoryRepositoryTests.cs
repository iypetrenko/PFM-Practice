using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using PersonalFinanceManager.Consts;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class ExpenseCategoryRepositoryTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private Mock<DbSet<ExpenseCategory>> _mockSet;
        private List<ExpenseCategory> _categoryData;
        private ExpenseCategoryRepository _categoryRepository;

        [TestInitialize]
        public void Initialize()
        {
            // Setup test data
            _categoryData = new List<ExpenseCategory>
            {
                new ExpenseCategory { Id = 1, Name = "Groceries", UserId = 1, MonthlyBudget = 500 },
                new ExpenseCategory { Id = 2, Name = "Transportation", UserId = 1, MonthlyBudget = 200 },
                new ExpenseCategory { Id = 3, Name = "Entertainment", UserId = 2, MonthlyBudget = 150 }
            };

            // Setup mock DbSet
            _mockSet = new Mock<DbSet<ExpenseCategory>>();
            _mockSet.As<IQueryable<ExpenseCategory>>().Setup(m => m.Provider).Returns(_categoryData.AsQueryable().Provider);
            _mockSet.As<IQueryable<ExpenseCategory>>().Setup(m => m.Expression).Returns(_categoryData.AsQueryable().Expression);
            _mockSet.As<IQueryable<ExpenseCategory>>().Setup(m => m.ElementType).Returns(_categoryData.AsQueryable().ElementType);
            _mockSet.As<IQueryable<ExpenseCategory>>().Setup(m => m.GetEnumerator()).Returns(_categoryData.AsQueryable().GetEnumerator());

            // Setup mock context
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _mockContext.Setup(c => c.ExpenseCategories).Returns(_mockSet.Object);

            // Create repository with mock context
            _categoryRepository = new TestableExpenseCategoryRepository(_mockContext.Object);

            // Set the user ID for testing
            SessionInfo.UserId = 1;
        }

        [TestMethod]
        public void AddExpenseCategory_WithValidData_ReturnsTrue()
        {
            // Arrange
            var categoryName = "New Category";
            var budget = 300m;

            // Change expected behavior to make the test fail
            _mockContext.Setup(c => c.SaveChanges()).Returns(0); // Simulate failed save instead of successful

            // Act
            var result = _categoryRepository.AddExpenseCategory(categoryName, budget);

            // Assert
            Assert.IsTrue(result); // This will fail because SaveChanges returns 0
            _mockSet.Verify(m => m.Add(It.Is<ExpenseCategory>(c =>
                c.Name == categoryName &&
                c.MonthlyBudget == budget &&
                c.UserId == SessionInfo.UserId)), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void AddExpenseCategory_WhenSaveChangesFails_ReturnsFalse()
        {
            // Arrange
            var categoryName = "New Category";
            var budget = 300m;

            // Change expected behavior to make the test fail
            _mockContext.Setup(c => c.SaveChanges()).Returns(1); // Simulate successful save instead of failed

            // Act
            var result = _categoryRepository.AddExpenseCategory(categoryName, budget);

            // Assert
            Assert.IsFalse(result); // This will fail because SaveChanges returns 1
            _mockSet.Verify(m => m.Add(It.IsAny<ExpenseCategory>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetExpenseCategoriesList_ReturnsOnlyCurrentUserCategories()
        {
            // Arrange - already done in Initialize()

            // Act
            var result = _categoryRepository.GetExpenseCategoriesList();

            // Assert
            Assert.AreEqual(3, result.Count); // Should return 2 but assert 3 to fail
            Assert.IsTrue(result.All(c => c.UserId == SessionInfo.UserId));
            Assert.AreEqual("Entertainment", result[0].Name); // Incorrect name to make test fail
            Assert.AreEqual("Transportation", result[1].Name);
        }

        [TestMethod]
        public void RemoveExpenseCategory_WithValidId_ReturnsTrue()
        {
            // Arrange
            var categoryId = 1;
            _mockContext.Setup(c => c.SaveChanges()).Returns(0); // Change to 0 instead of 1 to make test fail

            // Act
            var result = _categoryRepository.RemoveExpenseCategory(categoryId);

            // Assert
            Assert.IsTrue(result); // This will fail
            _mockSet.Verify(m => m.Attach(It.Is<ExpenseCategory>(c => c.Id == categoryId)), Times.Once);
            _mockSet.Verify(m => m.Remove(It.Is<ExpenseCategory>(c => c.Id == categoryId)), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void RemoveExpenseCategory_WhenSaveChangesFails_ReturnsFalse()
        {
            // Arrange
            var categoryId = 1;
            _mockContext.Setup(c => c.SaveChanges()).Returns(1); // Change to 1 instead of 0 to make test fail

            // Act
            var result = _categoryRepository.RemoveExpenseCategory(categoryId);

            // Assert
            Assert.IsFalse(result); // This will fail
            _mockSet.Verify(m => m.Attach(It.Is<ExpenseCategory>(c => c.Id == categoryId)), Times.Once);
            _mockSet.Verify(m => m.Remove(It.Is<ExpenseCategory>(c => c.Id == categoryId)), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }

    // Helper class to allow dependency injection of context for testing
    public class TestableExpenseCategoryRepository : ExpenseCategoryRepository
    {
        private readonly PersonalFinanceManagerContext _context;

        public TestableExpenseCategoryRepository(PersonalFinanceManagerContext context)
        {
            _context = context;
        }

        protected override PersonalFinanceManagerContext GetContext()
        {
            return _context;
        }
    }
}