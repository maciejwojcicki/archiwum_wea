using Domain.Core;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using WebArchiwum.WebUI.Exceptions;
using WebArchiwum.WebUI.Expections;
using WebArchiwum.WebUI.Interface;
using WebArchiwum.WebUI.Models;
using WebArchiwum.WebUI.Utils;

namespace WebArchiwum.WebUI.Service
{
    public class UserService : IUserService
    {

        private MyDbContext context;
        public UserService()
        {

            this.context = new MyDbContext();
            this.context.Configuration.LazyLoadingEnabled = false;
            this.context.Database.Log = (text) =>
            {
                Console.WriteLine(text);
            };
        }
        public int Login(User model)
        {
            var hash = UserUtils.PasswordHash(model.Password);

            var user = context.Users
                .Where(w => w.Login == model.Login && w.Password == hash)
                //.Include(i => i.Permissions)               
                .SingleOrDefault();

            if (user == null)
            {
                throw new InvalidLoginPasswordException();
            }

            //if (user.Permissions.Select(p => p.Value)
            //    .Contains(Permission.PermissionList.CanLogin) == false)
            //{
            //    throw new PermissionException(Permission.PermissionList.CanLogin);
            //}

            int userId = user.UserId;

            return userId;
        }
        public IPrincipal GetPrincipal(int userId)
        {
            var user = context.Set<User>()
                //.AsQueryable()
                .Where(w => w.UserId == userId)
                //.Include(i => i.Permissions)
                .SingleOrDefault();

            if (user == null)
            {
                throw new NotFoundException();
            }

            //string[] roles = user.Permissions.OfType<Permission>().Select(p => p.Name.ToString()).ToArray();
            string[] roles = { "test" };
            return new GenericPrincipal(
                new GenericIdentity(user.UserId.ToString()),
                roles);
        }

        public User GetCurrentUser(IPrincipal currentPrincipal)
        {
            User user = null;
            int userId = 0;
            if (currentPrincipal != null && currentPrincipal.Identity != null && int.TryParse(currentPrincipal.Identity.Name, out userId))
            {
                user = context.Set<User>().Where(i => i.UserId == userId).Single();
            }
            return user;
        }
    }
}