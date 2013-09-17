using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Models;

namespace Core.Services
{
    public interface ILogger
    {
        void Log(LogLevel level, LogType type, string user, string ipAddress, string message, string details, int? objectId);
        IQueryable<EventLog> GetAll();
    }
}
