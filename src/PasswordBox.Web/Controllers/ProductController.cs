using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordBox.Domain.Models;
using PasswordBox.Core.Exceptions;
using PasswordBox.Web.TagHelpers.Breadcrumb;
using PasswordBox.Core.Resources;
using PasswordBox.Application.Services;
using PasswordBox.Web.ViewModels.Search;

namespace PasswordBox.Web.Controllers
{
    //[BreadCrumb(
    //      TitleResourceName = nameof(CommonText.Products),
    //      TitleResourceType = typeof(CommonText),
    //      Order = 0,
    //      UseDefaultRouteUrl = true,
    //      Icon = "icon-briefcase")]
    public class ProductController : BaseController
    {
        public IActionResult Index(ProductSearchViewModel model)
        {
            var result = ProductService.Instance.Search(model.ToSearchModel());

            return View(result);
        }

        [BreadCrumb(
          TitleResourceName = nameof(CommonText.Add),
          TitleResourceType = typeof(CommonText),
          Order = 1,
          Icon = "fa fa-plus")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Product product)
        {
            try
            {
                ProductService.Instance.Add(product);

                SetSuccess();
            }
            catch (BusinessException ex)
            {
                SetError(ex);

                return View(product);
            }

            return RedirectToAction("Index");
        }

        [BreadCrumb(
          TitleResourceName = nameof(CommonText.Edit),
          TitleResourceType = typeof(CommonText),
          Order = 1,
          Icon = "fa fa-pencil")]
        public IActionResult Edit(int id)
        {
            var model = ProductService.Instance.GetById(id);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Product product)
        {
            try
            {
                var original = ProductService.Instance.GetById(product.Id);

                original = original.Update(product);

                ProductService.Instance.Update(original);

                SetSuccess();
            }
            catch (BusinessException ex)
            {
                SetError(ex);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                ProductService.Instance.Delete(id);
                SetSuccess();
            }
            catch (BusinessException ex)
            {
                SetError(ex);
            }

            return RedirectToAction("Index");
        }

    }
}