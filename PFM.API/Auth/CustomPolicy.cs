using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PFM.API.Auth
{
    public class CustomPolicy : AuthorizationHandler<CustomPolicy>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomPolicy requirement)
        {
            if (context.User.Identity is ClaimsIdentity identity)
            {
            }

            return Task.CompletedTask;
        }
    }
}
