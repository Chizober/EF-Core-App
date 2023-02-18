using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace EF_Core_DAL
{
    public class ToDoAppDbContextFactory: IDesignTimeDbContextFactory<ToDoDbContext>
    {
        public ToDoAppDbContextFactory()
        {

        }
        public ToDoDbContext CreateDbContext(string[] args)
        {
            var OptionBuilder = new DbContextOptionsBuilder<ToDoDbContext>();
            var ConnectionString = @"Data Source=LAPTOP-3L0EUQLC\SQLEXPRESS;Integrated Security=True;Initial Catalog=TodoDb;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            OptionBuilder.UseSqlServer(ConnectionString);
            return new ToDoDbContext(OptionBuilder.Options);
        }
    }
}
