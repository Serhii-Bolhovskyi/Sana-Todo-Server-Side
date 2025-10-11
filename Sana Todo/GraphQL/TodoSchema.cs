using GraphQL.Types;
using Sana_Todo.GraphQL.Mutations;

namespace Sana_Todo.GraphQL
{
    public class TodoSchema : Schema
    {
        public TodoSchema(IServiceProvider provider) : base(provider) { 
            Query = provider.GetRequiredService<TaskQuery>();
            Mutation = provider.GetRequiredService<TaskMutation>();
        }
    }
}
