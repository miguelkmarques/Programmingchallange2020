using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Database.Data;
using Microsoft.EntityFrameworkCore;

namespace Database
{
    class DatabaseContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql("Server=localhost;Database=movielens;Port=5432;User Id=postgres;Password=postgres;");
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
