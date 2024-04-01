using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Cl.Gob.Energia.Ecocarga.Data.Models;
using Cl.Gob.Energia.Ecocarga.Web;
using Cl.Gob.Energia.Ecocarga.Web.Models;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace EcocargaWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly IOptions<AppSettings> _settings;
        private readonly ILogger _logger;
        private readonly EcocargaContext _context;

        public AccountController(IOptions<AppSettings> settings, ILogger<AccountController> logger, EcocargaContext context)
        {
            _settings = settings;
            _logger = logger;
            _context = context;
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login([Bind("User,Password")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = LDAPUtils.ValidarLogin(usuario.User, usuario.Password, _settings, _logger);

                    if (user != null)
                    {
                        user = await ValidarUuarioEcoCargaAsync(user);

                        if (user.Roles == null)
                        {
                            ModelState.AddModelError("Password", "El usuario no está registrado en el sistema");
                            return View(usuario);
                        }

                        List<Claim> claims = new List<Claim> {
                            new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(user))
                        };
                        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await AuthenticationHttpContextExtensions.SignInAsync(HttpContext, CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError("Password", "Usuario o contraseña invalida");
                    return View(usuario);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    return NotFound();
                }
            }
            return RedirectToAction(nameof(Login));
        }
        public async Task<IActionResult> Logout()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

            return RedirectToAction("Login", "Account", null);
        }

        [Route("Account/AccessDenied")]
        public IActionResult AccessDenied()
        {
            ViewBag.PreviousUrl = Request.Headers["Referer"].ToString();
            return View();
        }

        private async Task<User> ValidarUuarioEcoCargaAsync(User user)
        {
            var dataUsuario = await _context.DataUsuario
                                    .Include(d => d.Compania)
                                    .FirstOrDefaultAsync(m => m.Usuario == user.Usuario);

            if (dataUsuario != null)
            {
                List<UserRole> roles = new List<UserRole>
                        {
                            (UserRole)Enum.ToObject(typeof(UserRole), dataUsuario.PerfilId)
                        };
                user.Roles = roles;
                user.Empresa = dataUsuario.Compania != null  ? dataUsuario.Compania.Nombre : string.Empty;
            }
            return user;
        }
    }
}