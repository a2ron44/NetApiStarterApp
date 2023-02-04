using System;
using System.ComponentModel.DataAnnotations.Schema;
using NetApiStarterLibrary.Models;
using NetApiStarterLibrary.Permissions;

namespace NetApiStarterApp.NetApiStarterLibrary.Permissions
{
    [Table("permission_role")]
    public class PermissionRole
	{
		public Guid Id { get; set; }

		public int PermissionId { get; set; }

		public string RoleId { get; set; }

		public virtual Permission Permission { get; set; }

        public virtual ApiRole Role { get; set; }
    }
}

