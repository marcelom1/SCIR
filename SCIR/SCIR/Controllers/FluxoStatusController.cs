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
    public class FluxoStatusController : Controller
    {
        private FluxoStatusServer FluxoStatusServer = new FluxoStatusServer();
        private TipoRequerimentoServer TipoRequerimentoServer = new TipoRequerimentoServer();
        private StatusRequerimentoServer StatusRequerimentoServer = new StatusRequerimentoServer();

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils.Format(Request, searchPhrase, current, rowCount);

            var response = FluxoStatusServer.Listar(request);

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
            var model = new FluxoStatusVM();

            if (id != 0)
                model.FluxoStatus = FluxoStatusServer.GetEntidade(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(FluxoStatus fluxoStatus)
        {
            var model = new FluxoStatusVM();
            try
            {
                if (0 != 0)
                {
                    FluxoStatusServer.Atualizar(fluxoStatus);
                    model.FluxoStatus = fluxoStatus;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                {
                    FluxoStatusServer.Novo(fluxoStatus);
                    model.FluxoStatus = fluxoStatus;
                    model.Consistencia.Add("Incluido com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
            }
            catch (Exception e)
            {

                model.FluxoStatus = fluxoStatus;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }


            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(FluxoStatus fluxoStatus)
        {
            var model = new FluxoStatusVM();
            try
            {
                FluxoStatusServer.Excluir(fluxoStatus);
                model.Consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                model.FluxoStatus = fluxoStatus;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;

                return View("Form", model);
            }


            return RedirectToAction("Index", "FluxoStatus");
        }

        [HttpPost]
        public JsonResult ExcluirAjax(FluxoStatus fluxoStatus)
        {
            var consistencia = new ConsisteUtils();
            try
            {
                FluxoStatusServer.Excluir(fluxoStatus);
                consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
            }

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(FluxoStatus fluxoStatus)
        {
            var consistencia = new ConsisteUtils();

            if (0 != 0)
                consistencia = FluxoStatusServer.ConsisteAtualizar(fluxoStatus);
            else
                consistencia = FluxoStatusServer.ConsisteNovo(fluxoStatus);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteExcluir(FluxoStatus fluxoStatus)
        {
            var consistencia = new ConsisteUtils();

            consistencia = FluxoStatusServer.ConsisteExcluir(fluxoStatus);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }
        

        public ActionResult AdicionarProximoStatus(FluxoStatus fluxoStatus)
        {
            fluxoStatus.TipoRequerimento = TipoRequerimentoServer.GetEntidade(fluxoStatus.TipoRequerimentoId);
            fluxoStatus.StatusAtual = StatusRequerimentoServer.GetEntidade(fluxoStatus.StatusAtualId);
            return PartialView(fluxoStatus);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetTipoRequerimento(string searchTerm)
        {
            var tipoRequerimento = TipoRequerimentoServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = tipoRequerimento.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetStatusAtual(string searchTerm)
        {
            var status = StatusRequerimentoServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = status.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

    }
}