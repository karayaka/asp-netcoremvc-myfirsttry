using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.EntityFrameworkCore;


namespace projeDeneme.Models
{
    public class ModelmysqlContext:DbContext
    {
        

        public ModelmysqlContext(DbContextOptions<ModelmysqlContext>options):base(options)
        {


        }
        public DbSet<Modelmysql> Modelmysqls { get; set; }
        public DbSet<yeniModel> YeniModels { get; set; }



        //public DbSet<CustumUser> User { get; set; }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlite("Data Source=database.db");
        //    //optionsBuilder.UseMySql(@"server")
        //    //optionsBuilder.UseMySql("Server=localhost;Database=Modelmysql;User=root;Password=;");

        //}

    }
}
