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

        internal void RecuperarSenha(Usuario usuario)
        {
            if (usuario.Ativo)
            {
               
            }

        }

        internal void CriarUsuarioAutoCadastro(string email)
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
                    SenhaReset = senha
                };

                var uServer = new UsuarioServer();
                novoUsuario = uServer.Novo(novoUsuario);
                var link = "2222";

                var textEmail = ($@"Seja Bem-Vindo ao SCIR-IFSC </br></br> Foi criado o seu usuário e senha para acessar deve entrar no link a baixo para ativar a conta</br> {link} <br><br> E a sua senha provisória é: </br>{senha} </br></br>Após efetuar o login é aconselhavel trocar a sua senha no sistema!");
                EnvioEmail.SendMailGeneric(novoUsuario, textEmail, "Conta de Usuário SCIR - IFSC");
            }
            else
                throw new Exception("Deve informar o seu e-mail fornecido pela instituição!");
        }

        private string GeraSenhaAleatoria()
        {
            string caracteresPermitidos = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
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
        
    }
}
