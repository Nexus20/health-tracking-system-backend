using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HealthTrackingSystem.Application.Authorization;
using HealthTrackingSystem.Application.Interfaces.Persistent;
using HealthTrackingSystem.Domain.Entities;
using HealthTrackingSystem.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HealthTrackingSystem.Infrastructure.Auth;

public class JwtHandler {
    
    private readonly IConfigurationSection _jwtSettings;
    private readonly UserManager<AppUser> _userManager;
    private readonly IRepository<User> _userRepository;

    public JwtHandler(IConfiguration configuration, UserManager<AppUser> userManager, IRepository<User> userRepository) {
        _userManager = userManager;
        _userRepository = userRepository;
        _jwtSettings = configuration.GetSection("JwtSettings");
    }
    
    public SigningCredentials GetSigningCredentials() {
        
        var key = Encoding.UTF8.GetBytes(_jwtSettings["securityKey"]);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
    
    public async Task<List<Claim>> GetClaimsAsync(AppUser user) {
        
        var claims = new List<Claim> {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };
        
        var roles = await _userManager.GetRolesAsync(user);
        
        foreach (var role in roles) {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        
        var domainUser = await _userRepository.GetByIdAsync(user.Id);
        
        if(domainUser?.Doctor != null)
            claims.Add(new Claim(CustomClaimTypes.DoctorId, domainUser.Doctor.Id));
        
        if(domainUser?.Patient != null)
            claims.Add(new Claim(CustomClaimTypes.PatientId, domainUser.Patient.Id));
        
        if(domainUser?.PatientCaretaker != null)
            claims.Add(new Claim(CustomClaimTypes.PatientCaretakerId, domainUser.PatientCaretaker.Id));
        
        if(domainUser?.HospitalAdministrator != null)
            claims.Add(new Claim(CustomClaimTypes.HospitalAdministratorId, domainUser.HospitalAdministrator.Id));
        
        return claims;
    }
    
    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims) {
        
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["validIssuer"],
            audience: _jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["expiryInMinutes"])),
            signingCredentials: signingCredentials);
        
        return tokenOptions;
    }
}