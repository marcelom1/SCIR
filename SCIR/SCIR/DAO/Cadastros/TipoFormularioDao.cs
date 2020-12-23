using PagedList;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SCIR.DAO.Cadastros
{
    public class TipoFormularioDao : ICadastrosDao<TipoFormulario, TipoFormulario>
    {
        public TipoFormulario BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TipoFormulario entidade)
        {
            throw new NotImplementedException();
        }

        public bool Exist(TipoFormulario entidade)
        {
            throw new NotImplementedException();
        }

        public IList<TipoFormulario> FiltroPorColuna(string coluna, string searchPhrase)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(searchPhrase))
            {
                where += string.Format(coluna + ".Contains(\"{0}\")", searchPhrase);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                var ordenacao = coluna + " ASC";
                return contexto.TipoFormulario.Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public void Insert(TipoFormulario entidade)
        {
            throw new NotImplementedException();
        }

        public IPagedList<TipoFormulario> ListGrid(FormatGridUtils request)
        {
            throw new NotImplementedException();
        }

        public int TotalRegistros()
        {
            throw new NotImplementedException();
        }

        public void Update(TipoFormulario entidade)
        {
            throw new NotImplementedException();
        }
    }
}