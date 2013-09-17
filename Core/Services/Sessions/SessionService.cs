using Model;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class SessionService : ISessionService
    {
        private readonly ConferenceContext _context;

        public SessionService(ConferenceContext context)
        {
            _context = context;

        }

        public IQueryable<Session> GetAllByUserId(int id)
        {
            return from p in GetAll()
                   where p.UserId == id
                   select p;
        }

        public IQueryable<Session> GetAll()
        {
            return from p in _context.Sessions
                       .Include("User")
                       .Include("Users")
                       .Include("Users.User")
                   where !p.IsDeleted
                   select p;
        }

        public void Create(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();
        }

        public void Update(Session session)
        {
            _context.SaveChanges();
        }

        public Session GetById(int id)
        {
            return GetAll().FirstOrDefault(p => p.Id == id);
        }

        public void UserSubmit(int id)
        {
            var session = GetById(id);
            session.UserSubmittedAt = DateTime.Now;
            Update(session);
        }

        public void Accept(int id)
        {
            var session = GetById(id);
            session.AdminSubmittedAt = DateTime.Now;
            session.IsAccepted = true;

            Update(session);
        }

        public void Reject(int id, string rejectionReason)
        {
            var session = GetById(id);
            session.AdminSubmittedAt = DateTime.Now;
            session.IsAccepted = false;
            session.RejectionReason = rejectionReason;

            Update(session);
        }

        public void Register(int userId, int sessionId)
        {
            var session = GetById(sessionId);

            session.Users.Add(new UsersInSessions
            {
                RegistrationDate = DateTime.Now,
                SessionId = sessionId,
                UserId = userId
            });

            Update(session);
        }

        public void RemoveUser(int userId, int sessionId)
        {
            var userInSession = _context.UsersInSessions.FirstOrDefault(u => u.UserId == userId && u.SessionId == sessionId);

            _context.UsersInSessions.Remove(userInSession);
            _context.SaveChanges();
        }

        public SessionPermissionModel GetPermissionModel(Session session, User user)
        {
            return new SessionPermissionModel
            {
                CanEdit = CanEdit(session, user),
                CanView = CanView(session, user)
            };
        }

        private bool CanEdit(Session session, User user)
        {
            if (user.IsAdministrator)
                return true;

            if (session.UserSubmittedAt != null)
                return false;

            if (session.AdminSubmittedAt != null)
                return false;

            if (session.End < DateTime.Now)
                return false;

            return true;
        }

        private bool CanView(Session session, User user)
        {
            if (user.IsAdministrator)
                return true;

            if (session.UserId == user.Id)
                return true;

            return false;
        }

        public void Delete(int id)
        {
            var session = _context.Sessions.First(s => s.Id == id);
            if (session == null)
                return;

            session.IsDeleted = true;
            _context.SaveChanges();
        }
    }
}
