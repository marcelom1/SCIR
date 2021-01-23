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
using System.Web.Script.Services;
using System.Web.Services;

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
        public ActionResult Salvar(Cursos curso)
        {
            var model = new CursoVM();
            try
            {
                if (curso.Id != 0)
                {
                    cursoServer.Atualizar(curso);
                    model.Curso = curso;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                { 
                    cursoServer.Novo(curso);
                    model.Curso = curso;
                    model.Consistencia.Add("Incluido com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
            }
            catch (Exception e)
            {

                model.Curso = curso;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }
            
       
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(Cursos curso)
        {
            var model = new CursoVM();
            try
            {
                cursoServer.Excluir(curso);
                model.Consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                model.Curso = curso;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;

                return View("Form", model);
            }
           

            return RedirectToAction("Index", "Cursos");
        }

        [HttpPost]
        public JsonResult ExcluirAjax(Cursos curso)
        {
            var consistencia = new ConsisteUtils();
            try
            {
                cursoServer.Excluir(curso);
                consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
            }

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(Cursos curso)
        {
            var consistencia = new ConsisteUtils();

            if (curso.Id != 0)
                consistencia = cursoServer.ConsisteAtualizar(curso);
            else
                consistencia = cursoServer.ConsisteNovo(curso);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteExcluir(Cursos curso)
        {
            var consistencia = new ConsisteUtils();

            consistencia = cursoServer.ConsisteExcluir(curso);
          
            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }
        

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils<Cursos>.Format(Request, searchPhrase, new Cursos(),current, rowCount);

            var response = cursoServer.Listar(request);

            return Json(new { rows = response.Entidades,
                              current,
                              rowCount,
                              total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet );
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetCursos(string searchTerm)
        {
            var cursos = cursoServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

    }
}