using AutoMapper;
using HealthTrackingSystem.Application.Models.Requests.HospitalAdministrators;
using HealthTrackingSystem.Application.Models.Results.HospitalAdministrators;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Mappings;

public class HospitalAdministratorProfile : Profile
{
    public HospitalAdministratorProfile()
    {
        CreateMap<CreateHospitalAdministratorRequest, HospitalAdministrator>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<CreateHospitalAdministratorRequest, User>();
        
        CreateMap<UpdateHospitalAdministratorRequest, HospitalAdministrator>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<UpdateHospitalAdministratorRequest, User>();

        CreateMap<HospitalAdministrator, HospitalAdministratorResult>()
            .ForMember(x => x.BirthDate, o => o.MapFrom(s => s.User.BirthDate))
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.Phone))
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic));
    }
}