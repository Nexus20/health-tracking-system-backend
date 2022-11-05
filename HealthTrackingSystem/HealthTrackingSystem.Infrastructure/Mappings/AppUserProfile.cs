using AutoMapper;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Identity;

namespace HealthTrackingSystem.Infrastructure.Mappings;

public class AppUserProfile : Profile
{
    public AppUserProfile()
    {
        CreateMap<User, AppUser>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.Email))
            .ForMember(d => d.PhoneNumber, o => o.MapFrom(s => s.Phone));
    }
}