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
    public class TipoRequerimentoController : Controller
    {
        private TipoRequerimentoServer TipoRequerimentoServer = new TipoRequerimentoServer();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils< TipoRequerimento>.Format(Request, searchPhrase,new TipoRequerimento(), current, rowCount);

            var response = TipoRequerimentoServer.Listar(request);

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
            var model = new TipoRequerimentoVM();

            if (id != 0)
                model.TipoRequerimento = TipoRequerimentoServer.GetEntidade(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(TipoRequerimento tipoRequerimento)
        {
            var model = new TipoRequerimentoVM();
            try
            {
                if (tipoRequerimento.Id != 0)
                {
                    TipoRequerimentoServer.Atualizar(tipoRequerimento);
                    model.TipoRequerimento = tipoRequerimento;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                {
                    TipoRequerimentoServer.Novo(tipoRequerimento);
                    model.TipoRequerimento = tipoRequerimento;
                    model.Consistencia.Add("Incluido com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
            }
            catch (Exception e)
            {

                model.TipoRequerimento = tipoRequerimento;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }


            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(TipoRequerimento tipoRequerimento)
        {
            var model = new TipoRequerimentoVM();
            try
            {
                TipoRequerimentoServer.Excluir(tipoRequerimento);
                model.Consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                model.TipoRequerimento = tipoRequerimento;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;

                return View("Form", model);
            }


            return RedirectToAction("Index", "TipoRequerimento");
        }

        [HttpPost]
        public JsonResult ExcluirAjax(TipoRequerimento tipoRequerimento)
        {
            var consistencia = new ConsisteUtils();
            try
            {
                TipoRequerimentoServer.Excluir(tipoRequerimento);
                consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
            }

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(TipoRequerimento tipoRequerimento)
        {
            var consistencia = new ConsisteUtils();

            if (tipoRequerimento.Id != 0)
                consistencia = TipoRequerimentoServer.ConsisteAtualizar(tipoRequerimento);
            else
                consistencia = TipoRequerimentoServer.ConsisteNovo(tipoRequerimento);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteExcluir(TipoRequerimento tipoRequerimento)
        {
            var consistencia = new ConsisteUtils();

            consistencia = TipoRequerimentoServer.ConsisteExcluir(tipoRequerimento);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetTipoFormulario(string searchTerm)
        {
            var cursos = TipoRequerimentoServer.GetFiltroTipoFormularioString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetTipoRequerimento(string searchTerm)
        {
            var cursos = TipoRequerimentoServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }
    }
}