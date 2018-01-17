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
    }
}