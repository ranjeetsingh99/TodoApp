using Microsoft.EntityFrameworkCore;
using Todo.Models;

namespace Todo.Data
{

    public class AppDbContext : DbContext
    {
        public  AppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<TaskItem> Tasks { get; set; }

    }
}
