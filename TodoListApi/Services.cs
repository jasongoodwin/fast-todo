using LiteDB;
using TodoListApi.Models;

namespace TodoListApi.Services
{
    public class TodoStorage
    {
        private readonly LiteDatabase _db;

        public TodoStorage(string connectionString)
        {
            _db = new LiteDatabase(connectionString);
        }

        public TodoList GetTodoList(string userId)
        {
            var collection = _db.GetCollection<TodoList>("todolists");
            return collection.FindOne(x => x.UserId == userId);
        }

        public void SaveTodoList(TodoList todoList)
        {
            var collection = _db.GetCollection<TodoList>("todolists");
            if (todoList.Id == ObjectId.Empty)
            {
                todoList.Id = ObjectId.NewObjectId();
            }
            collection.Upsert(todoList);
        }
    }
}