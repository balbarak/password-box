using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PasswordBox.Web.Controllers
{
    public class PasswordBoxController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}