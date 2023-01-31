using System;
using Microsoft.AspNetCore.Authorization;

namespace Net6StarterApp.Authentication.Permissions
{
	public class HasPermissionAttribute : AuthorizeAttribute
	{
		public HasPermissionAttribute(int permission): base(policy: permission.ToString())
		{
		}
	}
}

