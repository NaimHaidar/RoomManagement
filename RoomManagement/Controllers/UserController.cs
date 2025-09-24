using Microsoft.AspNetCore.Authorization; 
using Microsoft.AspNetCore.Identity; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens; 
using RoomManagement.Repository;
using RoomManagement.Repository.Models;
using System.IdentityModel.Tokens.Jwt; 
using System.Security.Claims; 
using System.Security.Cryptography; 
using System.Text;

namespace RoomManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly RoomManagementDBContext _context; 

        public UserController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            RoomManagementDBContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                expires: DateTime.Now.AddMinutes(30),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64]; 
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false, 
                ValidateIssuer = false,   
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"])),
                ValidateLifetime = false 
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;

            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }


        [HttpGet(Name = "GetUser")]
        [Authorize(Roles = "Admin")] 
      public async Task<IEnumerable<UserDto>> Get()
        {
            var users = _userManager.Users.ToList();
            var userDtos = new List<UserDto>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                userDtos.Add(new UserDto(u, roles));
            }
            return userDtos;
        }
        [HttpGet("count", Name = "GetNbrUser")]
        [Authorize]
        public async Task<int> GetNbr()
        {
            return await _userManager.Users.CountAsync();
        }

        [HttpGet("{id}")]
        [Authorize] 
        public async Task<ActionResult<UserDto>> GetById(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var roles = await _userManager.GetRolesAsync(user);
            return new UserDto(user,roles);
        }

        [HttpPost(Name = "setUser")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Set([FromBody] NewUserDto newUserDto)
        {
            var user = new User(newUserDto.Name, newUserDto.Email);

            var result = await _userManager.CreateAsync(user, newUserDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            if (String.IsNullOrWhiteSpace(newUserDto.Role)||newUserDto.Role=="string") { 
                newUserDto.Role = "Gest";
            }
            await _userManager.AddToRoleAsync(user, newUserDto.Role); 
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserDto(user,roles));
        }

        [HttpPut( Name = "UpdateUser")]
        [Authorize(Roles = "Admin,Employee")]
        public async Task<IActionResult> Update(string id, [FromBody] NewUserDto updateUserDto)
        {
            if (updateUserDto == null)
                return BadRequest("User data is required.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });

            user.Name = updateUserDto.Name;
            user.Email = updateUserDto.Email;
            user.UserName = updateUserDto.Email;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            if (!string.IsNullOrEmpty(updateUserDto.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordChangeResult = await _userManager.ResetPasswordAsync(user, token, updateUserDto.Password);
                if (!passwordChangeResult.Succeeded)
                    return BadRequest(passwordChangeResult.Errors);
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!string.IsNullOrWhiteSpace(updateUserDto.Role))
            {
                if (!currentRoles.Contains(updateUserDto.Role))
                {
                    if (currentRoles.Any())
                        await _userManager.RemoveFromRolesAsync(user, currentRoles);
                    await _userManager.AddToRoleAsync(user, updateUserDto.Role);
                }
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new UserDto(user, roles));
        }


        [HttpDelete(Name = "DeleteUser")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> Delete(string id) 
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(new { message = $"User with ID {id} has been deleted." });
        }


        [HttpPost("login")]
        [AllowAnonymous] 
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id) 
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10); 
                await _userManager.UpdateAsync(user);
               String userId= user.Id;
                return Ok(new
                {
                    accessToken = new JwtSecurityTokenHandler().WriteToken(token),
                    refreshToken = refreshToken,
                    expiration = token.ValidTo,
                    use = userId


                });
            }
            return Unauthorized(new { message = "Invalid credentials." });
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenRequest)
        {
            if (tokenRequest == null || string.IsNullOrEmpty(tokenRequest.AccessToken) || string.IsNullOrEmpty(tokenRequest.RefreshToken))
            {
                return BadRequest("Invalid client request: tokenRequest is null or missing tokens.");
            }

            ClaimsPrincipal? principal = null;
            try
            {
                principal = GetPrincipalFromExpiredToken(tokenRequest.AccessToken);
            }
            catch (SecurityTokenException ex)
            {
                return BadRequest($"Invalid access token: {ex.Message}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }

            if (principal == null)
            {
                return BadRequest("Invalid access token.");
            }

            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
            {
                return BadRequest("Invalid access token: User ID claim not found.");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null || user.RefreshToken != tokenRequest.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid refresh token or refresh token expired.");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(10);
            await _userManager.UpdateAsync(user);

            return Ok(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

       
        [HttpPost("create-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    return Ok($"Role '{roleName}' created successfully.");
                }
                return BadRequest(result.Errors);
            }
            return BadRequest($"Role '{roleName}' already exists.");
        }

        [HttpPost("assign-role")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRole(string userId, string roleName)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            var roleExist = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                return BadRequest("Role does not exist.");
            }

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (result.Succeeded)
            {
                return Ok($"User '{user.UserName}' assigned to role '{roleName}' successfully.");
            }
            return BadRequest(result.Errors);
        }
    }
}
