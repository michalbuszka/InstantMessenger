using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace InstantMessenger.Application.Services;

public sealed class JwtService(IConfiguration configuration)
{
    public string GenerateToken(Guid id)
    {
        var issuer = configuration["JwtConfig:Issuer"]!;
        var audience = configuration["JwtConfig:Audience"];
        var key = configuration["JwtConfig:Key"];
        var tokenValidityMins = configuration["JwtConfig:TokenValidityMins"];
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(Convert.ToDouble(tokenValidityMins));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, id.ToString()) }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public string GenerateRefreshToken(Guid id)
    {
        var issuer = configuration["JwtConfig:Issuer"]!;
        var audience = configuration["JwtConfig:Audience"];
        var key = configuration["JwtConfig:Key"];
        var refreshTokenValidityDays = configuration["JwtConfig:RefreshTokenValidityDays"];
        var tokenExpiryTimeStamp = DateTime.UtcNow.AddDays(Convert.ToDouble(14 * Convert.ToDouble(refreshTokenValidityDays)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, id.ToString()) }),
            Expires = tokenExpiryTimeStamp,
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials
            (new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}