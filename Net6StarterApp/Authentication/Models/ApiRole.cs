using System;
using Microsoft.AspNetCore.Identity;
using Net6StarterApp.Authentication.Permissions;

namespace Net6StarterApp.Authentication.Models
{
	public class ApiRole : IdentityRole
	{
        public ApiRole() : base(){}

        public virtual ICollection<PermissionType> PermissionTypes { get; set; }
	}
}

