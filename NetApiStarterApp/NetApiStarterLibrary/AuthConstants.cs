using System;
namespace NetApiStarterLibrary
{
    public static class AuthConstants
    {
        public const string DEFAULT_PASSWORD = "Password!23";

        public const string SUPERADMIN_ROLE = "SuperAdmin";

        public const string InvalidLogin = "Invalid Login";

        public const string UnknownAuthError = "Unknown Error";

        public const string UknownRegistrationError = "Unknown Error in User Registration";

        public const string CustomClaimPermissions = "permissions";

        public static string GetDefaultJwtKey()
        {
            return "f20eabfb-ed2c-4a78-914e-96cb476c150c";
        }
    }

   
}

