using System.Diagnostics;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Sana_Todo.Models;
using Sana_Todo.Services;

namespace Sana_Todo.Controllers
{
  
    public class TaskController : Controller
    {
        private readonly IStorageFactory _storageFactory;
        public TaskController(TaskService taskService, XmlTaskService xmlTaskService, IStorageFactory storageFactory)
        {
            _storageFactory = storageFactory;
        }

        private ITaskImplement GetTaskService()
        {
            var storageOption = Request.Cookies["StorageOption"];
            return _storageFactory.CreateTaskImplement(storageOption);
        }

        [HttpPost]
        public IActionResult SetStorageOption(string storageOption)
        {

            HttpContext.Response.Cookies.Append("StorageOption", storageOption);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var taskService = GetTaskService();

            var tasks = taskService.GetAllTasks();
            var categories = taskService.GetAllCategories();
            var todoModel = new TodoModel
            {
                Tasks = tasks,
                Categories = categories,
                StorageOption = Request.Cookies["StorageOption"]
            };

            return View(todoModel);
        }

        private const int DefaultCategoryId = 5;

        [HttpPost]
        public IActionResult AddTask(TaskModel task)
        {
            var taskService = GetTaskService();

            if (task.CategoryId == null)
            {
                    task.CategoryId = DefaultCategoryId;
            }
            
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            taskService.AddTask(task);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteTask(int Id)
        {
            var taskService = GetTaskService();
            taskService.DeleteTask(Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CompleteTask(TaskModel task)
        {
            var taskService = GetTaskService();

            taskService.CompleteTask(task);
            
            return RedirectToAction("Index");
        }
    }

  
}
