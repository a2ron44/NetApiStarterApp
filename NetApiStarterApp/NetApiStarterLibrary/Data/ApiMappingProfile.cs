using AutoMapper;
using NetApiStarterLibrary.Models;

namespace NetApiStarterLibrary.Data
{
	public class ApiMappingProfile : Profile
	{
		public ApiMappingProfile()
		{
            //Auth OBJ
            CreateMap<ApiUser, ApiUserDTO>().ReverseMap();
        }
	}
}

