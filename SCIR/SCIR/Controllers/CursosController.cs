using SCIR.DAO.Cadastros;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;

namespace SCIR.Controllers
{
    public class CursosController : Controller
    {
        private CursosDao dbCursos = new CursosDao();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Form(int id = 0)
        {
            var model = new Cursos();

            if (id != 0)
                model = dbCursos.BuscarPorId(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Novo(Cursos curso)
        {

            var pesquisa = dbCursos.BuscarPorId(curso.Id);
            var model = new Cursos();
           
            if (ModelState.IsValid)
            {
                //Caso já existir ela será atualizada
                if (pesquisa != null)
                {
                    dbCursos.Update(curso);
                    return RedirectToAction("Index", "Cursos");
                }
                else //Senão é nova sendo adicionada
                {
                    dbCursos.Add(curso);
                    return RedirectToAction("Form", new { id = curso.Id });
                }
            }
            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(Cursos curso)
        {
            var pesquisa = dbCursos.BuscarPorId(curso.Id);

            if (pesquisa != null)
                dbCursos.Delete(pesquisa);

            return RedirectToAction("Index", "Cursos");
        }

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            var ordenacao = Request[chave];
            var campo = chave.Replace("sort[", string.Empty).Replace("]", string.Empty);

            var campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);

            var pesquisa = dbCursos.ListGrid(current, rowCount, campoOrdenacao, searchPhrase);
            var totalRegistros = dbCursos.TotalRegistros();

            return Json(new { rows = pesquisa,
                              current,
                              rowCount,
                              total = totalRegistros
            }, JsonRequestBehavior.AllowGet );
        }

    }
}