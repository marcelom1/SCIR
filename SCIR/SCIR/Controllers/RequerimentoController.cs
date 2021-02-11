using SCIR.Business.Cadastros;
using SCIR.Business.Login;
using SCIR.Business.Requerimentos;
using SCIR.DAO.Cadastros;
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
        private FluxoStatusServer FluxoStatusServer = new FluxoStatusServer();
        private UsuarioServer UsuarioServer = new UsuarioServer();
        private AuditoriaServer AuditoriaServer = new AuditoriaServer();

        public ActionResult Index(int filtro)
        {
            ViewBag.filtro = filtro;
            ViewBag.filtrarPorAtendente = filtro;
            ViewBag.filtrarPorRequerente = filtro == 0 ? 1 : 0;
            return View();
        }

        public ActionResult Form()
        {
            return View();
        }

        public ActionResult VisualizarRequerimento(RequerimentoVM requerimento, int origem)
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
            ViewBag.origem = origem;
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

        public string Download(int file, int requerimentoId)
        {

            //string filePath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["FileManagementPath"]);

            //string actualFilePath = System.IO.Path.Combine(filePath, file);

            var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
            var requerimento = new RequerimentoVM { Id = requerimentoId };
            var req = ServerRequerimento.GetRequerimentoId(requerimento, usuario); // ira verrificar a permissão do download
            var arquivo = ArquivoRequerimentoServer.GetArquivo(file);

            if (req.Id == arquivo.RequerimentoId)
            {
                var actualFilePath = arquivo.Caminho;

                HttpContext.Response.ContentType = "APPLICATION/OCTET-STREAM";
                string filename = Path.GetFileName(actualFilePath);
                String Header = "Attachment; Filename=" + filename;
                HttpContext.Response.AppendHeader("Content-Disposition", Header);
                HttpContext.Response.WriteFile(actualFilePath);
                HttpContext.Response.End();
            }
            return "";
        }

        public PartialViewResult ModalEncaminhar(int requerimentoId, int chamadoOrigem)
        {
            ViewBag.RequerimentoId = requerimentoId;
            ViewBag.Filtro = chamadoOrigem;
            return PartialView();
        }

        public PartialViewResult ModalAuditoria(int requerimentoId)
        {
            //ViewBag.RequerimentoId = requerimentoId;

            var response = AuditoriaServer.FiltroPorRequerimento(requerimentoId);
            var text = "";
            var dataAnterior = "";
            foreach (var item in response)
            {
                var data = item.DataModificacao.ToString("dd/MM/yyyy");
                if (dataAnterior != data)
                    text += "\n" + data;

                text += $" - Campo: {item.Campo}, \n Antes: {item.Antes}, \n Depois: {item.Depois} \n\n";
            }
            @ViewBag.Text = text;
            return PartialView();
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

        [WebMethod()]
        public ActionResult CarregarRequerimentoCamposExtras(int requerimentoID = 0)
        {
            var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
            var request = new RequerimentoVM { Id = requerimentoID };
            var requerimento = ServerRequerimento.GetRequerimentoId(request, usuario);

            var action = TipoFormularioUtils.RetornarCamposExtras(requerimento.TipoFormulario.Codigo);

            return RedirectToAction(action.Action, action.Route, new { requerimentoID });
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult EncaminharRequerimento(RequerimentoVM encaminhar)
        {
            try
            {
                var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
                encaminhar = new RequerimentoVM(ServerRequerimento.EncaminharRequerimento(encaminhar, usuario));
                encaminhar.Consistencia.Add("Encaminhado com sucesso", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                encaminhar.Consistencia = consistencia;
            }
            
            return Json(encaminhar, JsonRequestBehavior.AllowGet);
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
                var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
                request.Entidade = new RequerimentoGridDC { UsuarioRequerenteId = usuario.Id };
                if (usuario.PapelId == (int)PapelDao.PapelUsuario.Administrador)
                    response = ServerRequerimento.ListarTudo(request);
                else
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
        
        public JsonResult ListarAuditoriaRequerimento(string searchPhrase, int current = 1, int rowCount = 10, int requerimentoId = 0)
        {
            var requerimento = new Auditoria();

            requerimento.RequerimentoId= requerimentoId;

            var request = FormatGridUtils<Auditoria>.Format(Request, searchPhrase, requerimento, current, rowCount);

            var response = AuditoriaServer.Listar(request);

            return Json(new
            {
                rows = response.Entidades,
                current,
                rowCount,
                total = response.QuantidadeRegistros
            }, JsonRequestBehavior.AllowGet);
        }



        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetProximoStatus(string searchTerm, int requerimentoId)
        {
            var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
            var request = new RequerimentoVM { Id = requerimentoId };
            var requerimento = ServerRequerimento.GetRequerimentoId(request, usuario);

            var status = FluxoStatusServer.GetProximoStatus(requerimento.StatusRequerimentoId, searchTerm,requerimento.TipoRequerimentoId);

            var modifica = status.Select(x => new
            {
                id = x.StatusProximoId,
                text = x.StatusProximoId + " - " + x.StatusProximoNome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetProximoAtendente(string searchTerm, int requerimentoId)
        {
            var usuario = LoginServer.RetornarUsuarioLogado(User.Identity.Name);
            var request = new RequerimentoVM { Id = requerimentoId };
            var requerimento = ServerRequerimento.GetRequerimentoId(request, usuario);

            var status = UsuarioServer.GetProximoAtendente(requerimento, searchTerm);

            var modifica = status.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }


    }
}