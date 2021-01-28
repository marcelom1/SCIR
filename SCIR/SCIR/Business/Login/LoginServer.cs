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

    }
}