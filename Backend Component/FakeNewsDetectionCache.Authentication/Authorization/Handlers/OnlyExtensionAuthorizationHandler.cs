using FakeNewsDetectionCache.Authentication.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FakeNewsDetectionCache.Authentication.Authorization.Handlers
{
    public class OnlyExtensionAuthorizationHandler: AuthorizationHandler<OnlyExtensionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OnlyExtensionRequirement requirement)
        {
            if (context.User.IsInRole(Roles.Extension)||context.User.IsInRole(Roles.Developer))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
