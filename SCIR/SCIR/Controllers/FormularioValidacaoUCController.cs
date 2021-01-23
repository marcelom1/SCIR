using SCIR.Business.Cadastros;
using SCIR.Business.Requerimentos;
using SCIR.Controllers.Interfaces;
using SCIR.Models;
using SCIR.Models.ViewModels;
using SCIR.Utils;
using SCIR.Utils.TipoFormularioUtils;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace SCIR.Controllers
{
    public class FormularioValidacaoUCController : Controller, IFormulario 
    {
        private FormularioValidacaoUCServer FormularioValidacaoUCServer = new FormularioValidacaoUCServer();
        private TipoValidacaoCurricularServer TipoValidacaoCurricularServer = new TipoValidacaoCurricularServer();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(int statusAtual = 0, int tipoRequerimento = 0)
        {
            var model = new FormularioValidacaoUCVM();

            return PartialView(model);
        }

        public ActionResult FormEncaminhamento(int statusAtual = 0, int tipoRequerimento = 0)
        {
            var model = new FormularioValidacaoUCVM();

            return PartialView(model);
        }

        public TipoFormularioUtils GetActionForm()
        {
            var formulario = new TipoFormularioUtils { Action = "Form", Route = "FormularioValidacaoUC" };

            return formulario;
        }

        public JsonResult ListarAnexos(string searchPhrase, int current = 1, int rowCount = 10, int statusAtualId = 0, int tipoRequerimentoId = 0)
        {
            var request = FormatGridUtils<FormularioValidacaoUC>.Format(Request, searchPhrase, new FormularioValidacaoUC() , current, rowCount);

            return Json(new
            {
                rows = "TESTE",
                current,
                rowCount,
                total = 1
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Salvar(FormularioValidacaoUC formularioValidacaoUC)
        {
            var files = Request.Files;
            formularioValidacaoUC.UsuarioAtendenteId = 7;
            formularioValidacaoUC.UsuarioRequerenteId = 7;
            formularioValidacaoUC.StatusRequerimentoId = 3;
            

            var model = new FormularioValidacaoUCVM();
            try
            {
                if (formularioValidacaoUC.Id != 0)
                {
                    FormularioValidacaoUCServer.Atualizar(formularioValidacaoUC, files);
                    model.FormularioValidacaoUC = formularioValidacaoUC;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                {
                    FormularioValidacaoUCServer.Novo(formularioValidacaoUC, files, Server);
                    model.Consistencia.Add("Incluido com sucesso! Protocolo: " + formularioValidacaoUC.Protocolo, ConsisteUtils.Tipo.Sucesso);
                    model.FormularioValidacaoUC = new FormularioValidacaoUC();
                    
                }
            }
            catch (Exception e)
            {

                model.FormularioValidacaoUC = formularioValidacaoUC;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }

            return Json(model.Consistencia, JsonRequestBehavior.AllowGet);
        }

       

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(FormularioValidacaoUC formularioValidacaoUC)
        {
            var consistencia = new ConsisteUtils();

            if (formularioValidacaoUC.Id != 0)
                consistencia = FormularioValidacaoUCServer.ConsisteAtualizar(formularioValidacaoUC);
            else
                consistencia = FormularioValidacaoUCServer.ConsisteNovo(formularioValidacaoUC);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetTipoValidacaoCurricular(string searchTerm)
        {
            var cursos = TipoValidacaoCurricularServer.GetFiltroEntidadeString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }
    }
}