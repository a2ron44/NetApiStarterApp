using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Microsoft.AspNetCore.Identity;
using Net6StarterApp.Authentication.Models;

namespace Net6StarterApp.Authentication.Permissions
{
	public static class Permission
	{
		//list of all permissions in system, to be used by contollers to Authorize via Permission Policy
		public const int ViewData = 1;

		public const int EditData = 2;

		public const int CreateData = 3;

	}

	//Add Roles used in System.  Must add to SeeDataRoles too.
    public static class RoleType
    {
        public const string NormalUser = "User";

        public const string SuperAdmin = "SuperAdmin";

        public const string Support = "Support";

    }

	[Table("permission_type")]
    public class PermissionType
    {
		public int Id { get; set; }

		public string Name { get; set; }

		public ICollection<ApiRole> Roles { get; set; }

		//Uses reflection to add Permissions.  Just add new permission to Permission class and assign Int.
		//Anything new will be added to data seed on next migration
        public static List<PermissionType> GetAllPermissions()
        {
			List<PermissionType> permList = new List<PermissionType>();

			foreach (FieldInfo info in typeof(Permission).GetFields().Where(x => x.IsStatic && x.IsLiteral))
			{
				permList.Add(new PermissionType
                {
					Id = (int)info.GetValue(null),
					Name = info.Name
				}) ;

			}

			return permList;
        }
    }

}

