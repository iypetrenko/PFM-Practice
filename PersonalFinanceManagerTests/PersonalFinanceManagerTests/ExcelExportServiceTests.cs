using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OfficeOpenXml;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Services;
namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class ExcelExportServiceTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private ExcelExportService _service;
        private List<ExpenseCategory> _testCategories;
        private List<Item> _testItems;

        [TestInitialize]
        public void Setup()
        {
            ExcelPackage.License.SetNonCommercialOrganization("My Noncommercial organization");
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _service = new ExcelExportService(_mockContext.Object);

            _testCategories = new List<ExpenseCategory>
        {
            new ExpenseCategory { Id = 1, Name = "Food", MonthlyBudget = 5000, UserId = 1 }
        };

            _testItems = new List<Item>
        {
            new Item { Id = 1, Name = "Milk", Price = 50, BuyDate = DateTime.Now, ToDoListId = 1 }
        };

            var mockCategories = CreateMockDbSet(_testCategories);
            var mockItems = CreateMockDbSet(_testItems);

            _mockContext.Setup(c => c.ExpenseCategories).Returns(mockCategories.Object);
            _mockContext.Setup(c => c.Items).Returns(mockItems.Object);
        }

        private Mock<DbSet<T>> CreateMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockSet = new Mock<DbSet<T>>();
            mockSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockSet;
        }

        [TestMethod]
        public void ExportUserData_CreatesValidExcelFile()
        {
            // Arrange
            var user = new User { Id = 1, UserName = "testuser", Role = UserRole.User };
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var testFilePath = Path.Combine(desktopPath, $"{user.UserName}_Data.xlsx");

            // Cleanup previous test files
            if (File.Exists(testFilePath))
            {
                File.Delete(testFilePath);
            }

            // Act
            _service.ExportUserData(user);

            // Assert
            Assert.IsTrue(File.Exists(testFilePath));

            using (var package = new ExcelPackage(new FileInfo(testFilePath)))
            {
                var userSheet = package.Workbook.Worksheets["Информация о пользователе"];
                Assert.AreEqual("testuser", userSheet.Cells[2, 2].Value);
            }

            // Cleanup
            File.Delete(testFilePath);
        }
    }
}
