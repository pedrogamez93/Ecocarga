using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecocarga.Models;
using Ecocarga.Services;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserActionService _userActionService;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, UserActionService userActionService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userActionService = userActionService;
    }

    // Vista de inicio de sesión
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (ModelState.IsValid)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    // Registrar el inicio de sesión
                    await _userActionService.LogActionAsync("Inicio de sesión", $"El usuario {user.Email} ha iniciado sesión.");
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                return View("Lockout");
            }

            ModelState.AddModelError(string.Empty, "Intento de inicio de sesión inválido.");
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        var user = await _userManager.GetUserAsync(User);

        if (user != null)
        {
            // Registrar el cierre de sesión
            await _userActionService.LogActionAsync("Cierre de sesión", $"El usuario {user.Email} ha cerrado sesión.");
        }

        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }


}
