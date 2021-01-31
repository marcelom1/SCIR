using SCIR.Business.Cadastros;
using SCIR.Business.Login;
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
    [Authorize(Roles = "ADMINISTRADOR,SERVIDOR,DISCENTE")]
    public class FormularioValidacaoUCController : Controller, IFormulario 
    {
        private FormularioValidacaoUCServer FormularioValidacaoUCServer = new FormularioValidacaoUCServer();
        private TipoValidacaoCurricularServer TipoValidacaoCurricularServer = new TipoValidacaoCurricularServer();
        private CursosServer CursosServer = new CursosServer();
        private UnidadeCurricularServer UnidadeCurricularServer = new UnidadeCurricularServer();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(int statusAtual = 0, int tipoRequerimento = 0)
        {
            var model = new FormularioValidacaoUCVM();

            return PartialView(model);
        }

        [Authorize(Roles = "ADMINISTRADOR,SERVIDOR")]
        public ActionResult FormEncaminhamento(int statusAtual = 0, int tipoRequerimento = 0)
        {
            var model = new FormularioValidacaoUCVM();

            return PartialView(model);
        }

        public PartialViewResult VisualizarRequerimento(int requerimentoID)
        {
            var entidade = FormularioValidacaoUCServer.GetEntidade(requerimentoID);
            var model = new FormularioValidacaoUCVM { FormularioValidacaoUC = entidade};

            return PartialView(model);
        }

        public TipoFormularioUtils GetActionForm()
        {
            var formulario = new TipoFormularioUtils { Action = "Form", Route = "FormularioValidacaoUC" };

            return formulario;
        }

        public TipoFormularioUtils GetActionVisualizarRequerimento()
        {
            var formulario = new TipoFormularioUtils { Action = "VisualizarRequerimento", Route = "FormularioValidacaoUC" };

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
            
           
            var model = new FormularioValidacaoUCVM();
            try
            {
                formularioValidacaoUC.UsuarioRequerenteId = LoginServer.RetornarUsuarioLogado(User.Identity.Name).Id;
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