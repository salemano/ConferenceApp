using Model;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Sessions
{
    public class SessionService:ISessionService
    {
        private readonly ConferenceContext _context;

        public SessionService(ConferenceContext context)
        {
            _context = context;
            
        }

        public IQueryable<Session> GetAll()
        {
            return _context.Sessions;
        }
        public Session Create(Session session)
        {
            _context.Sessions.Add(session);
            _context.SaveChanges();

            return session;
        }
    }
}
