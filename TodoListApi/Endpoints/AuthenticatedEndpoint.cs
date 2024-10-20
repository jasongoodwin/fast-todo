using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace TodoListApi.Endpoints
{
    [Authorize]
    public abstract class AuthenticatedEndpoint<TRequest, TResponse> : Endpoint<TRequest, TResponse>
    {
        protected string GetUserId() => User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
    }
}