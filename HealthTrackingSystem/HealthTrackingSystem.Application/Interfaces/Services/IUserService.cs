using HealthTrackingSystem.Application.Models.Results.Users;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface IUserService
{
    Task<ProfileResult> GetOwnProfileAsync(string userId);
}