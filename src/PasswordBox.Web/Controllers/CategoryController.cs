using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordBox.Web.ViewModels.Search;
using PasswordBox.Application.Services;
using PasswordBox.Domain.Models;
using PasswordBox.Core.Exceptions;
using PasswordBox.Web.Models;
using PasswordBox.Web.TagHelpers.Breadcrumb;
using PasswordBox.Core.Resources;

namespace PasswordBox.Web.Controllers
{

    [BreadCrumb(
          TitleResourceName = nameof(CommonText.Categories),
          TitleResourceType = typeof(CommonText),
          Order = 0,
          UseDefaultRouteUrl = true,
          Icon = "icon-list")]
    public class CategoryController : BaseController
    {
        private const string PARTIAL_LIST = "~/Views/Category/_List.cshtml";

        private const string PARTIAL_FORM = "~/Views/Category/_Form.cshtml";

        public IActionResult Index(CategorySearchViewModel model)
        {
            var result = CategoryService.Instance.Search(model.ToSearchModel());

            if (IsAjaxRequest())
                return PartialView(PARTIAL_LIST, result);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Category category)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                CategoryService.Instance.Add(category);

                SetSuccess(result);
            }
            catch (BusinessException ex)
            {
                SetError(result, ex);

                return BadRequest(result);
            }

            return Ok(result);
        }

        public async Task<IActionResult> Edit(int id)
        {
            JsonResultObject result = new JsonResultObject();

            var model = CategoryService.Instance.GetById(id);

            result.PartialViewHtml = await RenderService.RenderToStringAsync(PARTIAL_FORM, model);

            return Ok(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Category category)
        {
            JsonResultObject result = new JsonResultObject();
            try
            {
                var original = CategoryService.Instance.GetById(category.Id);

                original = original.Update(category);

                CategoryService.Instance.Update(original);

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
        public IActionResult Delete(int id)
        {
            return SimpleAjaxAction(() =>
            {
                CategoryService.Instance.Delete(id);
            });
        }
    }
}