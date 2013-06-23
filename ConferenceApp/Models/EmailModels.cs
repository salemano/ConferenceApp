using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ConferenceApp.Models
{
    public class RegistrationConfirmationModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string ActivationToken { get; set; }
    }

    public class RequestResetPasswordModel
    {
        public string FullName { get; set; }
        public string ResetPasswordToken { get; set; }
        public string Email { get; set; }
    }
}