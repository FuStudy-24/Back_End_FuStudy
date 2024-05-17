using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FuStudy_Repository;
using FuStudy_Repository.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Tools;

public class Authentication
{
    private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;

        public Authentication(IConfiguration configuration, UnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateJwtToken(User user, float hour)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null");
            }

            var jwtKey = _configuration["Jwt:Key"];
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey ?? ""));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            long roleId = user.RoleId;
            Role role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId);

            // Log role value to console
            Console.WriteLine($"Role: {role}");

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim("Name", user.Username),
                new Claim(ClaimTypes.Role, role.RoleName),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],     
                _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(hour),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public static string GetUserIdFromHttpContext(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.ContainsKey("Authorization"))
            {
                throw new CustomException.InternalServerErrorException("Need Authorization");
            }

            string? authorizationHeader = httpContext.Request.Headers["Authorization"];

            if (string.IsNullOrWhiteSpace(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                throw new CustomException.InternalServerErrorException(
                    $"Invalid authorization header: {authorizationHeader}");
            }

            string jwtToken = authorizationHeader["Bearer ".Length..];
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwtToken);
            var idClaim = token.Claims.FirstOrDefault(claim => claim.Type == "Id");
            return idClaim?.Value ??
                   throw new CustomException.InternalServerErrorException($"Can not get userId from token");

        }
}