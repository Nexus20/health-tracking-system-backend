using AutoMapper;
using HealthTrackingSystem.Application.Exceptions;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Application.Interfaces.Services;
using HealthTrackingSystem.Application.Models.Results.Users;
using HealthTrackingSystem.Domain.Entities;

namespace HealthTrackingSystem.Application.Services;

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IMapper _mapper;

    public UserService(IRepository<User> userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<ProfileResult> GetOwnProfileAsync(string userId)
    {
        var source = await _userRepository.GetByIdAsync(userId);

        if (source == null)
            throw new NotFoundException(nameof(User), userId);
        
        var result = _mapper.Map<User, ProfileResult>(source);
        return result;
    }
}