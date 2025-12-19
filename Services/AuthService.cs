using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Teatro.Data;
using Teatro.Helpers;
using Teatro.Models.Dto.Requests;
using Teatro.Models.Dto.Responses;
using Teatro.Models.Entities;

namespace Teatro.Services
{
    public class AuthService : ServiceBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthService(
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(request.Email);
                if (existingUser != null)
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Email già registrata"
                    };
                }

                var user = new ApplicationUser
                {
                    UserName = request.Email,
                    Email = request.Email,
                    Nome = request.Nome,
                    Cognome = request.Cognome,
                    DataDiNascita = request.DataDiNascita,
                    DataCreazione = DateTime.UtcNow,
                    Active = true,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = string.Join(", ", result.Errors.Select(e => e.Description))
                    };
                }

                var roleResult = await _userManager.AddToRoleAsync(user, StringConstants.UserRole);

                if (!roleResult.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                    return new RegisterResponse
                    {
                        Success = false,
                        Message = "Errore assegnazione del ruolo"
                    };
                }

                return new RegisterResponse
                {
                    Success = true,
                    Message = "Registrazione completata ",
                    Email = user.Email
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponse
                {
                    Success = false,
                    Message = $"Errore durante la registrazione: {ex.Message}"
                };
            }
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null || !user.Active)
                {
                    return null;
                }

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                {
                    return null;
                }

                var roles = await _userManager.GetRolesAsync(user);

                var token = GenerateJwtToken(user, roles);

                return new LoginResponse
                {
                    Token = token,
                    Email = user.Email!,
                    Nome = user.Nome,
                    Cognome = user.Cognome,
                    Roles = roles.ToList()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string GenerateJwtToken(ApplicationUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, $"{user.Nome} {user.Cognome}")
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("uRcA4z5PNgJH2vLq9Xy7TfK3wB8mS6cZ"));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "server.com",
                audience: "client.com",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}