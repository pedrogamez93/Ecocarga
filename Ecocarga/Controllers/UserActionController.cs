using Ecocarga.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

public class UserActionController : Controller
{
    private readonly ApplicationDbContext _context;

    public UserActionController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var actions = _context.UserActions.OrderByDescending(a => a.ActionTime).ToList();
        return View(actions);
    }
}
