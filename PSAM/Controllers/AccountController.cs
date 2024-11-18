using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Identity.Client;
using PSAM.DTOs.AccountDTOs;
using PSAM.Exceptions;
using PSAM.Models;
using PSAM.Services.IServices;
using System.ComponentModel.DataAnnotations;

namespace PSAM.Controllers
{
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;

        public AccountController(IAccountService accountService, IAuthService authService)
        {
            _accountService = accountService;
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            await _accountService.RegisterAccount(registerModel);
            return Ok();
        }

        [HttpGet("GetMe")]
        public async Task<ActionResult<AccountDTO>> GetMe()
        {
            var accountId = _authService.GetUserIdFromToken();

            if (accountId != null)
            {
                var account = await _accountService.GetPlayerById(accountId.Value);
                return Ok(account);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteMe")]
        public async Task<IActionResult> DeleteMe()
        {
            var accountId = _authService.GetUserIdFromToken();
            try
            {
                await _accountService.DeleteAccountById(accountId.Value);
                return Ok(new { message = "Account deleted successfully." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("UpdateMe")]
        public async Task<IActionResult> UpdateMe([FromBody] AccountUpdateDTO updateDTO)
        {
            try
            {
                var accountId = _authService.GetUserIdFromToken();

                await _accountService.UpdateById(accountId.Value, updateDTO);

                return Ok(new { message = "Account updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the account.", error = ex.Message });
            }
        }

        [HttpGet("GetAccountById/{accountId}")]
        public async Task<ActionResult<AccountDTO>> GetAccountById(int accountId)
        {
            try
            {
                var account = await _accountService.GetPlayerById(accountId);
                if (account == null)
                {
                    return NotFound(new { message = "Account not found." });
                }

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the account.", error = ex.Message });
            }
        }

        [HttpPost("AddTechnology")]
        public async Task<IActionResult> AddTechnology([FromBody] TechnologyModel model)
        {
            var accountID = _authService.GetUserIdFromToken();
            if (accountID != null)
            {
                try
                {
                    await _accountService.AddTechnology(accountID.Value, model.Technology);
                    return Ok();
                }
                catch (ValidationException ex)
                {
                    return StatusCode(450, ex.Message);
                }
                catch (Exception ex)
                {
                    throw new AccountExceptions($"Account technology error: {ex.Message}");
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpDelete("RemoveTechnology/{technologyId}")]
        public async Task<IActionResult> RemoveTechnology(int technologyId)
        {
            var accountID = _authService.GetUserIdFromToken();
            if (accountID != null)
            {
                try
                {
                    await _accountService.RemoveTechnology(technologyId);
                    return Ok();
                }
                catch (ValidationException ex)
                {
                    return StatusCode(450, ex.Message);
                }
                catch (Exception ex)
                {
                    throw new AccountExceptions($"Account technology error: {ex.Message}");
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpGet("GetAccountsTechnologies")]
        public async Task<IActionResult> GetAccountsTechnologies(int accountId)
        {
            try
            {
                var techs = await _accountService.GetAccountTechs(accountId);
                return Ok(techs);
            }
            catch(Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost("Subscribe/{subscribeeId}")]
        public async Task<IActionResult> Subscribe(int subscribeeId)
        {
            var accountID = _authService.GetUserIdFromToken();
            if (accountID != null)
            {
                try
                {
                    await _accountService.Subscribe(accountID.Value, subscribeeId);
                    return Ok(new { message = "Subscribed!" });
                }
                catch (Exception ex)
                {
                    throw new AccountExceptions($"Subscription error: {ex.Message}");
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpDelete("Subscribe/{subscribeeId}")]
        public async Task<IActionResult> Unsubscribe(int subscribeeId)
        {
            var accountID = _authService.GetUserIdFromToken();
            if (accountID != null)
            {
                try
                {
                    await _accountService.Unsubscribe(accountID.Value, subscribeeId);
                    return Ok(new { message = "Unsubscribed!" });
                }
                catch (Exception ex)
                {
                    throw new AccountExceptions($"Unsubscription error: {ex.Message}");
                }
            }
            else
            {
                return Unauthorized("User not authenticated.");
            }
        }

        [HttpGet("GetAccountsSubscriptions/{accountId}")]
        public async Task<IActionResult> GetAccountsSubscriptions(int accountId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var list = await _accountService.GetAccountsSubscriptions(accountId, pageNumber, pageSize);
                return Ok(list);
            }
            catch (Exception ex)
            {
                throw new AccountExceptions($"Subscription list error: {ex.Message}");
            }
        }

        [HttpGet("GetAccountsSubscribers/{accountId}")]
        public async Task<IActionResult> GetAccountsSubscribers(int accountId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var subscribers = await _accountService.GetAccountsSubscribers(accountId, pageNumber, pageSize);
                return Ok(subscribers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving subscribers: {ex.Message}");
            }
        }

        [HttpGet("SubscriberAmount/{accountId}")]
        public async Task<IActionResult> GetSubscriberAmount(int accountId)
        {
            var subscriberAmount = await _accountService.GetSubscriberAmount(accountId);
            return Ok(new { SubscriberAmount = subscriberAmount });
        }

        /* [HttpGet("GetAccounts")]
         public async Task<IActionResult> GetAccounts(int pageNumber = 1, int pageSize = 10)
         {
             var accounts = await _accountService.GetAllAccounts(pageNumber, pageSize);
             return Ok(accounts);
         }*/

        [HttpGet("GetAccounts")]
        public async Task<IActionResult> GetAccounts(
            int pageNumber = 1,
            int pageSize = 10,
            string? username = null,
            string? firstName = null,
            string? lastName = null,
            string? city = null)
        {
            try
            {
                var accounts = await _accountService.GetFilteredAccounts(pageNumber, pageSize, username, firstName, lastName, city);
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving accounts.", error = ex.Message });
            }
        }

        [HttpPost("UpdateProfileImage")]
        public async Task<IActionResult> UpdateProfileImage([FromBody] ProfileImageUpdateModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Base64Image))
            {
                return BadRequest(new { message = "Invalid image data." });
            }

            try
            {
                var accountId = _authService.GetUserIdFromToken();

                if (accountId == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                // Call the service to update the profile image.
                await _accountService.UpdateProfileImage(accountId.Value, model.Base64Image);

                return Ok(new { message = "Profile image updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while updating the profile image.", error = ex.Message });
            }
        }

        [HttpDelete("DeleteProfileImage")]
        public async Task<IActionResult> DeleteProfileImage()
        {
            try
            {
                var accountId = _authService.GetUserIdFromToken();

                if (accountId == null)
                {
                    return Unauthorized(new { message = "User not authenticated." });
                }

                await _accountService.DeleteProfileImage(accountId.Value);

                return Ok(new { message = "Profile image deleted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while deleting the profile image.", error = ex.Message });
            }
        }
    }
}
