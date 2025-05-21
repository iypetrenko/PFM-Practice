using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using System.Data.Entity;
using System.Collections.Generic;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class ItemRepositoryTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private Mock<DbSet<Item>> _mockSet;
        private List<Item> _itemData;
        private ItemRepository _itemRepository;

        [TestInitialize]
        public void Initialize()
        {
            // Setup test data
            _itemData = new List<Item>
            {
                new Item {
                    Id = 1,
                    Name = "Milk",
                    Description = "Organic milk",
                    BuyDate = new DateTime(2025, 5, 10),
                    Price = 3.99m,
                    ToDoListId = 1
                },
                new Item {
                    Id = 2,
                    Name = "Bread",
                    Description = "Whole wheat",
                    BuyDate = new DateTime(2025, 5, 11),
                    Price = 2.49m,
                    ToDoListId = 1
                },
                new Item {
                    Id = 3,
                    Name = "Movie Ticket",
                    Description = "Cinema visit",
                    BuyDate = new DateTime(2025, 5, 15),
                    Price = 12.00m,
                    ToDoListId = 2
                }
            };

            // Setup mock DbSet
            _mockSet = new Mock<DbSet<Item>>();
            _mockSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(_itemData.AsQueryable().Provider);
            _mockSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(_itemData.AsQueryable().Expression);
            _mockSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(_itemData.AsQueryable().ElementType);
            _mockSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(_itemData.AsQueryable().GetEnumerator());

            // Setup mock context
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _mockContext.Setup(c => c.Items).Returns(_mockSet.Object);

            // Create repository with mock context
            _itemRepository = new TestableItemRepository(_mockContext.Object);
        }

        [TestMethod]
        public void AddNewItem_WithValidData_ReturnsTrue()
        {
            // Arrange
            var newItem = new Item
            {
                Name = "Coffee",
                Description = "Morning coffee",
                BuyDate = DateTime.Now,
                Price = 4.50m,
                ToDoListId = 1
            };

            _mockContext.Setup(c => c.SaveChanges()).Returns(1); // Simulate successful save

            // Act
            var result = _itemRepository.AddNewItem(newItem);

            // Assert
            Assert.IsTrue(result);
            _mockSet.Verify(m => m.Add(It.Is<Item>(i => i.Name == "Coffee")), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetItems_WithValidToDoListId_ReturnsMatchingItems()
        {
            // Arrange
            var todoId = 1;

            // Act
            var result = _itemRepository.GetItems(todoId);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(i => i.ToDoListId == todoId));
            Assert.AreEqual("Milk", result[0].Name);
            Assert.AreEqual("Bread", result[1].Name);
        }

        [TestMethod]
        public void GetItems_WithNonExistentToDoListId_ReturnsEmptyList()
        {
            // Arrange
            var todoId = 99;

            // Act
            var result = _itemRepository.GetItems(todoId);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetAllItems_ReturnsAllItems()
        {
            // Act
            var result = _itemRepository.GetAllItems();

            // Assert
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void DeleteItem_WithValidItem_ReturnsTrue()
        {
            // Arrange
            var itemToDelete = _itemData[0];
            _mockContext.Setup(c => c.SaveChanges()).Returns(1); // Simulate successful deletion

            // Act
            var result = _itemRepository.DeleteItem(itemToDelete);

            // Assert
            Assert.IsTrue(result);
            _mockSet.Verify(m => m.Attach(itemToDelete), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void UpdateItemPrice_WithValidData_ReturnsTrue()
        {
            // Arrange
            var itemId = 1;
            var newPrice = 4.99m;

            var item = _itemData.FirstOrDefault(i => i.Id == itemId);

            _mockContext.Setup(m => m.SaveChanges()).Returns(1);
            _mockContext.Setup(m => m.Items.FirstOrDefault(It.IsAny<Func<Item, bool>>())).Returns(item);

            // Act
            var result = _itemRepository.UpdateItemPrice(itemId, newPrice);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(newPrice, item.Price);
            _mockSet.Verify(m => m.Attach(item), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void FilterItems_WithSearchTextAndAllCategory_ReturnsFilteredItems()
        {
            // Arrange
            var searchText = "Milk";
            var selectedItem = "All";
            var date = new DateTime(2025, 5, 20);

            // Act
            var result = _itemRepository.FilterItems(searchText, selectedItem, date);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("Milk", result[0].Name);
        }

        [TestMethod]
        public void FilterItems_WithNonAllCategory_ReturnsAllItems()
        {
            // Arrange
            var searchText = "";
            var selectedItem = "Groceries"; // Any value that's not "All"
            var date = new DateTime(2025, 5, 20);

            // Act
            var result = _itemRepository.FilterItems(searchText, selectedItem, date);

            // Assert
            Assert.AreEqual(3, result.Count); // Should return all items
        }
    }

    // Helper class to allow dependency injection of context for testing
    public class TestableItemRepository : ItemRepository
    {
        private readonly PersonalFinanceManagerContext _context;

        public TestableItemRepository(PersonalFinanceManagerContext context)
        {
            _context = context;
        }

        protected override PersonalFinanceManagerContext GetContext()
        {
            return _context;
        }
    }
}