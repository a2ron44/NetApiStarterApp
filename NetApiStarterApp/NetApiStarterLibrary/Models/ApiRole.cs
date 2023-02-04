using System;
using Microsoft.AspNetCore.Identity;
using NetApiStarterApp.NetApiStarterLibrary.Permissions;
using NetApiStarterLibrary.Permissions;

namespace NetApiStarterLibrary.Models
{
	public class ApiRole : IdentityRole
	{
        public ApiRole() : base(){}

        public virtual ICollection<PermissionRole> PermissionRoles { get; set; }
	}
}

