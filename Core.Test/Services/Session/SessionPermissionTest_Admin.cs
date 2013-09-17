using System;
using System.Collections.Generic;
using Core.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model.Models;
using System.Linq;

namespace Core.Test
{
    [TestClass]
    public class SessionPermissionsTest_Admin
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

        [TestMethod]
        public void Can_Edit_Submitted()
        {
            var service = new SessionService(null);
            var user = new User { Id = 1, IsAdministrator = true };
            var session = new Session { Id = 1 };
            var permissions = service.GetPermissionModel(session, user);

            Assert.AreEqual(permissions.CanEdit, true);
        }

        [TestMethod]
        public void Can_Edit_AdminApproved()
        {
            var service = new SessionService(null);
            var user = new User { Id = 1, IsAdministrator = true };
            var session = new Session { Id = 1, UserSubmittedAt = DateTime.Now, AdminSubmittedAt = DateTime.Now, IsAccepted = true };
            var permissions = service.GetPermissionModel(session, user);

            Assert.AreEqual(permissions.CanEdit, true);
        }

        [TestMethod]
        public void Can_Edit_AdminRejected()
        {
            var service = new SessionService(null);
            var user = new User { Id = 1, IsDeleted = false, IsAdministrator = true };
            var session = new Session { Id = 1, UserSubmittedAt = DateTime.Now, AdminSubmittedAt = DateTime.Now, IsAccepted = false, RejectionReason = "I want" };
            var permissions = service.GetPermissionModel(session, user);

            Assert.AreEqual(permissions.CanEdit, true);
        }
    }
}
