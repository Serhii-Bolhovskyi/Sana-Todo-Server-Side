namespace Sana_Todo.Models
{
    public class TodoModel
    {
        public List<TaskModel> Tasks { get; set; }
        public List<CategoryModel> Categories { get; set; }

        public string? StorageOption { get; set; } = "db";
    }
}
