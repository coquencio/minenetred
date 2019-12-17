﻿using System;
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
            try
            {
                if (string.IsNullOrEmpty(address))
                {
                    throw new ArgumentNullException(nameof(address));
                }
                _usersManagementService.updateBaseAddress(address, UserPrincipal.Current.EmailAddress);
                if (!await _usersManagementService.IsValidBaseAddressAsync())
                {
                    _usersManagementService.updateBaseAddress("", UserPrincipal.Current.EmailAddress);
                    throw new InvalidCastException("Invalid base address");
                }
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "Missing data");
            }
            catch (InvalidCastException)
            {
                _logger.LogError(new InvalidCastException("Invalid base address"), "Invalid Base address");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            return BadRequest();
        }
        [Route("settings/key/{Redminekey}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpPost]
        public async Task<IActionResult> UpdateRedmineKeyAsync([FromRoute] string Redminekey)
        {
            try
            {
                if (string.IsNullOrEmpty(Redminekey))
                {
                    throw new ArgumentNullException((nameof(Redminekey)));
                }
                _usersManagementService.UpdateKey(Redminekey, UserPrincipal.Current.EmailAddress);
                await _usersManagementService.AddRedmineIdAsync(Redminekey, UserPrincipal.Current.EmailAddress);
                return Ok();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Missing key");
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "Invalid key");
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Bad request");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Unhandled exception");
            }
            return BadRequest();
        }
    }
}