using System;
using Microsoft.EntityFrameworkCore;

namespace RGR
{

    public class UsersContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UsersContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=Users.db");
        }
    }
    public class User
    {
        public string Login { get; set; }

        public Int64 N { get; set; }
        public Int64 ID { get; set; }
    };
}
