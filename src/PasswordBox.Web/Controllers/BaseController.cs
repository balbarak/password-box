using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordBox.Web.AppServices;
using PasswordBox.Web.Models;
using Newtonsoft.Json;
using PasswordBox.Web.Helpers;
using PasswordBox.Core.Resources;
using PasswordBox.Core.Exceptions;
using System.Text;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading;

namespace PasswordBox.Web.Controllers
{
    public class BaseController : Controller
    {
        protected IViewRenderService RenderService
            => HttpContext.RequestServices.GetService(typeof(IViewRenderService)) as IViewRenderService;

        protected void SetSuccess(bool isAutoHide = true)
        {
            var alert = new Alert(MessageText.OperationSuccess, Alert.Type.Success, isAutoHide: isAutoHide);

            var alertJson = JsonConvert.SerializeObject(alert);

            TempData[WebConstants.ALERT] = alertJson;
        }

        protected void SetSuccess(JsonResultObject result, bool isAutoHide = true)
        {
            var alert = new Alert(MessageText.OperationSuccess, Alert.Type.Success, isAutoHide: isAutoHide);
            result.Alert = alert;
        }

        protected void SetError(Exception ex = null, bool isAutoHide = false)
        {
            var msg = MessageText.OperationFailed;

            if (ex != null)
            {
                msg = GetExceptionError(ex);
            }

            var alert = new Alert(msg, Alert.Type.Error, isAutoHide: isAutoHide);
            var alertJson = JsonConvert.SerializeObject(alert);
            TempData[WebConstants.ALERT] = alertJson;
        }

        protected void SetError(JsonResultObject result, Exception ex = null, bool isAutoHide = false)
        {
            var msg = MessageText.OperationFailed;

            if (ex != null)
            {
                msg = GetExceptionError(ex);
            }

            var alert = new Alert(msg, Alert.Type.Error, isAutoHide: isAutoHide);

            result.Alert = alert;
            result.Success = false;
        }

        protected ActionResult SimpleAjaxAction(Action action, bool setSuccessMessage = true)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                action.Invoke();

                if (setSuccessMessage)
                    SetSuccess(result);
            }
            catch (BusinessException ex)
            {
                SetError(result, ex);
            }

            return Ok(result);
        }
        
        protected string FormatErrorMessage(List<string> errors)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(MessageText.PleaseFixTheFollowingErrors);

            sb.Append("<ul>");

            foreach (var item in errors)
                sb.AppendLine($"<li>{item}</li>");

            sb.Append("</ul>");

            return sb.ToString();
        }

        protected bool IsAjaxRequest()
        {
            if (Request == null)
                throw new ArgumentNullException(nameof(Request));

            if (Request.Headers != null)
                return Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return false;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Thread.CurrentPrincipal = context.HttpContext.User;

            base.OnActionExecuting(context);
        }

        private string GetExceptionError(Exception ex)
        {
            if (ex == null)
                return "";

            if (ex is EntityValidationException entityValidationException)
            {
                return FormatErrorMessage(entityValidationException.Errors);
            }

            if (ex is BusinessException businessValidationMessage && businessValidationMessage.Errors?.Count > 0)
            {
                return FormatErrorMessage(businessValidationMessage.Errors);
            }

            return ex.Message;
        }
    }
}