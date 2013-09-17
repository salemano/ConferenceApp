using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.Models;

namespace Core.Services
{
    public class Logger : ILogger
    {
        private readonly ConferenceContext _context;

        public Logger(ConferenceContext context)
        {
            _context = context;
        }
        public IQueryable<EventLog> GetAll()
        {
            return _context.EventLogs;
        }

        public void Log(LogLevel level, LogType type, string user, string ipAddress, string message, string details, int? objectId)
        {
            var log = new EventLog();
            log.UserName = user;

            log.ObjectId = objectId;
            log.CreateDate = DateTime.Now;
            log.LogLevel = (int)level;
            log.LogType = (int)type;
            log.IPAddress = ipAddress;
            log.Message = message;
            log.Detail = details;

            _context.EventLogs.Add(log);
            _context.SaveChanges();
        }
    }

    public enum LogLevel
    {
        INFO = 1,
        DEBUG = 2,
        ERROR = 3
    }

    public enum LogType
    {
        USER_ACTION = 1,
        ADMIN_ACTION = 2,
        SYSTEM = 3,
        PROGRAMMER_NOTE = 4
    }
}
