using AutoMapper;
using HealthTrackingSystem.Application.Models.Results.Users;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Mappings;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, ProfileResult>();
    }
}