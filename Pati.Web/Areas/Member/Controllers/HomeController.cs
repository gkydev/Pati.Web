﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pati.Web.StringConsts;

namespace Pati.Web.Areas.Member.Controllers
{
    [Area(AreaConsts.Member)]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
