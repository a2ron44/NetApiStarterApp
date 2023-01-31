using System;
using System.ComponentModel.DataAnnotations;

namespace NetApiStarterLibrary.Models
{
	public class LoginUserDTO
	{
		[Required]
		[DataType(DataType.EmailAddress)]
		public string Email { get; set; }

		[Required]
		[StringLength(15, MinimumLength = 6)]
		public string Password { get; set; }
	}

    public class UserDTO : LoginUserDTO
    {
        
    }
}

