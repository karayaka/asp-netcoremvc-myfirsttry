using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace projeDeneme.Identity
{
    public class AppIdentityUser:IdentityUser
    {
        public string userImage { get; set; }
    }
}
