using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ShopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopApp.Modules.InnerModules
{
    public class VerificationUserAccess : IVerificationUserAccess
    {

        public async Task<ValueTuple<string, bool>> Verify(User user, User loginedUser, UserManager<User> userManager, List<string> accessedRoles)
        {
            if (loginedUser == null || user == null)
                return (null, false);

            if (loginedUser.Id == user.Id)
                return (user.Id, true);

            foreach (var item in accessedRoles)
                if (await userManager.IsInRoleAsync(loginedUser, item))
                    return (user.Id, true);

            return (null,false);
        }
    }
}
