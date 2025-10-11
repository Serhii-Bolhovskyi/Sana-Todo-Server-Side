using GraphQL;
using GraphQL.Types;
using Sana_Todo.GraphQL.Types;
using Sana_Todo.Models;
using Sana_Todo.Services;

namespace Sana_Todo.GraphQL.Mutations
{
    public class TaskMutation : ObjectGraphType
    {
        public TaskMutation(IStorageFactory factory, IHttpContextAccessor httpContextAccessor) {

            // визначення сховища
            var httpContext = httpContextAccessor.HttpContext;
            string storageOption = "db";
            if (httpContext != null && httpContext.Request.Cookies.TryGetValue("StorageOption", out var cookieValue))
            {
                storageOption = cookieValue;
            }
            var service = factory.CreateTaskImplement(storageOption);

            Field<TaskType>(
                "addTask",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<TaskInputType>> { Name = "task" }),
                resolve: context =>
                {
                    var task = context.GetArgument<TaskModel>("task");
                    if (task.CategoryId == null)
                        task.CategoryId = 5;
                    return service.AddTask(task);
                });
            Field<BooleanGraphType>(
                "deleteTask",
                arguments: new QueryArguments(new QueryArgument<NonNullGraphType<IntGraphType>> { Name = "id" }),
                resolve: context =>
                {
                    var id = context.GetArgument<int>("id");
                    return service.DeleteTask(id);
                });
            Field<TaskType>(
                "updateTask",
                arguments: new QueryArguments(
                  new QueryArgument<NonNullGraphType<TaskInputUpdate>> { Name = "task" }
                ),
                resolve: context =>
                {
                    var task = context.GetArgument<TaskModel>("task");
                    return service.CompleteTask(task);
                });

        }
    }
}
