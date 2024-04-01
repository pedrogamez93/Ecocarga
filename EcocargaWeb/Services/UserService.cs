using Cl.Gob.Energia.Ecocarga.Web.Services.Interfaces;
using Cl.Gob.Energia.Ecocarga.Web.Utils;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Cl.Gob.Energia.Ecocarga.Web.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetUser()
        {
            Claim claim = _httpContextAccessor.HttpContext.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.UserData);
            return claim != null ? JsonConvert.DeserializeObject<User>(claim.Value) : null;
        }

        public bool IsAdmin()
        {
            var user = this.GetUser();
            bool isAdmin = false;
            if (user != null)
            {
                user.Roles.ForEach(r =>
                {
                    if (r.Equals(UserRole.Administrador))
                    {
                        isAdmin = true;
                    }
                });
            }
            return isAdmin;
        }

        public bool IsElectrolinera()
        {
            var user = this.GetUser();
            bool isElectrolinera = false;
            if (user != null) {
                user.Roles.ForEach(r =>
                {
                    if (r.Equals(UserRole.Electrolinera))
                    {
                        isElectrolinera = true;
                    }
                });
            }
            return isElectrolinera;
        }
        public bool IsSec()
        {
            var user = this.GetUser();
            bool isSec = false;
            if (user != null)
            {
                user.Roles.ForEach(r =>
                {
                    if (r.Equals(UserRole.Sec))
                    {
                        isSec = true;
                    }
                });
            }
            return isSec;
        }

        public string GetName()
        {
            var user = this.GetUser();
            string name = string.Empty;
            if (user != null)
            {
                name = string.Concat(user.Nombres, " ", user.Apellidos);
            }
            return name;
        }
        public string GetPerfil()
        {
            var user = this.GetUser();
            string perfil = string.Empty;
            if (user != null)
            {
                int rol = (int)user.Roles.First();
                perfil = Roles.GetDescription(user.Roles.First());
            }
            return perfil;
        }
    }
}
