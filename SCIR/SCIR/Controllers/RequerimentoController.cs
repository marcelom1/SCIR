using SCIR.Business.Cadastros;
using SCIR.Business.Requerimentos;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using SCIR.Utils.TipoFormularioUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

namespace SCIR.Controllers
{
    public class RequerimentoController : Controller
    {
        private TipoRequerimentoServer TipoRequerimentoServer = new TipoRequerimentoServer();
        private RequerimentoServer ServerRequerimento = new RequerimentoServer();


        // GET: Requerimento
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form()
        {
            return View();
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
            var requerimento = new RequerimentoGridDC();

            if (filtrarPorAtendente)
            {
                //Definir regra para filtrar por Atendente pegando o usuário logado
            }

            if (filtrarPorRequerente)
            {
                //Definir regra para filtrar por Requerente pegando o usuário logado
            }

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