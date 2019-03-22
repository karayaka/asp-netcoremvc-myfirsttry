using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projeDeneme.Identity
{
    public class AppIdentityDbContex:IdentityDbContext<AppIdentityUser,AppIdentityRole,string>
    {
        public AppIdentityDbContex(DbContextOptions<AppIdentityDbContex> options):base(options)
        {

                
        }
    }
}
