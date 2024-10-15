using Ecocarga.Data;
using Ecocarga.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ecocarga.Services
{
    public class UserActionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserActionService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LogActionAsync(string actionType, string actionDescription)
        {
            var userId = _httpContextAccessor.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                var userAction = new UserAction
                {
                    UserId = userId,
                    ActionType = actionType,
                    ActionDescription = actionDescription,
                    ActionTime = DateTime.Now
                };

                _context.UserActions.Add(userAction);
                await _context.SaveChangesAsync();
            }
        }
    }
}
