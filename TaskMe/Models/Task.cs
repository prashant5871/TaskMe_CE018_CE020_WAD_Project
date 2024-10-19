using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskMe.Models
{
    public enum TaskPriority
    {
        Low = 1,
        Medium = 2,
        High = 3,
        Urgent = 4
    }

    public class Task
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool IsCompleted { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium; // Default priority set to Medium

        // New property for due date
        [Required]
        public DateTime DueDate { get; set; } // Add a DueDate property
    }
}
