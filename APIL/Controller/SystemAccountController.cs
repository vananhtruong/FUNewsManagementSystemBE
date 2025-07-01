using BusinessObject.Entities;
using BusinessObject.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAppAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _aS;
        private readonly IConfiguration _configuration;

        public SystemAccountController(ISystemAccountService aS, IConfiguration configuration)
        {
            _aS = aS;
            _configuration = configuration;
        }

        // -------------------------- GET ALL --------------------------
        [HttpGet]
        [EnableQuery]
        /*[Authorize(Roles = "3")]*/ // Only Admin
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _aS.GetAllSystemAccountsAsync();
            return Ok(accounts);
        }

        // -------------------------- LOGIN --------------------------
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var defaultAdminEmail = _configuration["DefaultAccount:Email"];
            var defaultAdminPassword = _configuration["DefaultAccount:Password"];

            // Check login as default admin (from config)
            if (loginModel.Email == defaultAdminEmail && loginModel.Password == defaultAdminPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginModel.Email),
                    new Claim("role", "3"), // Role 3 = Admin
                    new Claim("AccountId", "0") // Admin not in DB
                };

                var token = GenerateJwtToken(claims);
                return Ok(new { token });
            }

            // Otherwise: check in DB
            var user = await _aS.Login(loginModel.Email, loginModel.Password);
            if (user == null)
                return Unauthorized("Invalid credentials.");

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.AccountName ?? ""),
                new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
                new Claim("role", user.AccountRole?.ToString() ?? "0"),

                new Claim("AccountId", user.AccountId.ToString())
            };

            var jwtToken = GenerateJwtToken(userClaims);
            return Ok(new { token = jwtToken });
        }

        private string GenerateJwtToken(List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // -------------------------- REGISTER --------------------------
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");

            await _aS.AddSystemAccountAsync(systemAccount);

            // Sau khi đăng ký, sinh token để auto login
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, systemAccount.AccountEmail ?? ""),
                new Claim(ClaimTypes.Role, systemAccount.AccountRole?.ToString() ?? "0"),
                new Claim("AccountId", systemAccount.AccountId.ToString())
            };

            var token = GenerateJwtToken(claims);
            return Ok(new { token });
        }


        // -------------------------- LOGIN WITH GOOGLE --------------------------
        [HttpPost("loginwithgoogle")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] LoginGoogleModel loginGoogleModel)
        {
            var user = await _aS.LoginGoogle(loginGoogleModel);
            if (user == null) return Unauthorized("Google login failed.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.AccountEmail ?? ""),
                new Claim(ClaimTypes.Role, user.AccountRole?.ToString() ?? "0"),
                new Claim("AccountId", user.AccountId.ToString())
            };

            var token = GenerateJwtToken(claims);
            return Ok(new { token });
        }


        // -------------------------- GET BY ID --------------------------
        [Authorize(Roles = "1,2,3")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _aS.GetSystemAccountByIdAsync(id);
            return account == null ? NotFound() : Ok(account);
        }

        // -------------------------- ADD --------------------------
        [HttpPost]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Add([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");

            await _aS.AddSystemAccountAsync(systemAccount);
            return CreatedAtAction(nameof(GetById), new { id = systemAccount.AccountId }, systemAccount);
        }

        // -------------------------- UPDATE --------------------------
        [HttpPut]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Update([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");

            await _aS.UpdateSystemAccountAsync(systemAccount);
            return NoContent();
        }

        // -------------------------- DELETE --------------------------
        [HttpDelete("{id}")]
        [Authorize(Roles = "3")]
        public async Task<IActionResult> Delete(short id)
        {
            try
            {
                await _aS.DeleteSystemAccountAsync(id);
                return NoContent();
            }
            catch (ArgumentException)
            {
                return NotFound("System account not found.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // -------------------------- CHECK EXISTS --------------------------
        [HttpPost("Any")]
        public async Task<IActionResult> Any([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");

            var existingAccount = await _aS.GetSystemAccountByIdAsync(systemAccount.AccountId);
            return existingAccount == null ? NotFound() : Ok(existingAccount);
        }
    }
}
