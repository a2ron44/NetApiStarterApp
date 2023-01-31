using System;
using Microsoft.AspNetCore.Authorization;

namespace Net6StarterApp.Authentication.Permissions
{
	public class PermissionRequirement : IAuthorizationRequirement
	{
		public PermissionRequirement(string permission)
		{
			Permission = permission;
		}

		public string Permission { get;  }
	}
}

