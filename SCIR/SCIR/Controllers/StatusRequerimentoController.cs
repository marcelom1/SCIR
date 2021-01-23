using SCIR.Business.Cadastros;
using SCIR.Models;
using SCIR.Models.ViewModels;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;

namespace SCIR.Controllers
{
    public class StatusRequerimentoController : Controller
    {
        private StatusRequerimentoServer StatusRequerimentoServer = new StatusRequerimentoServer();

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils<StatusRequerimento>.Format(Request, searchPhrase, new StatusRequerimento() , current, rowCount);

            var response = StatusRequerimentoServer.Listar(request);

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
            var model = new StatusRequerimentoVM();

            if (id != 0)
                model.StatusRequerimento = StatusRequerimentoServer.GetEntidade(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(StatusRequerimento statusRequerimento)
        {
            var model = new StatusRequerimentoVM();
            try
            {
                if (statusRequerimento.Id != 0)
                {
                    StatusRequerimentoServer.Atualizar(statusRequerimento);
                    model.StatusRequerimento = statusRequerimento;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                {
                    StatusRequerimentoServer.Novo(statusRequerimento);
                    model.StatusRequerimento = statusRequerimento;
                    model.Consistencia.Add("Incluido com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
            }
            catch (Exception e)
            {

                model.StatusRequerimento = statusRequerimento;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }


            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(StatusRequerimento statusRequerimento)
        {
            var model = new StatusRequerimentoVM();
            try
            {
                StatusRequerimentoServer.Excluir(statusRequerimento);
                model.Consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                model.StatusRequerimento = statusRequerimento;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;

                return View("Form", model);
            }


            return RedirectToAction("Index", "StatusRequerimento");
        }

        [HttpPost]
        public JsonResult ExcluirAjax(StatusRequerimento statusRequerimento)
        {
            var consistencia = new ConsisteUtils();
            try
            {
                StatusRequerimentoServer.Excluir(statusRequerimento);
                consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
            }

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(StatusRequerimento statusRequerimento)
        {
            var consistencia = new ConsisteUtils();

            if (statusRequerimento.Id != 0)
                consistencia = StatusRequerimentoServer.ConsisteAtualizar(statusRequerimento);
            else
                consistencia = StatusRequerimentoServer.ConsisteNovo(statusRequerimento);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteExcluir(StatusRequerimento statusRequerimento)
        {
            var consistencia = new ConsisteUtils();

            consistencia = StatusRequerimentoServer.ConsisteExcluir(statusRequerimento);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

       
    }
}