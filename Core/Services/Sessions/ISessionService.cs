using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface ISessionService
    {
        IQueryable<Session> GetAll();
        void Create(Session session);
        void Update(Session session);
        Session GetById(int n);
        IQueryable<Session> GetAllByUserId(int id);
        void UserSubmit(int id);
        void Accept(int id);
        void Reject(int id, string rejectionReason);
        void Register(int userId, int sessionId);
        void RemoveUser(int userId, int sessionId);
        void Delete(int id);
        SessionPermissionModel GetPermissionModel(Session session, User user);
    }
}
