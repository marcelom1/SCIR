﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCIR.Controllers
{
    public class CursosController : Controller
    {
        // GET: Cursos
        public ActionResult Index()
        {
            return View();
        }
    }
}