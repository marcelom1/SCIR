using SCIR.Business.Login;
using SCIR.DAO.Cadastros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCIR.Controllers
{
    public class LoginController : Controller
    {
        private LoginServer LoginServer = new LoginServer();

        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Erro = "";
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Autentica(string login, string senha)
        {
            if (LoginServer.Autentica(login,senha) != PapelDao.PapelUsuario.UsuarioNaoAutenticado)
            {

                if (Request.QueryString["ReturnUrl"] == null)
                    return RedirectToAction("Index", "Home");
                else
                    return RedirectToAction(Request.QueryString["ReturnUrl"].ToString());
            }

            ViewBag.Erro = "Falha no login";
            return View("Index");

        }


        public ActionResult Logout()
        {
            LoginServer.Logout();
            return RedirectToAction("Index", "Login");
        }

        [Authorize]
        public string RetornarNome()
        {
            var email = User.Identity.Name;
            return LoginServer.RetornarUsuarioLogado(email).Nome;
        }
        
    }
}