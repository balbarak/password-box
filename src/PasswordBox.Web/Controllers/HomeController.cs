using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PasswordBox.Web.Models;
using PasswordBox.Web.TagHelpers.Breadcrumb;

namespace PasswordBox.Web.Controllers
{
    public class HomeController : BaseController
    {
        
        public IActionResult Index()
        {
            SetSuccess(false);

            return View();
        }
        
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
