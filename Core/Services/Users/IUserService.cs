using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Models;

namespace Core.Services
{
    public interface IUserService
    {
        User Create(User user);
    }
}
