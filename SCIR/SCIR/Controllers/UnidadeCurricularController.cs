using SCIR.Business.Cadastros;
using SCIR.Models;
using SCIR.Models.ViewModels;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace SCIR.Controllers
{
    public class UnidadeCurricularController : Controller
    {
        private UnidadeCurricularServer UnidadeCurricularServer = new UnidadeCurricularServer();
        private CursosServer CursosServer = new CursosServer();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils< UnidadeCurricular>.Format(Request, searchPhrase, new UnidadeCurricular(), current, rowCount);

            var response = UnidadeCurricularServer.Listar(request);

            return Json(new
            {
                rows = response.Entidades,
                current,
                rowCount,
                total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Form(int id = 0)
        {
            var model = new UnidadeCurricularVM();

            if (id != 0)
                model.UnidadeCurricular = UnidadeCurricularServer.GetEntidade(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(UnidadeCurricular unidadeCurricular)
        {
            var model = new UnidadeCurricularVM();
            try
            {
                if (unidadeCurricular.Id != 0)
                {
                    UnidadeCurricularServer.Atualizar(unidadeCurricular);
                    model.UnidadeCurricular = unidadeCurricular;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                {
                    UnidadeCurricularServer.Novo(unidadeCurricular);
                    model.UnidadeCurricular = unidadeCurricular;
                    model.Consistencia.Add("Incluido com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
            }
            catch (Exception e)
            {

                model.UnidadeCurricular = unidadeCurricular;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }


            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(UnidadeCurricular unidadeCurricular)
        {
            var model = new UnidadeCurricularVM();
            try
            {
                UnidadeCurricularServer.Excluir(unidadeCurricular);
                model.Consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                model.UnidadeCurricular = unidadeCurricular;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;

                return View("Form", model);
            }


            return RedirectToAction("Index", "UnidadeCurricular");
        }

        [HttpPost]
        public JsonResult ExcluirAjax(UnidadeCurricular unidadeCurricular)
        {
            var consistencia = new ConsisteUtils();
            try
            {
                UnidadeCurricularServer.Excluir(unidadeCurricular);
                consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
            }

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(UnidadeCurricular unidadeCurricular)
        {
            var consistencia = new ConsisteUtils();

            if (unidadeCurricular.Id != 0)
                consistencia = UnidadeCurricularServer.ConsisteAtualizar(unidadeCurricular);
            else
                consistencia = UnidadeCurricularServer.ConsisteNovo(unidadeCurricular);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteExcluir(UnidadeCurricular unidadeCurricular)
        {
            var consistencia = new ConsisteUtils();

            consistencia = UnidadeCurricularServer.ConsisteExcluir(unidadeCurricular);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetCursos(string searchTerm)
        {
            var cursos = CursosServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetUnidadeCurricular(string searchTerm)
        {
            var cursos = UnidadeCurricularServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetUnidadeCurricularFilterCurso(string searchTerm, int cursoId)
        {
            var unidadeCurricular = UnidadeCurricularServer.GetFiltroEntidadeString("Nome", searchTerm, cursoId);

            var modifica = unidadeCurricular.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }
    }
}