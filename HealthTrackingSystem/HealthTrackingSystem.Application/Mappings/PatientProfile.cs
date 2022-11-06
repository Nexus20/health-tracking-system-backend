using AutoMapper;
using HealthTrackingSystem.Application.Models.Requests.Patients;
using HealthTrackingSystem.Application.Models.Results.Patients;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Mappings;

public class PatientProfile : Profile
{
    public PatientProfile()
    {
        CreateMap<CreatePatientRequest, Patient>()
            .ForMember(x => x.User, o => o.MapFrom(s => s));
        CreateMap<CreatePatientRequest, User>();

        CreateMap<Patient, PatientResult>()
            .ForMember(x => x.Email, o => o.MapFrom(s => s.User.Email))
            .ForMember(x => x.Phone, o => o.MapFrom(s => s.User.Phone))
            .ForMember(x => x.FirstName, o => o.MapFrom(s => s.User.FirstName))
            .ForMember(x => x.LastName, o => o.MapFrom(s => s.User.LastName))
            .ForMember(x => x.Patronymic, o => o.MapFrom(s => s.User.Patronymic));
    }
}