using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Security
{
    public class CustomPrincipal : IPrincipal
    {
        public IIdentity Identity { get; private set; }
        public bool IsInRole(string role)
        {
            if (roles.Any(r => role.Contains(r)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public CustomPrincipal(string Username)
        {
            this.Identity = new GenericIdentity(Username);
        }


        public int UserId { get; set; }


        public string Login { get; set; }

        public string Password { get; set; }

        public string[] roles { get; set; }
    }


    public class CustomPrincipalSerializeModel
    {

        public int UserId { get; set; }


        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Boolean IsActive { get; set; }


        public string[] roles { get; set; }
    }
}
