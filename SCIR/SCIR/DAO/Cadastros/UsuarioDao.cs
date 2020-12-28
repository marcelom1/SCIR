﻿using Microsoft.EntityFrameworkCore;
using PagedList;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;

namespace SCIR.DAO.Cadastros
{
    public class UsuarioDao : ICadastrosDao<Usuario, UsuarioGridDC>
    {
        public Usuario BuscarPorId(int id)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Usuario.Include(e => e.Papel).Where(e => e.Id == id).FirstOrDefault();
            }
        }

        public void Delete(Usuario entidade)
        {
            using (var contexto = new ScirContext())
            {
                contexto.Usuario.Remove(entidade);
                contexto.SaveChanges();
            }
        }

        public bool Exist(Usuario entidade)
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Usuario.Contains(entidade);
            }
        }

        public IList<Usuario> FiltroPorColuna(string coluna, string searchPhrase)
        {
            throw new NotImplementedException();
        }

        public void Insert(Usuario entidade)
        {
            using (var context = new ScirContext())
            {
                entidade.Papel = context.Papel.Find(entidade.PapelId);
                context.Usuario.Add(entidade);
                context.SaveChanges();
            }
        }

        public IPagedList<UsuarioGridDC> ListGrid(FormatGridUtils request)
        {
            var where = "";
            if (!string.IsNullOrWhiteSpace(request.SearchPhrase))
            {
                int id = 0;
                if (int.TryParse(request.SearchPhrase, out id))
                    where = string.Format("Id = {0}", id);

                bool ativo = true;
                if (bool.TryParse(request.SearchPhrase, out ativo))
                {
                    if (!string.IsNullOrWhiteSpace(where))
                        where += " OR ";
                    where += string.Format("Ativo = {0} ", ativo);
                }

                if (!string.IsNullOrWhiteSpace(where))
                    where += " OR ";

                where += string.Format("Nome.Contains(\"{0}\")", request.SearchPhrase);
            }
            else
            {
                where = "1=1";
            }

            using (var contexto = new ScirContext())
            {
                if (string.IsNullOrWhiteSpace(request.CampoOrdenacao))
                    request.CampoOrdenacao = "Id asc";

                var listUnidadeCurricular = contexto.Usuario.Include(e => e.Papel).AsNoTracking().Where(where).OrderBy(request.CampoOrdenacao).ToPagedList(request.Current, request.RowCount);

                var lista = new List<UsuarioGridDC>();
                foreach (var item in listUnidadeCurricular)
                {
                    lista.Add(new UsuarioGridDC
                    {
                        Id = item.Id,
                        Ativo = item.Ativo,
                        Nome = item.Nome,
                        Papel = item.Papel.Id + " - " + item.Papel.Nome
                    });
                }
                return lista.ToPagedList(request.Current, request.RowCount);
            }
        }

        public int TotalRegistros()
        {
            using (var contexto = new ScirContext())
            {
                return contexto.Usuario.Count();
            }
        }

        public void Update(Usuario entidade)
        {
            using (var contexto = new ScirContext())
            {
                entidade.Papel = contexto.Papel.Find(entidade.PapelId);
                contexto.Usuario.Update(entidade);
                contexto.SaveChanges();
            }
        }
    }
}