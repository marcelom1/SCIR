using Newtonsoft.Json;
using SCIR.Business.Login;
using SCIR.DAO.Cadastros;
using SCIR.Datacontract;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Services;
using System.Web.Services;

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

        [Authorize]
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
                var url = Request.Url.DnsSafeHost;

                var usuario = LoginServer.BuscarEmailCadastrado(email);
                if (usuario == null)
                    LoginServer.CriarUsuarioAutoCadastro(email, url);
                else
                    LoginServer.RecuperarSenha(usuario, url);
                return "Email enviado com sucesso! Acesse a sua caixa de email para ativação da nova senha, além da caixa de entrada verifique se o e-mail não está na caixa de spam.";
            }
            return "=( Ocorreu um erro tente novamente!";
        }

        public static CaptchaReponse ValidateCaptcha(string response)
        {
            string secret = "6LdAp18aAAAAAGpus-aE7kmuZdWC9H0F1Ud-5p0N";
            var client = new WebClient();
            var jsonResult = client.DownloadString(
                string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
                secret, response));
            var resposta = JsonConvert.DeserializeObject<CaptchaReponse>(jsonResult.ToString());

            return resposta;
        }

        public ActionResult ConfirmacaoEmail(int id, string s)
        {
            LoginServer.ConfirmacaoEmail(id,s);
            return RedirectToAction("Index", "Login");

        }

        [Authorize]
        public PartialViewResult AlterarSenha()
        {
            return PartialView();
        }

        [Authorize]
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public JsonResult AlterarSenhaUsuario(string senhaAtual, string novaSenha, string confirmacaoSenha)
        {
            var email = User.Identity.Name;
            var usuario = LoginServer.RetornarUsuarioLogado(email);
            usuario.Senha = senhaAtual;

            var resposta = LoginServer.AlterarSenha(usuario, novaSenha, confirmacaoSenha);

            return Json(resposta, JsonRequestBehavior.AllowGet);

        }
    }
}