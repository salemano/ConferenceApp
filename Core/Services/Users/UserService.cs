using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Model;
using Model.Models;

namespace Core.Services
{
    public class UserService: IUserService
    {
        private readonly ConferenceContext _context;
        private User _currentUser;
        public UserService(ConferenceContext context)
        {
            _context = context;
        }

        public IQueryable<User> GetAll()
        {
            return _context.Users;
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByUsername(string userName)
        {
            return _context.Users.FirstOrDefault(u => u.Email == userName);
        }

        public User CurrentUser
        {
            get
            {
                if (_currentUser == null)
                    _currentUser = GetAll().FirstOrDefault(u => u.Email == HttpContext.Current.User.Identity.Name);

                return _currentUser;
            }
        }

        public void Update(User user)
        {
            _context.SaveChanges();
            _currentUser = null;
        }

        public void SignIn(String userName, bool stayLoggedIn)
        {
            FormsAuthentication.SetAuthCookie(userName, stayLoggedIn);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }

        public User Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }
    }
}
