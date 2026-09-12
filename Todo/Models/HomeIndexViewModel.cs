namespace Todo.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<TaskItem>? Tasks { get; set; }
        public TaskItem NewTask { get; set; } = new TaskItem();
    }
}
