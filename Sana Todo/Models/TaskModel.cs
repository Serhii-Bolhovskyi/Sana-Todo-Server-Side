using System.ComponentModel.DataAnnotations;
namespace Sana_Todo.Models
{
    
    public class TaskModel
    {
     
        public int Id { get; set; }

        [Required(ErrorMessage = "Task Name is required")]
        public string Title { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? Deadline { get; set; }
        public DateTime? CompleteDate { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
