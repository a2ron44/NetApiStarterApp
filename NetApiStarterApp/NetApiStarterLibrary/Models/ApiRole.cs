using System;
using Microsoft.AspNetCore.Identity;
using NetApiStarterLibrary.Permissions;

namespace NetApiStarterLibrary.Models
{
	public class ApiRole : IdentityRole
	{
        public ApiRole() : base(){}

        public virtual ICollection<PermissionType> PermissionTypes { get; set; }
	}
}

