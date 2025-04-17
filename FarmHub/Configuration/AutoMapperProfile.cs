using AutoMapper;
using FarmHub.Models;
using Microsoft.AspNetCore.Identity;

namespace FarmHub.Configuration
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {

            CreateMap<IdentityUser, ApplicationUser>();

        }
    }
}
