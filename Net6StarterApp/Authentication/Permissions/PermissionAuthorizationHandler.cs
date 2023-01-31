using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Net6StarterApp.Authentication.Permissions
{
	public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
	{

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {

			string? userName = context.User.Claims.FirstOrDefault(c=> c.Type == ClaimTypes.Name)?.Value;

			if(userName == null)
			{
				return Task.CompletedTask;
			}

            //assumes permissions are in JWT   https://www.youtube.com/watch?v=SZtZuvcMBA0  for DB lookup implementation
            var permissionClaims = context.User.Claims
				.Where(x => x.Type == AuthConstants.CustomClaimPermissions)
				.Select(x => x.Value)
				.ToList();

			if (permissionClaims.Contains(requirement.Permission))
			{
				context.Succeed(requirement);
			}

            return Task.CompletedTask;
        }
    }
}

