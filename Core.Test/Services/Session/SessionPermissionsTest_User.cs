using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Models;

namespace Core.Test.Services
{
    [TestClass]
    public class SessionPermissionsTest_User
    {
        [TestMethod]
        public void Can_Edit_NotSubmitted()
        {
            var service = new SessionService(null);
            var user = new User { Id = 1, IsAdministrator = true };
            var session = new Session { Id = 1, UserSubmittedAt = null };
            var permissions = service.GetPermissionModel(session, user);

            Assert.AreEqual(permissions.CanEdit, true);
        }
    }
}
