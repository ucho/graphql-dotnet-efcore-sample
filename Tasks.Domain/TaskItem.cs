using System;

namespace Tasks.Domain
{
    public class TaskItem
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }
    }
}
