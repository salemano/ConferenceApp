using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Email
{
    public interface IEmailService
    {
        void SendEmail(Mail emailName, object model, EmailDescription desc);
    }
}
