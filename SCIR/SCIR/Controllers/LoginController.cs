using Newtonsoft.Json;
using SCIR.Business.Login;
using SCIR.DAO.Cadastros;
using SCIR.Datacontract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            return View();
        }

        public ActionResult Autentica(string login, string senha)
        {
            if (LoginServer.Autentica(login,senha) != PapelDao.PapelUsuario.UsuarioNaoAutenticado)
            {

                if (Request.QueryString["ReturnUrl"] == null)
                    return RedirectToAction("Index", "Login");
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

        public PartialViewResult PrimeiroAcessoEsqueciSenha()
        {
            return PartialView();
        }

        public string EnviarPESenha(string email)
        {
            CaptchaReponse response = ValidateCaptcha(Request["g-recaptcha-response"]);
            if (response.Success)
            {
                var usuario = LoginServer.BuscarEmailCadastrado(email);
                if (usuario == null)
                    LoginServer.CriarUsuarioAutoCadastro(email);
                else
                    LoginServer.RecuperarSenha(usuario);
                return "teste";
            }
            return "";
        }

        public static CaptchaReponse ValidateCaptcha(string response)
        {
            string secret = "6Le_N7YUAAAAAKiu_3xKXm3uUtV7P_WFpbB_Qe7d";
            var client = new WebClient();
            var jsonResult = client.DownloadString(
                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                secret, response));
            var resposta = JsonConvert.DeserializeObject<CaptchaReponse>(jsonResult.ToString());

            return resposta;
        }

    }
}