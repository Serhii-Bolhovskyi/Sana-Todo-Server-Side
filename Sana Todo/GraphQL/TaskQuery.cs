using GraphQL.Types;
using Sana_Todo.GraphQL.Types;
using Sana_Todo.Services;

namespace Sana_Todo.GraphQL
{
    public class TaskQuery : ObjectGraphType
    {
        public TaskQuery(IStorageFactory factory, IHttpContextAccessor httpContextAccessor) {
            Field<ListGraphType<TaskType>>("tasks",
                resolve: context =>
                {
                    var httpContext = httpContextAccessor.HttpContext;
                    string storageOption = "db";
                    if (httpContext != null && httpContext.Request.Cookies.TryGetValue("StorageOption", out var cookieValue))
                    {
                        storageOption = cookieValue;
                    }

                    var service = factory.CreateTaskImplement(storageOption);
                    return service.GetAllTasks();
                }
                );
            Field<ListGraphType<CategoryType>>(
                "categories", 
                resolve: context => factory.CreateTaskImplement("db").GetAllCategories()
                );
        }
    }
}
