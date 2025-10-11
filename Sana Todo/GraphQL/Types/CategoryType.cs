using GraphQL.Types;
using Sana_Todo.Models;

namespace Sana_Todo.GraphQL.Types
{
    public class CategoryType : ObjectGraphType<CategoryModel>
    {
        public CategoryType() {
            Field(x => x.Id);
            Field(x => x.Name);
        }
    }
}
