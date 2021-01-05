using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public abstract class Requerimento
    {
        public int Id { get; set; }
        public string Protocolo { get; set; }
        public DateTime Abertura { get; set; }
        public DateTime Encerramento { get; set; }
        public string Mensagem { get; set; }

        public int UsuarioRequerenteId { get; set; }
        public Usuario UsuarioRequerente { get; set; }

        public int UsuarioAtendenteId { get; set; }
        public Usuario UsuarioAtendente { get; set; }

        public int StatusRequerimentoId { get; set; }
        public StatusRequerimento StatusRequerimento { get; set; }

        public int TipoRequerimentoId { get; set; }
        public TipoRequerimento TipoRequerimento { get; set; }

        public int TipoFormularioId { get; set; }
        public TipoFormulario TipoFormulario { get; set; }

        public Requerimento()
        {
            UsuarioRequerente = new Usuario();
            UsuarioAtendente = new Usuario();

            StatusRequerimento = new StatusRequerimento();
            TipoRequerimento = new TipoRequerimento();
            TipoFormulario = new TipoFormulario();

        }
    }
}