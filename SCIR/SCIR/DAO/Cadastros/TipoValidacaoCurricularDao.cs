using Microsoft.EntityFrameworkCore;
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
    public class TipoValidacaoCurricularDao : ICadastrosDao<TipoValidacaoCurricular, TipoValidacaoCurricular>
    {
        public TipoValidacaoCurricular BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(TipoValidacaoCurricular entidade)
        {
            throw new NotImplementedException();
        }

        public bool Exist(TipoValidacaoCurricular entidade)
        {
            throw new NotImplementedException();
        }

        public IList<TipoValidacaoCurricular> FiltroPorColuna(string coluna, string searchPhrase)
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
                return contexto.TipoValidacaoCurricular.Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public void Insert(TipoValidacaoCurricular entidade)
        {
            throw new NotImplementedException();
        }

        public IPagedList<TipoValidacaoCurricular> ListGrid(FormatGridUtils<TipoValidacaoCurricular> request)
        {
            throw new NotImplementedException();
        }

        public int TotalRegistros()
        {
            throw new NotImplementedException();
        }

        public void Update(TipoValidacaoCurricular entidade)
        {
            throw new NotImplementedException();
        }
    }
}