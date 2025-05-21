using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PersonalFinanceManager.Model;
using PersonalFinanceManager.Repository;

namespace PersonalFinanceManagerTests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private Mock<PersonalFinanceManagerContext> _mockContext;
        private Mock<DbSet<User>> _mockDbSet;
        private List<User> _data;
        private UserRepository _repository;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize test data
            _data = new List<User>
            {
                new User { Id = 1, UserName = "admin", Password = "admin", Role = UserRole.Admin },
                new User { Id = 2, UserName = "user1", Password = "password1", Role = UserRole.User },
                new User { Id = 3, UserName = "user2", Password = "password2", Role = UserRole.User }
            };

            // Set up the mock DbSet
            _mockDbSet = MockDbSet(_data);

            // Set up the mock context
            _mockContext = new Mock<PersonalFinanceManagerContext>();
            _mockContext.Setup(c => c.Users).Returns(_mockDbSet.Object);

            // Create repository with mock context
            _repository = new TestUserRepository(_mockContext.Object);
        }

        private class TestUserRepository : UserRepository
        {
            private readonly PersonalFinanceManagerContext _context;

            public TestUserRepository(PersonalFinanceManagerContext context)
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
        public void GetAdministrators_ShouldReturnOnlyAdmins()
        {
            // Act
            var result = _repository.GetAdministrators().ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("admin", result[0].UserName);
            Assert.AreEqual(UserRole.Admin, result[0].Role);
        }



        [TestMethod]
        public void CheckLogin_WithNonExistingUser_ShouldReturnNull()
        {
            // Act
            var result = _repository.CheckLogin("nonexistent", "password");

            // Assert
            Assert.IsNull(result);
        }





        [TestMethod]
        public void AdminExists_WhenAdminExists_ShouldReturnTrue()
        {
            // Act
            var result = _repository.AdminExists();

            // Assert
            Assert.IsTrue(result);
        }





        [TestMethod]
        public void CreateAdminIfNotExists_WhenAdminExists_ShouldNotCreateAdmin()
        {
            // Act
            _repository.CreateAdminIfNotExists();

            // Assert
            _mockDbSet.Verify(m => m.Add(It.IsAny<User>()), Times.Never);
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void RegisterUser_WithNewUsername_ShouldCreateUserAndReturnTrue()
        {
            // Arrange
            string username = "newuser";
            string password = "password";
            _mockContext.Setup(m => m.SaveChanges()).Returns(1);

            // Act
            var result = _repository.RegisterUser(username, password);

            // Assert
            Assert.IsTrue(result);
            _mockDbSet.Verify(m => m.Add(It.Is<User>(u =>
                u.UserName == username &&
                u.Password == password &&
                u.Role == UserRole.User)), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void RegisterUser_WithExistingUsername_ShouldReturnFalse()
        {
            // Arrange
            string username = "user1"; // Existing username
            string password = "newpassword";

            // Act
            var result = _repository.RegisterUser(username, password);

            // Assert
            Assert.IsFalse(result);
            _mockDbSet.Verify(m => m.Add(It.IsAny<User>()), Times.Never);
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }

        [TestMethod]
        public void CreateGuestUser_ShouldReturnGuestUserWithUniqueNameAndCorrectRole()
        {
            // Act
            var result = _repository.CreateGuestUser();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.UserName.StartsWith("Guest_"));
            Assert.AreEqual("guest_temp_password", result.Password);
            Assert.AreEqual(UserRole.Guest, result.Role);
        }




        [TestMethod]
        public void UpdateUser_WithNonExistingUser_ShouldReturnFalse()
        {
            // Arrange
            var userToUpdate = new User { Id = 999, UserName = "nonexistent", Role = UserRole.User };
            _mockContext.Setup(m => m.Users.Find(userToUpdate.Id)).Returns((User)null);

            // Act
            var result = _repository.UpdateUser(userToUpdate);

            // Assert
            Assert.IsFalse(result);
            _mockContext.Verify(m => m.SaveChanges(), Times.Never);
        }
    }
}