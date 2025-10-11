using GraphQL.Types;
using Sana_Todo.Models;

namespace Sana_Todo.GraphQL.Types
{
    public class TaskInputUpdate : InputObjectGraphType<TaskModel>
    {
        public TaskInputUpdate() { 
            Name = "taskUpdateInput";
            Field(x => x.Id);
            Field(x => x.IsCompleted);
        }
    }
}
