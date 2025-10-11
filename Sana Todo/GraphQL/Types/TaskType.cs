using GraphQL.Types;
using Sana_Todo.Models;

namespace Sana_Todo.GraphQL.Types
{
    public class TaskType : ObjectGraphType<TaskModel>
    {
        public TaskType()
        {
            Field(x => x.Id);
            Field(x => x.Title);
            Field(x => x.CategoryId, nullable: true);
            Field(x => x.Deadline, nullable: true);
            Field(x => x.CompleteDate, nullable: true);
            Field(x => x.IsCompleted);
        }
    }
}
