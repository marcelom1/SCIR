using SCIR.Business.Cadastros;
using SCIR.DAO.Cadastros;
using SCIR.Email;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace SCIR.Business.Login
{
    public class LoginServer
    {
        private UsuarioDao dbUsuario = new UsuarioDao();
        private CriptoAES cripto = new CriptoAES("TRABALHOCONCLUSAOCURSOMARCELOMIGLIOLIADS2018");

        public PapelDao.PapelUsuario Autentica(string login, string senha)
        {
            var autenticado = dbUsuario.ConfirmacaoAutenticacao(login, senha);
            if (autenticado != null && autenticado.Ativo)
            {
                FormsAuthentication.SetAuthCookie(autenticado.Id.ToString(), false);

                switch (autenticado.PapelId)
                {
                    case (int)PapelDao.PapelUsuario.Administrador:
                        return PapelDao.PapelUsuario.Administrador;

                    case (int)PapelDao.PapelUsuario.Servidor:
                        return PapelDao.PapelUsuario.Servidor;

                    case (int)PapelDao.PapelUsuario.Discente:
                        return PapelDao.PapelUsuario.Discente;
                }

            }

            return PapelDao.PapelUsuario.UsuarioNaoAutenticado;

        }
        public PapelDao.PapelUsuario Logout()
        {
            FormsAuthentication.SignOut();
            return PapelDao.PapelUsuario.UsuarioNaoAutenticado;
        }

        public static Usuario RetornarUsuarioLogado(string idUsuario)
        {
            UsuarioDao dbUsuario = new UsuarioDao();
            var id = 0;
            int.TryParse(idUsuario, out id);
            if (id == 0)
                throw new ArgumentException("Usuário não encontrado.");

            var user = dbUsuario.BuscarPorId(id);
            return user;
        }

        internal void RecuperarSenha(Usuario usuario, string url)
        {
            if (usuario.Ativo)
            {
                var senha = GeraSenhaAleatoria();
                usuario.SenhaReset = cripto.Encrypt(senha);
                var uServer = new UsuarioServer();
                usuario = uServer.Atualizar(usuario,true);

                url += $"/Login/ConfirmacaoEmail?id={usuario.Id}&s={senha}";

                var textEmail = ($@"    Foi solicitado a retificação de senha para o seu usuário no sistema SCIR - IFSC, para confirmar a retificação de senha deve entrar no link abaixo <br> {url} <br><br> E a sua senha após entrar no link anterior provisória é: <br>{senha} <br><br>Após efetuar o login é aconselhavel trocar a sua senha no sistema imediatamente!");
                EnvioEmail.SendMailGeneric(usuario, textEmail, "Conta de Usuário SCIR - IFSC");
            }

        }

        internal void CriarUsuarioAutoCadastro(string email, string url)
        {
            if (!ValidateUtils.IsValidEmail(email))
                throw new Exception("e-mail informado não é válido");

            var indiceArroba = email.IndexOf("@");
            var dominio = email.Substring(indiceArroba+1);
            var usuario = email.Substring(0, indiceArroba);
            var papel = PapelDao.PapelUsuario.UsuarioNaoAutenticado;

            if (dominio.ToLower() == "aluno.ifsc.edu.br")
                papel = PapelDao.PapelUsuario.Discente;
            else if (dominio.ToLower() == "ifsc.edu.br")
                papel = PapelDao.PapelUsuario.Servidor;

            if (papel != PapelDao.PapelUsuario.UsuarioNaoAutenticado)
            {
                var senha = GeraSenhaAleatoria();
                var novoUsuario = new Usuario
                {
                    Nome = usuario,
                    Email = email,
                    Ativo = false,
                    PapelId = (int)papel,
                    Senha = senha,
                    SenhaReset = cripto.Encrypt(senha)
                };

                var uServer = new UsuarioServer();
                novoUsuario = uServer.Novo(novoUsuario);

                url += $"/Login/ConfirmacaoEmail?id={novoUsuario.Id}&s={novoUsuario.Senha}";

                var textEmail = ($@"Seja Bem-Vindo ao SCIR-IFSC <br><br> Foi criado o seu usuário e senha, para acessar deve entrar no link a baixo para ativar a sua conta<br> {url} <br><br> E a sua senha após entrar no link anterior provisória é: <br>{senha} <br><br>Após efetuar o login é aconselhavel trocar a sua senha no sistema imediatamente!");
                EnvioEmail.SendMailGeneric(novoUsuario, textEmail, "Conta de Usuário SCIR - IFSC");
            }
            else
                throw new Exception("Deve informar o seu e-mail fornecido pela instituição de ensino!");
        }

        internal void ConfirmacaoEmail(int id, string senha)
        {
            var usuario = dbUsuario.BuscarPorId(id);
            var senhaDescript = cripto.Decrypt(usuario.SenhaReset);
            if (senhaDescript == senha)
            {
                if (usuario.Senha == usuario.SenhaReset)//Significa que é usuário novo, e deve ativar ele no cadastro
                    usuario.Ativo = true;
                else //Senão é um usuário querendo recuperar a senha, não deve mudar o status ativo do usuário
                    usuario.Senha = usuario.SenhaReset;

                usuario.SenhaReset = "";
                var uServer = new UsuarioServer();
                uServer.Atualizar(usuario,true);
            }
            else
                throw new Exception("Código de autenticação não é valido");
        }

        private string GeraSenhaAleatoria()
        {
            string caracteresPermitidos = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            char[] chars = new char[8];
            Random rd = new Random();
            for (int i = 0; i < 8; i++)
            {
                chars[i] = caracteresPermitidos[rd.Next(0, caracteresPermitidos.Length)];
            }
            return new string(chars);
        }

        public Usuario BuscarEmailCadastrado(string email)
        {
            return dbUsuario.BuscarUserName(email);
        }

        public string AlterarSenha(Usuario usuario, string novaSenha, string confirmacaoSenha)
        {
            var autenticado = Autentica(usuario.Email, usuario.Senha);
            if (autenticado != PapelDao.PapelUsuario.UsuarioNaoAutenticado)
            {
                if (novaSenha.Equals(confirmacaoSenha))
                {
                    usuario = BuscarEmailCadastrado(usuario.Email);
                    usuario.Senha = novaSenha;
                    var uServer = new UsuarioServer();
                    usuario = uServer.Atualizar(usuario);
                    return "Senha alterada com sucesso!";
                }
                return "Erro nova senha e a confirmação não são as mesmas";
            }
            return "A senha atual não confere com a cadastrada";
        }
        
    }
}
