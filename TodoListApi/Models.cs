using System;
using System.Collections.Generic;
using LiteDB;

namespace TodoListApi.Models
{
    public class TodoList
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string UserId { get; set; }
        public string Title { get; set; }
        public List<TodoItem> Items { get; set; } = new List<TodoItem>();
    }

    public class TodoItem
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? DueDate { get; set; }
        public int Priority { get; set; }
    }
}