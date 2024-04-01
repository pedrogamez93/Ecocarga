using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Cl.Gob.Energia.Ecocarga.Web.Models;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Cl.Gob.Energia.Ecocarga.Web.Authorization;
using EcocargaWeb.Controllers;

namespace Cl.Gob.Energia.Ecocarga.Web.Controllers
{
    [AllowedRoles(new UserRole[] { UserRole.Administrador, UserRole.Electrolinera, UserRole.Sec })]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
