using SCIR.DAO.Cadastros;
using SCIR.Models;
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
            if (autenticado != null)
            {
                FormsAuthentication.SetAuthCookie(autenticado.Email, false);

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
        
        public static Usuario RetornarUsuarioLogado(string email)
        {
            UsuarioDao dbUsuario = new UsuarioDao();
            var user = dbUsuario.BuscarUserName(email);
            return user;
        }

    }
}