using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Factory
{
    public class OvipDbContextFactory
    : IDesignTimeDbContextFactory<OvipDbContext>
    {
        public OvipDbContext CreateDbContext(string[] args)
        {
            var connectionString =
                "Server=localhost;Port=3306;Database=OVIP_Seged_Raktarkezelo;User=root;Password=;";

            var optionsBuilder =
                new DbContextOptionsBuilder<OvipDbContext>();

            optionsBuilder.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString));

            return new OvipDbContext(optionsBuilder.Options);
        }
    }
}