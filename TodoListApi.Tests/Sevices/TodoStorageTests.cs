using Xunit;
using TodoListApi.Models;
using TodoListApi.Services;
using System;
using System.IO;
using LiteDB;
using System.Linq;

namespace TodoListApi.Tests
{
    public class TodoStorageTests : IDisposable
    {
        private readonly TodoStorage _storage;
        private readonly string _dbPath = "TestTodoList.db";

        public TodoStorageTests()
        {
            _storage = new TodoStorage($"Filename={_dbPath};Connection=shared");
        }

        [Fact]
        public void CanSaveAndRetrieveTodoList()
        {
            // Arrange
            var todoList = new TodoList
            {
                UserId = "testUser",
                Title = "Test List",
                Items = new List<TodoItem>
                {
                    new TodoItem { Title = "Test Item", IsCompleted = false }
                }
            };

            // Act
            _storage.SaveTodoList(todoList);
            var retrievedList = _storage.GetTodoList("testUser");

            // Assert
            Assert.NotNull(retrievedList);
            Assert.Equal(todoList.Title, retrievedList.Title);
            Assert.Single(retrievedList.Items);
            Assert.Equal(todoList.Items[0].Title, retrievedList.Items[0].Title);
            Assert.NotEqual(ObjectId.Empty, retrievedList.Id);
        }

        [Fact]
        public void GetTodoList_ReturnsNull_WhenUserIdNotFound()
        {
            var result = _storage.GetTodoList("nonexistentUser");
            Assert.Null(result);
        }

        [Fact]
        public void SaveTodoList_UpdatesExistingList_WhenUserIdExists()
        {
            // Arrange
            var originalList = new TodoList
            {
                UserId = "existingUser",
                Title = "Original Title",
                Items = new List<TodoItem> { new TodoItem { Title = "Original Item" } }
            };
            _storage.SaveTodoList(originalList);

            // Act
            var updatedList = new TodoList
            {
                Id = originalList.Id,
                UserId = "existingUser",
                Title = "Updated Title",
                Items = new List<TodoItem> { new TodoItem { Title = "Updated Item" } }
            };
            _storage.SaveTodoList(updatedList);

            // Assert
            var retrievedList = _storage.GetTodoList("existingUser");
            Assert.Equal("Updated Title", retrievedList.Title);
            Assert.Single(retrievedList.Items);
            Assert.Equal("Updated Item", retrievedList.Items[0].Title);
        }

        [Fact]
        public void SaveTodoList_PreservesItemsOrder()
        {
            // Arrange
            var todoList = new TodoList
            {
                UserId = "orderTestUser",
                Title = "Order Test",
                Items = new List<TodoItem>
                {
                    new TodoItem { Title = "First" },
                    new TodoItem { Title = "Second" },
                    new TodoItem { Title = "Third" }
                }
            };

            // Act
            _storage.SaveTodoList(todoList);
            var retrievedList = _storage.GetTodoList("orderTestUser");

            // Assert
            Assert.Equal(3, retrievedList.Items.Count);
            Assert.Equal("First", retrievedList.Items[0].Title);
            Assert.Equal("Second", retrievedList.Items[1].Title);
            Assert.Equal("Third", retrievedList.Items[2].Title);
        }

        [Fact]
        public void SaveTodoList_HandlesLargeNumberOfItems()
        {
            // Arrange
            var largeList = new TodoList
            {
                UserId = "largeListUser",
                Title = "Large List",
                Items = Enumerable.Range(1, 1000).Select(i => new TodoItem { Title = $"Item {i}" }).ToList()
            };

            // Act
            _storage.SaveTodoList(largeList);
            var retrievedList = _storage.GetTodoList("largeListUser");

            // Assert
            Assert.Equal(1000, retrievedList.Items.Count);
            Assert.Equal("Item 1", retrievedList.Items[0].Title);
            Assert.Equal("Item 1000", retrievedList.Items[999].Title);
        }

        public void Dispose()
        {
            if (File.Exists(_dbPath))
            {
                File.Delete(_dbPath);
            }
        }
    }
}