using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IEmailService
    {
        void SendMessage(EmailDescription desc);
        void WriteMessageToFile(EmailDescription desc);
    }
}
