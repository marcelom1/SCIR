using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SCIR.Models;

namespace SCIR.DAO.Formularios
{
    public class RequerimentoDao 
    {
        public static string WhereFiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            int id = 0;
            if (int.TryParse(searchPhrase, out id))
            {
                switch (coluna.ToUpper())
                {
                    case "USUARIOREQUERENTE":
                        where += string.Format("USUARIOREQUERENTEID = {0}", id);
                        break;
                    case "USUARIOATENDENTE":
                        where += string.Format("USUARIOATENDENTEID = {0}", id);
                        break;
                    case "STATUSREQUERIMENTO":
                        where += string.Format("STATUSREQUERIMENTOID = {0}", id);
                        break;
                    case "TIPOREQUERIMENTO":
                        where += string.Format("TIPOREQUERIMENTOID = {0}", id);
                        break;
                    case "TIPOFORMULARIO":
                        where += string.Format("TIPOFORMULARIOID = {0}", id);
                        break;
                }
            }
            return where;
        }
        
    }
}