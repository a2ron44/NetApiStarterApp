using AutoMapper;
using Net6StarterApp.Authentication.Models;

namespace Net6StarterApp.Data
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{

			//Auth OBJ
			CreateMap<ApiUser, UserDTO>().ReverseMap();
		}
	}
}

