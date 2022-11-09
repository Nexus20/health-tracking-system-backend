using HealthTrackingSystem.Application.Models.Requests.Auth;
using HealthTrackingSystem.Application.Models.Results.Auth;

namespace HealthTrackingSystem.Application.Interfaces.Services;

public interface ISignInService {

    Task<LoginResult> SignInAsync(LoginRequest request);
}