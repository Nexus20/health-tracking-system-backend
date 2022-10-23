﻿using AutoMapper;
using HealthTrackingSystem.Application.Models.Results.Hospitals;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Mappings;

public class HospitalProfile : Profile
{
    public HospitalProfile()
    {
        CreateMap<Hospital, HospitalResult>();
    }
}