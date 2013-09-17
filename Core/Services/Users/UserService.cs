using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using Core.Security;
using Model;
using Model.Models;

namespace Core.Services
{
    public class UserService: IUserService
    {
        private readonly ConferenceContext _context;
        private ICryptographyService _cryptographyService;
        private User _currentUser;

        public UserService(ConferenceContext context, ICryptographyService cryptographyService)
        {
            _context = context;
            _cryptographyService = cryptographyService;
        }
        public void Delete(User user)
        {
            user.IsDeleted = true;
            _context.SaveChanges();
        }
        public IQueryable<User> GetAll()
        {
            return from p in _context.Users where p.IsDeleted == false select p;
        }

        public User GetById(int id)
        {
            return GetAll().FirstOrDefault(u => u.Id == id);
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

        public void Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void SetPassword(int id, string password)
        {
            var user = GetById(id);
            var salt = _cryptographyService.GetRandomString(50, false, false);
            user.Password = _cryptographyService.EncryptPassword(password + salt);

            _context.SaveChanges();
        }
    }
}
