using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Server.Models;

namespace TodoApi.Server.Data
{
    public class TodoApiServerContext : DbContext
    {
        public TodoApiServerContext (DbContextOptions<TodoApiServerContext> options)
            : base(options)
        {
        }

        public DbSet<TodoApi.Server.Models.TodoItem> TodoItem { get; set; } = default!;
    }
}
