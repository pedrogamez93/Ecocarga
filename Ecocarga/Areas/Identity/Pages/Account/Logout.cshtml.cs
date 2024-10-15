// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Ecocarga.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Ecocarga.Services;

namespace Ecocarga.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly UserActionService _userActionService;

        public LogoutModel(SignInManager<ApplicationUser> signInManager, ILogger<LogoutModel> logger, UserActionService userActionService)
        {
            _signInManager = signInManager;
            _logger = logger;
            _userActionService = userActionService;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            // Obtener el usuario actual
            var user = await _signInManager.UserManager.GetUserAsync(User);

            // Registrar la acción de cierre de sesión si el usuario está autenticado
            if (user != null)
            {
                await _userActionService.LogActionAsync("Cierre de sesión", $"El usuario {user.Email} ha cerrado sesión.");
            }

            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
