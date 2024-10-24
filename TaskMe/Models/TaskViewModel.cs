using System.Collections.Generic;

namespace TaskMe.Models
{
    public class TaskViewModel
    {
        public IEnumerable<Task> RemainingTasks { get; set; }
        public IEnumerable<Task> CompletedTasks { get; set; }
    }
}
