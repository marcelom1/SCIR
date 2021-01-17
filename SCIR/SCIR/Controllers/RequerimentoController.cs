using SCIR.Business.Cadastros;
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

    }
}