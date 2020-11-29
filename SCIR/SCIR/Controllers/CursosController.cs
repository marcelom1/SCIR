using SCIR.DAO.Cadastros;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}