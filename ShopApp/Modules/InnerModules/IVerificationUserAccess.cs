using Microsoft.AspNetCore.Identity;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Modules.InnerModules
{
    public interface IVerificationUserAccess
    {
        Task<ValueTuple<string, bool>> Verify(User user, User loginedUser, UserManager<User> userManager, List<string> accessedRoles);
    }
}
