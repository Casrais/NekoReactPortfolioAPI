using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosAPI.Models
{
    public class Identity : IdentityUser
    {
        //public string DisplayName { get; set; }
    }

    public class Identity_UserRoles : IdentityRole
    {
    }

    public class Identity_Tokens : IdentityUserToken<string>
    {
    }
}
