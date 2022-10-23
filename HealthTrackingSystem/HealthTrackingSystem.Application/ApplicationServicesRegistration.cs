﻿using System.Reflection;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HealthTrackingSystem.Application;

public static class ApplicationServicesRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddScoped<IHospitalService, HospitalService>();

        return services;
    }
}