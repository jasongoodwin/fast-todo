using FastEndpoints;
using TodoListApi.Models;
using TodoListApi.Services;

namespace TodoListApi.Endpoints
{
    public class GetTodoListEndpoint : AuthenticatedEndpoint<EmptyRequest, TodoList>
    {
        private readonly TodoStorage _storage;

        public GetTodoListEndpoint(TodoStorage storage)
        {
            _storage = storage;
        }

        public override void Configure()
        {
            Get("/todolist");
            // Authentication is required by default, so we don't need to call AllowAnonymous
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var userId = GetUserId();
            var todoList = _storage.GetTodoList(userId);
            if (todoList == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }
            await SendAsync(todoList, cancellation: ct);
        }
    }

    public class UpdateTodoListEndpoint : AuthenticatedEndpoint<TodoList, TodoList>
    {
        private readonly TodoStorage _storage;

        public UpdateTodoListEndpoint(TodoStorage storage)
        {
            _storage = storage;
        }

        public override void Configure()
        {
            Put("/todolist");
            // Authentication is required by default, so we don't need to call AllowAnonymous
        }

        public override async Task HandleAsync(TodoList todoList, CancellationToken ct)
        {
            todoList.UserId = GetUserId();
            _storage.SaveTodoList(todoList);
            await SendAsync(todoList, cancellation: ct);
        }
    }
}