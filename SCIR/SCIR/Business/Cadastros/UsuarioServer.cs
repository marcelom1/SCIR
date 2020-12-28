﻿using SCIR.DAO.Cadastros;
using SCIR.Datacontract.Grid;
using SCIR.Models;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Business.Cadastros
{
    public class UsuarioServer
    {
        private UsuarioDao dbUsuario = new UsuarioDao();
        private PapelDao dbPapel = new PapelDao();

        public ConsisteUtils ConsisteNovo(Usuario usuario)
        {
            var consiste = new ConsisteUtils();

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                consiste.Add("O campo Nome não pode ficar em branco", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteExcluir(Usuario usuario)
        {
            var consiste = new ConsisteUtils();

            if (usuario == null)
                consiste.Add("Não foi encontrado o registro para exclusão", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public ConsisteUtils ConsisteAtualizar(Usuario usuario)
        {
            var consiste = new ConsisteUtils();

            var pesquisa = dbUsuario.BuscarPorId(usuario.Id);
            usuario = pesquisa;

            if (pesquisa == null)
                consiste.Add("Não foi encontrado o registro para atualização", ConsisteUtils.Tipo.Inconsistencia);

            return consiste;
        }

        public Usuario Novo(Usuario usuario)
        {
            var consiste = ConsisteNovo(usuario);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbUsuario.Insert(usuario);


            return usuario;
        }

        public Usuario Excluir(Usuario usuario)
        {
            var pesquisa = dbUsuario.BuscarPorId(usuario.Id);
            usuario = pesquisa;

            var consiste = ConsisteExcluir(usuario);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbUsuario.Delete(usuario);

            return usuario;
        }

        public Usuario Atualizar(Usuario usuario)
        {
            var consiste = ConsisteAtualizar(usuario);

            if (consiste.Inconsistencias.Any())
                throw new ArgumentException(consiste.Inconsistencias.ToString());
            else
                dbUsuario.Update(usuario);


            return usuario;
        }

        public Usuario GetEntidade(int id)
        {
            return dbUsuario.BuscarPorId(id);
        }

        public ResponseGrid<UsuarioGridDC> Listar(FormatGridUtils request)
        {
            var response = new ResponseGrid<UsuarioGridDC>();
            response.Entidades = dbUsuario.ListGrid(request);
            response.QuantidadeRegistros = response.Entidades.TotalItemCount;

            return response;
        }

        public IList<Papel> GetFiltroPapelString(string coluna, string searchTerm)
        {
            return dbPapel.FiltroPorColuna(coluna, searchTerm);
        }
    }
}