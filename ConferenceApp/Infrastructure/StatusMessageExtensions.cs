using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ConferenceApp.Infrastructure
{
    public static class StatusMessageExtensions
    {
        public static ActionResult RemoveMessage(this ActionResult innerResult)
        {
            return StatusResult.Remove(innerResult);
        }

        public static ActionResult WithErrorMessage(this ActionResult innerResult, string message)
        {
            return StatusResult.Decorate(innerResult, message, StatusType.Error);
        }

        public static ActionResult WithWarningMessage(this ActionResult innerResult, string message)
        {
            return StatusResult.Decorate(innerResult, message, StatusType.Warning);
        }

        public static ActionResult WithSuccessMessage(this ActionResult innerResult, string message)
        {
            return StatusResult.Decorate(innerResult, message, StatusType.Success);
        }
    }

    public enum StatusType
    {
        Error,
        Warning,
        Success
    }

    public class StatusResult : ActionResult
    {
        public ActionResult InnerResult { get; private set; }

        public string Message { get; set; }

        public StatusType Type { get; set; }

        public static StatusResult Decorate(ActionResult innerResult, string message, StatusType type)
        {
            return new StatusResult(innerResult, message, type);
        }

        public static StatusResult Remove(ActionResult innerResult)
        {
            return new StatusResult(innerResult, null, StatusType.Success);
        }

        private StatusResult(ActionResult innerResult, string message, StatusType type)
        {
            InnerResult = innerResult;
            Message = message;
            Type = type;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            if (Message == null)
            {
                context.Controller.TempData.Remove("StatusMessage");
                context.Controller.TempData.Remove("StatusMessageType");
            }
            else
            {
                context.Controller.TempData["StatusMessage"] = Message;
                context.Controller.TempData["StatusMessageType"] = Type.ToString().ToLower();
            }
            InnerResult.ExecuteResult(context);
        }
    }
}