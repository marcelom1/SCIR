using SCIR.Business.Cadastros;
using SCIR.Business.Login;
using SCIR.Business.Requerimentos;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Models.ViewModels;
using SCIR.Utils;
using SCIR.Utils.TipoFormularioUtils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace SCIR.Controllers
{
    [Authorize(Roles = "ADMINISTRADOR,SERVIDOR,DISCENTE")]
    public class RequerimentoController : Controller
    {
        private TipoRequerimentoServer TipoRequerimentoServer = new TipoRequerimentoServer();
        private RequerimentoServer ServerRequerimento = new RequerimentoServer();
        private ArquivoRequerimentoServer ArquivoRequerimentoServer = new ArquivoRequerimentoServer();
        


        // GET: Requerimento
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult VisualizarRequerimento(RequerimentoVM requerimento)
        {
            var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
            var model = new RequerimentoVM();
            try
            {
                model = new RequerimentoVM(ServerRequerimento.GetRequerimentoId(requerimento, usuario));
            }
            catch (Exception e)
            {
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }
          
            return View(model);
        }

        public JsonResult GetAnexosRequerimento(string searchPhrase, int current = 1, int rowCount = 5, int requerimentoId = 0)
        {
            var arquivoRequerimento = new ArquivoRequerimento {RequerimentoId = requerimentoId };
            var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
            var request = FormatGridUtils<ArquivoRequerimento>.Format(Request, searchPhrase, arquivoRequerimento, current, rowCount);
            var response = new ResponseGrid<ArquivoRequerimento>();
           
            response = ArquivoRequerimentoServer.Listar(request, usuario);
            
            return Json(new
            {
                rows = response.Entidades,
                current,
                rowCount,
                total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet);
        }

        public string Download(string file)
        {

            //string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FileManagementPath"]);


            //string actualFilePath = System.IO.Path.Combine(filePath, file);

            //actualFilePath = ArquivoRequerimentoServer.ge

            //HttpContext.Response.ContentType = "APPLICATION/OCTET-STREAM";
            //string filename = Path.GetFileName(actualFilePath);
            //String Header = "Attachment; Filename=" + filename;
            //HttpContext.Response.AppendHeader("Content-Disposition", Header);
            //HttpContext.Response.WriteFile(actualFilePath);
            //HttpContext.Response.End();
            return "";
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

        [WebMethod()]
        public ActionResult CarregarFormulario(int tipoRequerimentoID = 0)
        {
            var requerimento = TipoRequerimentoServer.GetEntidade(tipoRequerimentoID);
            var action = TipoFormularioUtils.RetornarFormulario(requerimento.TipoFormulario.Codigo);

            return RedirectToAction(action.Action, action.Route);
        }

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10, bool filtrarPorAtendente = false, bool filtrarPorRequerente = false)
        {
            var requerimento = new RequerimentoGridDC {};

            if (searchPhrase.ToLower() == "aberturatostring asc")
                searchPhrase = "ABERTURA ASC";
            else if (searchPhrase.ToLower() == "aberturatostring desc")
                searchPhrase = "ABERTURA DESC";

            var request = FormatGridUtils<Requerimento>.Format(Request, searchPhrase, requerimento, current, rowCount);

            var response = new ResponseGrid<RequerimentoGridDC>();

            if (filtrarPorAtendente)
            {
                request.Entidade = new RequerimentoGridDC { UsuarioAtendenteId = LoginServer.RetornarUsuarioLogado(User.Identity.Name).Id };
                response = ServerRequerimento.ListarPorAtendente(request);
                
            }
            else if (filtrarPorRequerente)
            {
                request.Entidade = new RequerimentoGridDC { UsuarioRequerenteId = LoginServer.RetornarUsuarioLogado(User.Identity.Name).Id };
                response = ServerRequerimento.ListarPorRequerente(request);
            }
           
            return Json(new
            {
                rows = response.Entidades,
                current,
                rowCount,
                total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "ADMINISTRADOR")]
        public JsonResult ListarTodos(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var requerimento = new RequerimentoGridDC();

            if (searchPhrase.ToLower() == "aberturatostring asc")
                searchPhrase = "ABERTURA ASC";
            else if (searchPhrase.ToLower() == "aberturatostring desc")
                searchPhrase = "ABERTURA DESC";

            var request = FormatGridUtils<Requerimento>.Format(Request, searchPhrase, requerimento, current, rowCount);

            var response = ServerRequerimento.ListarTudo(request);

            return Json(new
            {
                rows = response.Entidades,
                current,
                rowCount,
                total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet);
        }
    }
}