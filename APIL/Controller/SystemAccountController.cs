using BusinessObject.Entities;
using BusinessObject.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace WebAppAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemAccountController : ControllerBase
    {
        private readonly ISystemAccountService _aS;
        public SystemAccountController(ISystemAccountService aS)
        {
            _aS = aS;
        }
        [HttpGet]
        [EnableQuery]
        public async Task<IActionResult> GetAll()
        {
            var accounts = await _aS.GetAllSystemAccountsAsync();
            return Ok(accounts);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _aS.Login(loginModel.Email, loginModel.Password);
            return user == null ? Unauthorized() : Ok(user);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(short id)
        {
            var account = await _aS.GetSystemAccountByIdAsync(id);
            return account == null ? NotFound() : Ok(account);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");
            await _aS.AddSystemAccountAsync(systemAccount);
            return CreatedAtAction(nameof(GetById), new { id = systemAccount.AccountId }, systemAccount);
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] SystemAccount systemAccount)
        {
            if (systemAccount == null)
                return BadRequest("System account cannot be null.");
            await _aS.UpdateSystemAccountAsync(systemAccount);
            return NoContent();
        }
        [HttpDelete("{id}")]
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
