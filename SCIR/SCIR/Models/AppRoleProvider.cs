using SCIR.DAO.Cadastros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace SCIR.Models
{
    public class AppRoleProvider : RoleProvider
    {
        public override string ApplicationName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override void CreateRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new NotImplementedException();
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new NotImplementedException();
        }

        public override string[] GetAllRoles()
        {
            throw new NotImplementedException();
        }

        public override string[] GetRolesForUser(string username)
        {
            UsuarioDao dbUsuarios = new UsuarioDao();
            var user = dbUsuarios.BuscarUserName(username);

            switch (user.PapelId)
            {
                case (int)PapelDao.PapelUsuario.Administrador:
                    return new string[] { "ADMINISTRADOR" };

                case (int)PapelDao.PapelUsuario.Servidor:
                    return new string[] { "SERVIDOR" };

                case (int)PapelDao.PapelUsuario.Discente:
                    return new string[] { "DISCENTE" };

                default:
                    return new string[] { "USER" };
            }
            
        }

        public override string[] GetUsersInRole(string roleName)
        {
            throw new NotImplementedException();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            UsuarioDao dbUsuarios = new UsuarioDao();
            var user = dbUsuarios.BuscarUserName(username);

            switch (user.PapelId)
            {
                case (int)PapelDao.PapelUsuario.Administrador when roleName == "ADMINISTRADOR":
                    return true;

                case (int)PapelDao.PapelUsuario.Servidor when roleName == "SERVIDOR":
                    return true;

                case (int)PapelDao.PapelUsuario.Discente when roleName == "DISCENTE":
                    return true;

                default:
                    return false;
            }

        }

        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            throw new NotImplementedException();
        }

        public override bool RoleExists(string roleName)
        {
            switch (roleName.ToUpper())
            {
                case "ADMINISTRADOR":
                    return true;

                case "SERVIDOR":
                    return true;

                case "DISCENTE":
                    return true;

                default:
                    return false;
            }
            
        }
    }
}