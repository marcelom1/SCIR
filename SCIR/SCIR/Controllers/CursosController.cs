using SCIR.Business.Cadastros;
using SCIR.Models;
using SCIR.Models.ViewModels;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace SCIR.Controllers
{
    public class CursosController : Controller
    {
        private CursosServer cursoServer = new CursosServer();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(int id = 0)
        {
            var model = new CursoVM();

            if (id != 0)
                model.Curso = cursoServer.GetEntidade(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(Cursos curso, bool Consistido = false)
        {
            var model = new CursoVM();
          
            if (curso.Id != 0)
            {
                var consistencia = cursoServer.ConsisteAtualizar(curso);
                if (!Consistido && (consistencia.Inconsistencias.Any() || consistencia.Advertencias.Any()))
                {
                    model.Curso = curso;
                    model.Consistencia = consistencia;
                }
                else
                {
                    cursoServer.Atualizar(curso);
                    model.Curso = curso;
                }
            }
            else 
            {
                var consistencia = cursoServer.ConsisteNovo(curso);
                if (!Consistido && (consistencia.Inconsistencias.Any() || consistencia.Advertencias.Any()))
                {
                    model.Curso = curso;
                    model.Consistencia = consistencia;
                }
                else
                {
                    cursoServer.Novo(curso);
                    model.Curso = curso;
                }
            }
       
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(Cursos curso)
        {
            cursoServer.Excluir(curso);

            return RedirectToAction("Index", "Cursos");
        }

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils.Format(Request, searchPhrase, current, rowCount);

            var response = cursoServer.Listar(request);

            return Json(new { rows = response.Entidades,
                              current,
                              rowCount,
                              total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet );
        }


    }
}