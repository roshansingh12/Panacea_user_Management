using AutoMapper;
using Panacea_User_Management.Models;

namespace Panacea_User_Management.Profilers
{
    public class User_Mapper : Profile
    {
        public User_Mapper()
        {
            CreateMap<User_Model, UserDTO>();
            //CreateMap<User_Model, UserDTO>().ReverseMap();
        }
    }
}
