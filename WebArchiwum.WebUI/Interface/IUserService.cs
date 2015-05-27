using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using WebArchiwum.WebUI.Models;

namespace WebArchiwum.WebUI.Interface
{
    public interface IUserService
    {
        int Login(User model);
        IPrincipal GetPrincipal(int userId);
        User GetCurrentUser(IPrincipal currentPrincipal);
    
    }
}
