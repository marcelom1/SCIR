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
    [Authorize(Roles = "ADMINISTRADOR")]
    public class UsuarioController : Controller
    {
        private UsuarioServer UsuarioServer = new UsuarioServer();
        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Listar(string searchPhrase, int current = 1, int rowCount = 10)
        {
            var request = FormatGridUtils< Usuario>.Format(Request, searchPhrase, new Usuario(), current, rowCount);

            var response = UsuarioServer.Listar(request);

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
            var model = new UsuarioVM();

            if (id != 0)
                model.Usuario = UsuarioServer.GetEntidade(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Salvar(Usuario usuario)
        {
            var model = new UsuarioVM();
            try
            {
                if (usuario.Id != 0)
                {
                    UsuarioServer.Atualizar(usuario);
                    model.Usuario = usuario;
                    model.Consistencia.Add("Alterado com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
                else
                {
                    UsuarioServer.Novo(usuario);
                    model.Usuario = usuario;
                    model.Consistencia.Add("Incluido com sucesso!", ConsisteUtils.Tipo.Sucesso);
                }
            }
            catch (Exception e)
            {

                model.Usuario = usuario;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;
            }


            return View("Form", model);
        }

        [HttpPost]
        public ActionResult Excluir(Usuario usuario)
        {
            var model = new UsuarioVM();
            try
            {
                UsuarioServer.Excluir(usuario);
                model.Consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                model.Usuario = usuario;
                var consistencia = new ConsisteUtils();
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
                model.Consistencia = consistencia;

                return View("Form", model);
            }


            return RedirectToAction("Index", "Usuario");
        }

        [HttpPost]
        public JsonResult ExcluirAjax(Usuario usuario)
        {
            var consistencia = new ConsisteUtils();
            try
            {
                UsuarioServer.Excluir(usuario);
                consistencia.Add("Registro excluído com sucesso!", ConsisteUtils.Tipo.Sucesso);
            }
            catch (Exception e)
            {
                consistencia.Add(e.Message, ConsisteUtils.Tipo.Inconsistencia);
            }

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteNovoAtualiza(Usuario usuario)
        {
            var consistencia = new ConsisteUtils();

            if (usuario.Id != 0)
                consistencia = UsuarioServer.ConsisteAtualizar(usuario);
            else
                consistencia = UsuarioServer.ConsisteNovo(usuario);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ConsisteExcluir(Usuario usuario)
        {
            var consistencia = new ConsisteUtils();

            consistencia = UsuarioServer.ConsisteExcluir(usuario);

            return Json(consistencia, JsonRequestBehavior.AllowGet);
        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetPapel(string searchTerm)
        {
            var cursos = UsuarioServer.GetFiltroPapelString("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult GetUsuariosAdmServidores(string searchTerm)
        {
            var cursos = UsuarioServer.GetUsuariosAdmServidores("Nome", searchTerm);

            var modifica = cursos.Select(x => new
            {
                id = x.Id,
                text = x.Id + " - " + x.Nome
            });

            return Json(modifica, JsonRequestBehavior.AllowGet);


        }

        
    }
}