using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordBox.Application.Services;
using PasswordBox.Core.Exceptions;
using PasswordBox.Domain.Models;
using PasswordBox.Web.Models;
using PasswordBox.Web.ViewModels.Search;

namespace PasswordBox.Web.Controllers
{
    public class VaultController : BaseController
    {
        private const string PARTIAL_LIST = "~/Views/Vault/_List.cshtml";
        private const string PARTIAL_FORM = "~/Views/Vault/_Form.cshtml";
        private const string PARTIAL_DISPLAY = "~/Views/Vault/_Display.cshtml";


        public IActionResult Index(VaultSearchViewModel model)
        {
            var result = VaultService.Instance.Search(model.ToSearchModel());

            if (IsAjaxRequest())
                return PartialView(PARTIAL_LIST, result);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Vault model)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                VaultService.Instance.Add(model);

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
            JsonResultObject result = new JsonResultObject();

            try
            {
                VaultService.Instance.Delete(id);

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

            try
            {
                var model = VaultService.Instance.GetById(id);

                result.PartialViewHtml = await RenderService.RenderToStringAsync(PARTIAL_FORM, model);
            }
            catch (PermissionException)
            {
                return NotFound();
            }

            return Ok(result);
        }

        public async Task<IActionResult> Display(int id)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                var model = VaultService.Instance.GetById(id);

                result.PartialViewHtml = await RenderService.RenderToStringAsync(PARTIAL_DISPLAY, model);

            }
            catch (PermissionException)
            {
                return NotFound();
            }

            return Ok(result);
        }
    }
}