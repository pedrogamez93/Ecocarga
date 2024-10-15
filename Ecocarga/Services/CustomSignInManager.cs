using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Ecocarga.Models;
using Ecocarga.Services;
using Microsoft.AspNetCore.Authentication;

public class CustomSignInManager<TUser> : SignInManager<TUser> where TUser : class
{
    private readonly UserActionService _userActionService;
    private readonly UserManager<TUser> _userManager;

    public CustomSignInManager(UserManager<TUser> userManager,
                               IHttpContextAccessor contextAccessor,
                               IUserClaimsPrincipalFactory<TUser> claimsFactory,
                               IOptions<IdentityOptions> optionsAccessor,
                               ILogger<SignInManager<TUser>> logger,
                               IAuthenticationSchemeProvider schemes,
                               IUserConfirmation<TUser> confirmation,
                               UserActionService userActionService)
        : base(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes, confirmation)
    {
        _userActionService = userActionService;
        _userManager = userManager;
    }

    public override async Task SignInAsync(TUser user, bool isPersistent, string authenticationMethod = null)
    {
        await base.SignInAsync(user, isPersistent, authenticationMethod);

        // Registra el inicio de sesión
        if (user is ApplicationUser appUser)
        {
            await _userActionService.LogActionAsync("Inicio de sesión", $"El usuario {appUser.Email} ha iniciado sesión.");
        }
    }

    public override async Task SignOutAsync()
    {
        var user = await _userManager.GetUserAsync(Context.User);

        if (user is ApplicationUser appUser)
        {
            await _userActionService.LogActionAsync("Cierre de sesión", $"El usuario {appUser.Email} ha cerrado sesión.");
        }

        await base.SignOutAsync();
    }
}
