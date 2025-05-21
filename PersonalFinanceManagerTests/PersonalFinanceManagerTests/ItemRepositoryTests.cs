using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class ItemRepositoryTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private Mock<DbSet<Item>> _mockItemsDbSet;
        private List<Item> _itemsData;
        private List<ExpenseCategory> _categoriesData;
        private ItemRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize test data
            _categoriesData = new List<ExpenseCategory>
            {
                new ExpenseCategory { Id = 1, Name = "Groceries", MonthlyBudget = 300, UserId = 1 },
                new ExpenseCategory { Id = 2, Name = "Transportation", MonthlyBudget = 150, UserId = 1 },
                new ExpenseCategory { Id = 3, Name = "Entertainment", MonthlyBudget = 100, UserId = 2 }
            };

            _itemsData = new List<Item>
            {
                new Item { Id = 1, Name = "Bread", Price = 2.5m, ToDoListId = 1, BuyDate = DateTime.Today.AddDays(-1), ExpenseCategory = _categoriesData[0] },
                new Item { Id = 2, Name = "Milk", Price = 1.8m, ToDoListId = 1, BuyDate = DateTime.Today, ExpenseCategory = _categoriesData[0] },
                new Item { Id = 3, Name = "Bus Ticket", Price = 1.5m, ToDoListId = 2, BuyDate = DateTime.Today, ExpenseCategory = _categoriesData[1] },
                new Item { Id = 4, Name = "Movie Ticket", Price = 12m, ToDoListId = 3, BuyDate = DateTime.Today, ExpenseCategory = _categoriesData[2] }
            };

            // Set up the mock DbSet for Items
            _mockItemsDbSet = MockDbSet(_itemsData);

            // Set up the mock context
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _mockContext.Setup(c => c.Items).Returns(_mockItemsDbSet.Object);

            // Create repository with mock context
            _repository = new TestItemRepository(_mockContext.Object);
        }

        private class TestItemRepository : ItemRepository
        {
            private readonly PersonalFinanceManagerContext _context;

            public TestItemRepository(PersonalFinanceManagerContext context)
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

            mockSet.Setup(m => m.Include(It.IsAny<string>())).Returns(mockSet.Object);

            return mockSet;
        }


        [TestMethod]
        public void AddNewItem_ShouldAddItemAndReturnTrue()
        {
            // Arrange
            var newItem = new Item { Name = "Test Item", Price = 10m, ToDoListId = 1 };
            _mockContext.Setup(c => c.SaveChanges()).Returns(1);

            // Act
            bool result = _repository.AddNewItem(newItem);

            // Assert
            Assert.IsTrue(result);
            _mockItemsDbSet.Verify(m => m.Add(newItem), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetItems_ShouldReturnItemsForSpecificCategory()
        {
            // Arrange
            int categoryId = 1;

            // Act
            var result = _repository.GetItems(categoryId).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(i => i.ToDoListId == categoryId));
        }

        [TestMethod]
        public void GetAllItems_ShouldReturnAllItems()
        {
            // Act
            var result = _repository.GetAllItems();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }





        [TestMethod]
        public void FilterItems_WithAllOption_ShouldFilterByNameAndDate()
        {
            // Arrange
            string searchText = "Bread";
            string selectedItem = "All";
            DateTime date = DateTime.Today;

            // Act
            var result = _repository.FilterItems(searchText, selectedItem, date);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Bread", result[0].Name);
        }

        [TestMethod]
        public void FilterItems_WithOtherOption_ShouldReturnAllItems()
        {
            // Arrange
            string searchText = "anything";
            string selectedItem = "SomeOtherOption";
            DateTime date = DateTime.Today;

            // Act
            var result = _repository.FilterItems(searchText, selectedItem, date);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(4, result.Count);
        }
    }
}