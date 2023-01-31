using Microsoft.AspNetCore.Authorization;

namespace NetApiStarterLibrary.Permissions
{
	public class HasPermissionAttribute : AuthorizeAttribute
	{
		public HasPermissionAttribute(int permission): base(policy: permission.ToString())
		{
		}
	}
}

