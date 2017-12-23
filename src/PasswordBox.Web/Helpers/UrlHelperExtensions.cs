using PasswordBox.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PasswordBox.Web.Helpers
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ConfirmEmail),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ResetPasswordCallbackLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(AccountController.ResetPassword),
                controller: "Account",
                values: new { userId, code },
                protocol: scheme);
        }

        public static string ActionWithoutRouteValues(this IUrlHelper helper, string action, string[] removeRouteValues = null)
        {
            var routeValues = helper.ActionContext.RouteData.Values;
            var routeValueKeys = routeValues.Keys.Where(o => o != "controller" && o != "action").ToList();

            // Temporarily remove route values
            var oldRouteValues = new Dictionary<string, object>();

            foreach (var key in routeValueKeys)
            {
                if (removeRouteValues != null && !removeRouteValues.Contains(key))
                {
                    continue;
                }

                oldRouteValues[key] = routeValues[key];
                routeValues.Remove(key);
            }

            // Generate URL
            var url = helper.Action(action);

            // Reinsert route values
            foreach (var keyValuePair in oldRouteValues)
            {
                routeValues.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return url;
        }
    }
}
