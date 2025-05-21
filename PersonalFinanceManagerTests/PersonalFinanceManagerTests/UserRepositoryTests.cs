using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;
using System.Data.Entity;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private Mock<DbSet<User>> _mockSet;
        private List<User> _userData;
        private UserRepository _userRepository;

        [TestInitialize]
        public void Initialize()
        {
            // Setup test data
            _userData = new List<User>
            {
                new User { Id = 1, UserName = "testuser1", Password = "password1" },
                new User { Id = 2, UserName = "testuser2", Password = "password2" }
            };

            // Setup mock DbSet
            _mockSet = new Mock<DbSet<User>>();
            _mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_userData.AsQueryable().Provider);
            _mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_userData.AsQueryable().Expression);
            _mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_userData.AsQueryable().ElementType);
            _mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_userData.AsQueryable().GetEnumerator());

            // Setup mock context
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _mockContext.Setup(c => c.Users).Returns(_mockSet.Object);

            // Create repository with mock context
            _userRepository = new UserRepository();
        }

        [TestMethod]
        public void CheckLogin_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var username = "testuser1";
            var password = "password1";

            // Use a test-specific mock to control the behavior for this test
            var mockContext = new Mock<PersonalFinanceManagerContext>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_userData.AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_userData.AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_userData.AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_userData.AsQueryable().GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Using a test-specific repository instance that we can inject our mock into
            var repository = new TestableUserRepository(mockContext.Object);

            // Act
            var result = repository.CheckLogin(username, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.UserName);
            Assert.AreEqual(password, result.Password);
        }

        [TestMethod]
        public void CheckLogin_WithInvalidCredentials_ReturnsNull()
        {
            // Arrange
            var username = "testuser1";
            var password = "wrongpassword";

            // Use a test-specific mock to control the behavior for this test
            var mockContext = new Mock<PersonalFinanceManagerContext>();
            var mockSet = new Mock<DbSet<User>>();

            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(_userData.AsQueryable().Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(_userData.AsQueryable().Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(_userData.AsQueryable().ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(_userData.AsQueryable().GetEnumerator());

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);

            // Using a test-specific repository instance that we can inject our mock into
            var repository = new TestableUserRepository(mockContext.Object);

            // Act
            var result = repository.CheckLogin(username, password);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void RegisterUser_WithValidData_ReturnsTrue()
        {
            // Arrange
            var username = "newuser";
            var password = "newpassword";

            var mockContext = new Mock<PersonalFinanceManagerContext>();
            var mockSet = new Mock<DbSet<User>>();

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChanges()).Returns(1); // Simulate successful save

            var repository = new TestableUserRepository(mockContext.Object);

            // Act
            var result = repository.RegisterUser(username, password);

            // Assert
            Assert.IsTrue(result);
            mockSet.Verify(m => m.Add(It.Is<User>(u =>
                u.UserName == username &&
                u.Password == password)), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void RegisterUser_WhenSaveChangesFails_ReturnsFalse()
        {
            // Arrange
            var username = "newuser";
            var password = "newpassword";

            var mockContext = new Mock<PersonalFinanceManagerContext>();
            var mockSet = new Mock<DbSet<User>>();

            mockContext.Setup(c => c.Users).Returns(mockSet.Object);
            mockContext.Setup(c => c.SaveChanges()).Returns(0); // Simulate failed save

            var repository = new TestableUserRepository(mockContext.Object);

            // Act
            var result = repository.RegisterUser(username, password);

            // Assert
            Assert.IsFalse(result);
            mockSet.Verify(m => m.Add(It.IsAny<User>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }
    }

    // Helper class to allow dependency injection of context for testing
    public class TestableUserRepository : UserRepository
    {
        private readonly PersonalFinanceManagerContext _context;

        public TestableUserRepository(PersonalFinanceManagerContext context)
        {
            _context = context;
        }

        protected override PersonalFinanceManagerContext GetContext()
        {
            return _context;
        }
    }
}