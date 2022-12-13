using AutoMapper;
using HealthTrackingSystem.Application.Models.Requests.Doctors;
using HealthTrackingSystem.Application.Models.Results.Doctors;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Mappings;

public class DoctorProfile : Profile
{
    public DoctorProfile()
    {
        CreateMap<CreateDoctorRequest, Doctor>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<CreateDoctorRequest, User>();
        
        CreateMap<UpdateDoctorRequest, Doctor>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<UpdateDoctorRequest, User>();

        CreateMap<Doctor, DoctorResult>()
            .ForMember(x => x.BirthDate, o => o.MapFrom(s => s.User.BirthDate))
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.Phone))
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic));
    }
}