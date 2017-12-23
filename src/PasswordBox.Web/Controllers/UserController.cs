using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordBox.Web.ViewModels.Search;
using PasswordBox.Web.TagHelpers.Breadcrumb;
using PasswordBox.Core.Resources;
using PasswordBox.Application.Services;
using PasswordBox.Web.ViewModels;
using PasswordBox.Core.Exceptions;
using PasswordBox.Web.Models;

namespace PasswordBox.Web.Controllers
{
    public class UserController : BaseController
    {
        public const string PARTIAL_LIST = "~/Views/User/_List.cshtml";
        public const string PARTIAL_FORM = "~/Views/User/_Form.cshtml";

        [BreadCrumb(
            TitleResourceName = nameof(CommonText.Users),
            TitleResourceType = typeof(CommonText),
            Icon = "icon-users",
            UseDefaultRouteUrl = true,
            Order = 0)]
        public IActionResult Index(UserSearchViewModel model)
        {
            var result = UserService.Instance.Search(model);

            if (IsAjaxRequest())
                return PartialView(PARTIAL_LIST, result);


            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(UserViewModel model)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                await UserService.Instance.Add(model.ToDomainModel(), model.Password,model.Roles);

                SetSuccess(result);
            }
            catch (BusinessException ex)
            {
                SetError(result, ex);
                return BadRequest(result);
            }

            return Ok(result);
        }

        public async Task<IActionResult> Edit(string id)
        {
            JsonResultObject result = new JsonResultObject();

            var user = await UserService.Instance.GetByID(id);
            var model = new UserViewModel(user);

            result.PartialViewHtml = await RenderService.RenderToStringAsync(PARTIAL_FORM, model);

            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UserViewModel model)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                var user = await UserService.Instance.GetByID(model.Id);

                user = user.Update(model.ToDomainModel());

                await UserService.Instance.UpdateAsync(user,model.Roles);

                SetSuccess(result);
            }
            catch (BusinessException ex)
            {
                SetError(result, ex);
                return BadRequest(result);
            }

            return Ok(result);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                await UserService.Instance.DeleteAsync(id);

                SetSuccess(result);
            }
            catch (BusinessException ex)
            {
                SetError(result, ex);

                return BadRequest(result);
            }

            return Ok(result);
        }

    }
}