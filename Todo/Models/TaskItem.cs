using System.ComponentModel.DataAnnotations;

namespace Todo.Models
{
    public class TaskItem
    {
        [Key]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Description cannot be empty.")]
        [RegularExpression(@"^(?!\s*$).+", ErrorMessage = "Description cannot be just spaces.")]
        public string? Description { get; set; }
        public bool IsCompleted { get; set; } = false;


        public void taskPrint()
        {
            string taskid = "task id = " + this.TaskId + "........................\ntask des = "+ this.Description+ "................\n";
            Console.WriteLine(taskid);
        }
    }

    
}
    