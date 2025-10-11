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

        [HttpPost]
        public IActionResult SetStorageOption(string storageOption)
        {

            HttpContext.Response.Cookies.Append("StorageOption", storageOption);
            return RedirectToAction("Index");
        }
        public IActionResult Index()
        {
            var storageOption = Request.Cookies["StorageOption"];
            Console.WriteLine($"[DEBUG] storageOption: {storageOption}");

            var taskService = _storageFactory.CreateTaskImplement(storageOption);

            var tasks = taskService.GetAllTasks();
            var categories = taskService.GetAllCategories();
            var todoModel = new TodoModel
            {
                Tasks = tasks,
                Categories = categories,
                StorageOption = storageOption
            };

            return View(todoModel);
        }

        [HttpPost]
        public IActionResult AddTask(TaskModel task)
        {
            var storageOption = Request.Cookies["StorageOption"];
            var taskService = _storageFactory.CreateTaskImplement(storageOption);
            if (task.CategoryId == null)
            {
                    task.CategoryId = 5;
            }
            taskService.AddTask(task);
            if (!ModelState.IsValid)
            {
                return View("Index");
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteTask(int Id)
        {
            var storageOption = Request.Cookies["StorageOption"];
            var taskService = _storageFactory.CreateTaskImplement(storageOption);
            taskService.DeleteTask(Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CompleteTask(TaskModel task)
        {
            var storageOption = Request.Cookies["StorageOption"];
            var taskService = _storageFactory.CreateTaskImplement(storageOption);

                taskService.CompleteTask(task);


                return RedirectToAction("Index");
        }
    }

  
}
