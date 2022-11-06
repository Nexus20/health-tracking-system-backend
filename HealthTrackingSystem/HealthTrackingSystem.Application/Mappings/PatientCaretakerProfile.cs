using AutoMapper;
using HealthTrackingSystem.Application.Models.Requests.PatientCaretakers;
using HealthTrackingSystem.Application.Models.Results.PatientCaretakers;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Mappings;

public class PatientCaretakerProfile : Profile
{
    public PatientCaretakerProfile()
    {
        CreateMap<CreatePatientCaretakerRequest, PatientCaretaker>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<CreatePatientCaretakerRequest, User>();

        CreateMap<PatientCaretaker, PatientCaretakerResult>()
            .ForMember(x => x.BirthDate, o => o.MapFrom(s => s.User.BirthDate))
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.Phone))
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic));
    }
}