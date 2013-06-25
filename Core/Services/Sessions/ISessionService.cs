using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Sessions
{
    public interface ISessionService
    {
        Session Create(Session session);
        IQueryable<Session> GetAll();
    }
}
