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
    public class PapelDao : ICadastrosDao<Papel, Papel>
    {
        public Papel BuscarPorId(int id)
        {
            throw new NotImplementedException();
        }

        public void Delete(Papel entidade)
        {
            throw new NotImplementedException();
        }

        public bool Exist(Papel entidade)
        {
            throw new NotImplementedException();
        }

        public IList<Papel> FiltroPorColuna(string coluna, string searchPhrase)
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
                return contexto.Papel.Where(where).OrderBy(ordenacao).ToList();
            }
        }

        public void Insert(Papel entidade)
        {
            throw new NotImplementedException();
        }

        public IPagedList<Papel> ListGrid(FormatGridUtils request)
        {
            throw new NotImplementedException();
        }

        public int TotalRegistros()
        {
            throw new NotImplementedException();
        }

        public void Update(Papel entidade)
        {
            throw new NotImplementedException();
        }
    }
}