using GraphQL.Types;
using Sana_Todo.Models;

namespace Sana_Todo.GraphQL.Types
{
    public class TaskInputType : InputObjectGraphType<TaskModel>
    {
        public TaskInputType()
        {
            Name = "taskInput";
            Field(x => x.Title);
            Field(x => x.CategoryId, nullable: true);
            Field(x => x.Deadline, nullable: true);
        }
    }
}
