using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using NetApiStarterApp.NetApiStarterLibrary.Permissions;
using NetApiStarterLibrary.Models;

namespace NetApiStarterLibrary.Permissions
{
	public class PermissionType
	{
		//list of all permissions in system, to be used by contollers to Authorize via Permission Policy
		public const int DefaultAccess = 1;

        public const int ViewData = 2;

		public const int EditData = 3;

		public const int CreateData = 4;

        public const int DeleteData = 5;

    }

	//Add Roles used in System.  Must add to SeeDataRoles too.
    public static class RoleType
    {
        public const string NormalUser = "User";

        public const string SuperAdmin = "SuperAdmin";

        public const string Support = "Support";

    }

	[Table("permission")]
    public class Permission
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public ICollection<PermissionRole> PermissionRoles { get; set; }

		//Uses reflection to add Permissions.  Just add new permission to Permission class and assign Int.
		//Anything new will be added to data seed on next migration
        public static List<Permission> GetAllPermissions() 
        {
			List<Permission> permList = new List<Permission>();

			foreach (FieldInfo info in typeof(PermissionType).GetFields().Where(x => x.IsStatic && x.IsLiteral))
			{
				permList.Add(new Permission
                {
					Id = (int)info.GetValue(null),
					Name = info.Name
				}) ;

			}

			return permList;
        }
    }

}

