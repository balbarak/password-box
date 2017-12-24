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
    public class PasswordBoxController : BaseController
    {
        private const string PARTIAL_LIST = "~/Views/PasswordBox/_List.cshtml";
            

        public IActionResult Index(AccountSearchViewModel model)
        {
            var result = AccountService.Instance.Search(model.ToSearchModel());

            if (IsAjaxRequest())
                return PartialView(PARTIAL_LIST, result);

            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Account model)
        {
            JsonResultObject result = new JsonResultObject();

            try
            {
                AccountService.Instance.Add(model);

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
                AccountService.Instance.Delete(id);

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