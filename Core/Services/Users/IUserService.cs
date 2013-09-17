using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;

namespace Core.Services
{
    public interface IUserService
    {
        User GetById(int id);
        User GetByUsername(string userName);
        IQueryable<User> GetAll();
        void Create(User user);
        void Delete(User user);
        void Update(User user);
        void SignIn(String userName, bool stayLoggedIn);
        void SignOut();
        User CurrentUser { get; }
        void SetPassword(int id, string password);
    }
}
