using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Minenetred.Web.Services;

namespace Minenetred.Web.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ApiKeyController : Controller
    {
        private readonly IUsersManagementService _userManagementService;
        private readonly ILogger<ApiKeyController> _logger;

        public ApiKeyController(
            IUsersManagementService usersManagementService,
            ILogger<ApiKeyController> logger)
        {
            _userManagementService = usersManagementService;
            _logger = logger;
        }

        [Route("/UserSettings")]
        public async Task<IActionResult> KeySettingsAsync(string msj = null)
        {
            var userName = UserPrincipal.Current.EmailAddress;
            ViewBag.user = userName;
            if (!string.IsNullOrWhiteSpace(msj))
            {
                ViewBag.msj = msj;
            }
            var userKey = _userManagementService.GetUserKey(userName);
            if (userKey == null)
            {
                ViewBag.key = null;
            }
            else
            {
                var denryptionKey = _userManagementService.GetUserKey(UserPrincipal.Current.EmailAddress);
                ViewBag.key = denryptionKey;
            }
            ViewBag.Address = await _userManagementService.GetBaseAddresAsync(userName);
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> UpdateRedmineKeyAsync(string Redminekey)
        {
            try
            {
                if (string.IsNullOrEmpty(Redminekey))
                {
                    _logger.LogError(new ArgumentNullException("Key is null or empty"), "Invalid key");
                    return RedirectToAction("KeySettingsAsync");
                }
                _userManagementService.UpdateKey(Redminekey, UserPrincipal.Current.EmailAddress);
                await _userManagementService.AddRedmineIdAsync(Redminekey, UserPrincipal.Current.EmailAddress);
                return RedirectToAction("GetProjectsAsync", "Projects");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Invalid key");
                return RedirectToAction("KeySettingsAsync", new { msj = "Add a valid key" });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Bad request");
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            return BadRequest();
        }
        public async Task<IActionResult> UpdateBaseAddressAsync(string Address)
        {
            try
            {
               _userManagementService.updateBaseAddress(Address, UserPrincipal.Current.EmailAddress);

                if (!await _userManagementService.IsValidBaseAddressAsync())
                {
                    _userManagementService.updateBaseAddress("", UserPrincipal.Current.EmailAddress);
                    return RedirectToAction("KeySettingsAsync", new { msj = "Add a valid address" });
                }
                return RedirectToAction("GetProjectsAsync", "Projects");
            }
           catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid address");
                return RedirectToAction("KeySettingsAsync", new { msj = "Add a valid address" });
            }
        }
    }
}