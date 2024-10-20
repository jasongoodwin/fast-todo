using FastEndpoints;
using Auth0.AspNetCore.Authentication;
using TodoListApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuth0WebAppAuthentication(options =>
{
    options.Domain = builder.Configuration["Auth0:Domain"];
    options.ClientId = builder.Configuration["Auth0:ClientId"];
    options.ClientSecret = builder.Configuration["Auth0:ClientSecret"];
});

builder.Services.AddFastEndpoints();
builder.Services.AddSingleton<TodoStorage>(sp => new TodoStorage("Filename=TodoList.db;Connection=shared"));

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UseFastEndpoints();

app.Run();