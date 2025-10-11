using System.Runtime.CompilerServices;
using System.Security.Cryptography.Xml;
using System.Xml.Linq;
using Sana_Todo.Models;

namespace Sana_Todo.Services
{
    public class XmlTaskService : ITaskImplement
    {
        private readonly string _path = "wwwroot/taskList.xml";

        public List<TaskModel> GetAllTasks()
        {
            var doc = XDocument.Load(_path);
            return doc.Root.Element("Tasks")?.Elements("Task")
                 .Select(x => new TaskModel
                 {
                     Id = int.TryParse(x.Element("Id")?.Value, out var id) ? id : 0,
                     Title = x.Element("Title")?.Value,
                     CategoryId = int.TryParse(x.Element("CategoryId")?.Value, out var catId) ? catId : 5,
                     Deadline = DateTime.TryParse(x.Element("Deadline")?.Value, out var deadline)
                      ? deadline
                      : (DateTime?)null,
                     IsCompleted = bool.TryParse(x.Element("IsCompleted")?.Value, out var isCompleted) && isCompleted,
                     CompleteDate = DateTime.TryParse(x.Element("CompleteDate")?.Value, out var completeDate)
                       ? completeDate
                       : (DateTime?)null,

                 }).ToList() ?? new List<TaskModel>();
        }
        public List<CategoryModel> GetAllCategories()
        {
            var doc = XDocument.Load(_path);
            return doc.Root.Element("Categories")?.Elements("Category")
                .Select(x => new CategoryModel
                {
                    Id = int.Parse(x.Element("Id").Value),
                    Name = x.Element("Name").Value,

                }).ToList() ?? new List<CategoryModel>();
        }

        public TaskModel AddTask(TaskModel task)
        {
            var doc = XDocument.Load(_path);
            var existingIds = doc.Root.Element("Tasks")?
                .Elements("Task")
                .Select(x => int.TryParse(x.Element("Id")?.Value, out var id) ? id : 0)
                .ToList() ?? new List<int>();

            var newId = existingIds.Any() ? existingIds.Max() + 1 : 1;
            var newTask = new XElement("Task",
                new XElement("Id", newId),
                new XElement("Title", task.Title),
                new XElement("CategoryId", task.CategoryId),
                new XElement("Deadline", task.Deadline?.ToString("yyyy-MM-dd")),
                new XElement("IsCompleted", task.IsCompleted.ToString().ToLower())
            );
            doc.Root.Element("Tasks")?.Add(newTask);
            doc.Save(_path);
            return task;
        }

        public bool DeleteTask(int Id)
        {
            var doc = XDocument.Load(_path);
            var taskToDelete = doc.Root.Element("Tasks")?
                .Elements("Task")
                .FirstOrDefault(task => int.TryParse(task.Element("Id")?.Value, out var taskId) && taskId == Id);
            if (taskToDelete != null)
            {
                taskToDelete.Remove();
                doc.Save(_path);
                return true;
            }
            return false;
        }
        public TaskModel CompleteTask(TaskModel task)
        {
            var doc = XDocument.Load(_path);
            var taskToUpdate = doc.Root.Element("Tasks")?
                .Elements("Task")
                .FirstOrDefault(x => int.TryParse(x.Element("Id")?.Value, out var taskId) && taskId == task.Id);
            var currentState = bool.TryParse(taskToUpdate.Element("IsCompleted")?.Value, out var completed) && completed;
            if (taskToUpdate != null)
            {
                taskToUpdate.SetElementValue("IsCompleted", !completed);
                if (!completed)
                {
                    taskToUpdate.SetElementValue("CompleteDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                }
                else
                {
                    taskToUpdate.Element("CompleteDate")?.Remove();
                }

                doc.Save(_path);

                if(taskToUpdate != null)
                {
                    return new TaskModel
                    {
                        Id = int.Parse(taskToUpdate.Element("Id")?.Value ?? "0"),
                        Title = taskToUpdate.Element("Title")?.Value,
                        CategoryId = int.TryParse(taskToUpdate.Element("CategoryId")?.Value, out var catId) ? catId : (int?)null,
                        Deadline = DateTime.TryParse(taskToUpdate.Element("Deadline")?.Value, out var deadline) ? deadline : (DateTime?)null,
                        IsCompleted = bool.TryParse(taskToUpdate.Element("IsCompleted")?.Value, out var isComp) && isComp,
                        CompleteDate = DateTime.TryParse(taskToUpdate.Element("CompleteDate")?.Value, out var compDate) ? compDate : (DateTime?)null
                    };
                } else { Console.WriteLine($"Task with ID {task.Id} not found in XML."); }
            }
            return null;
        }
    }
}
