using Microsoft.AspNetCore.Authorization;

namespace NetApiStarterLibrary.Permissions
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

