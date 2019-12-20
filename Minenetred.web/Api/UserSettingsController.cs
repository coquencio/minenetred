using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minenetred.Web.Services;

namespace Minenetred.Web.Api
{
    [ApiController]
    [Authorize]
    public class UserSettingsController : ControllerBase
    {
        private readonly IUsersManagementService _usersManagementService;
        private readonly ILogger<UserSettingsController> _logger;
        public UserSettingsController(
            IUsersManagementService usersManagementService,
            ILogger<UserSettingsController> logger)
        {
            _usersManagementService = usersManagementService;
            _logger = logger;
        }
        [Route("settings/baseAddress")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> UpdateBaseAddressAsync(string address)
        {
            string message = "";
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    message = "Missing address";
                    throw new ArgumentNullException(nameof(address));
                }
                _usersManagementService.updateBaseAddress(address, UserPrincipal.Current.EmailAddress);
                if (!await _usersManagementService.IsValidBaseAddressAsync())
                {
                    _usersManagementService.updateBaseAddress("", UserPrincipal.Current.EmailAddress);
                    throw new InvalidCastException("Invalid base address");
                }
                message = "Base address successfully updated";
                return Ok(message);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, message);
            }
            catch (InvalidCastException ex)
            {
                message = "Invalid base address";
                _logger.LogError(ex, message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            return BadRequest(message);
        }
        [Route("settings/key/{Redminekey}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> UpdateRedmineKeyAsync([FromRoute] string Redminekey)
        {
            string message = "";
            try
            {
                if (string.IsNullOrEmpty(Redminekey))
                {
                    message = "Missing key";
                    throw new ArgumentNullException((nameof(Redminekey)));
                }
                _usersManagementService.UpdateKey(Redminekey, UserPrincipal.Current.EmailAddress);
                await _usersManagementService.AddRedmineIdAsync(Redminekey, UserPrincipal.Current.EmailAddress);
                message = "Redmine key successfully updated";
                return Ok(message);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, message);
            }
            catch (UnauthorizedAccessException ex)
            {
                message = "Invalid key";
                _logger.LogError(ex, message);
            }
            catch (HttpRequestException ex)
            {
                message = "Bad request";
                _logger.LogError(ex, message);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            _usersManagementService.UpdateKey("", UserPrincipal.Current.EmailAddress);
            return BadRequest(message);
        }
        [Route("settings/baseAddress")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<IActionResult> GetBaseAddressAsync()
        {
            var email = UserPrincipal.Current.EmailAddress;
            var address = await _usersManagementService.GetBaseAddresAsync(email);
            if (string.IsNullOrEmpty(address))
            {
                return NotFound();
            }
            return Ok(new { address = address });
        }
        [Route("settings/key")]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpGet]
        public async Task<IActionResult> GetRedmineKeyAsync()
        {
            var email = UserPrincipal.Current.EmailAddress;
            var address = await _usersManagementService.GetBaseAddresAsync(email);
            if (string.IsNullOrEmpty(address))
            {
                return BadRequest();
            }
            var Key = _usersManagementService.GetUserKey(email);
            if (string.IsNullOrEmpty(Key))
            {
                return NotFound();
            }
            return Ok( new { key = Key });
        }
    }
}