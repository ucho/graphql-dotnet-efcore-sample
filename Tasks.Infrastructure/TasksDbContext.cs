using Microsoft.EntityFrameworkCore;
using System;
using Tasks.Domain;

namespace Tasks.Infrastructure
{
    public class TasksDbContext : DbContext
    {
        public DbSet<TaskItem> Tasks { get; set; }

        public TasksDbContext(DbContextOptions<TasksDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskItem>().HasData(
                new TaskItem { Id = 1, Title = "牛乳を買う", DueDate = DateTime.Parse("2021/01/10"), Description = "近所のスーパーで買いたい" },
                new TaskItem { Id = 2, Title = "粗大ごみを出す", DueDate = DateTime.Parse("2021/01/21"), Description = "回収券を貼ること" },
                new TaskItem { Id = 3, Title = "休暇を申請する", DueDate = DateTime.Parse("2021/01/22"), Description = "チームへ共有を忘れずに" }
            );
        }
    }
}
