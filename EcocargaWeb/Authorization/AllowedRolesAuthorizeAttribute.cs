using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;
using System.Security.Claims;
using Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces;

namespace Cl.Gob.Energia.Ecocarga.Web.Authorization
{
    public class AllowedRolesAttribute : TypeFilterAttribute
    {
        public AllowedRolesAttribute(UserRole rol) : base(typeof(AllowedRolesFilter))
        {
            Arguments = new object[]
            {
                new AllowedRolesParameters()
                {
                    Rol = rol
                }
            };
        }
        public AllowedRolesAttribute(UserRole[] roles) : base(typeof(AllowedRolesFilter))
        {
            Arguments = new object[]
            {
                new AllowedRolesParameters()
                {
                    Roles = roles
                }
            };
        }
        public class AllowedRolesFilter : IAuthorizationFilter
        {
            private readonly AllowedRolesParameters _filterParameters;
            protected readonly IAuthorizationLogicService _authorizationLogicService;
            protected readonly IUserService _userService;
            public AllowedRolesFilter(AllowedRolesParameters filterParameters, IAuthorizationLogicService authorizationLogicService, IUserService userService)
            {
                _filterParameters = filterParameters;
                _authorizationLogicService = authorizationLogicService;
                _userService = userService;
            }

            public virtual void OnAuthorization(AuthorizationFilterContext context)
            {
                var hasClaim = context.HttpContext.User.Claims.Any();
                if (!hasClaim)
                {
                    context.Result = new RedirectResult("/Account");
                }
                else
                { 
                    User user = _userService.GetUser();

                    if (!_authorizationLogicService.AllowedRoles(user, _filterParameters))
                    {
                        context.Result = new ForbidResult();
                    }
                }
            }
        }
    }
}
