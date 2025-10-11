using Sana_Todo.Models;

namespace Sana_Todo.Services
{
    public interface ITaskImplement
    {
        List<TaskModel> GetAllTasks();
        List<CategoryModel> GetAllCategories();
        TaskModel AddTask(TaskModel task);
        bool DeleteTask(int Id);
        TaskModel CompleteTask(TaskModel task);

    }
}
