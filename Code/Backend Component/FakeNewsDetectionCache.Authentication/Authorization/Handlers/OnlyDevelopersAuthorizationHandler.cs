using FakeNewsDetectionCache.Authentication.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Authentication.Authorization.Handlers
{
    public class OnlyDevelopersAuthorizationHandler: AuthorizationHandler<OnlyDevelopersRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyDevelopersRequirement requirement)
        {
            if (context.User.IsInRole(Roles.Developer))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
