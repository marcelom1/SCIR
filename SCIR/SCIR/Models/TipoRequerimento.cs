using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SCIR.Models
{
    public class TipoRequerimento
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Nome é um campo obrigatório.")]
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public int TipoFormularioId { get; set; }
        public TipoFormulario TipoFormulario { get; set; }
        public string Sigla { get; set; }
        public string SequenciaProtocolo { get; set; }

        public int PrimeiroAtendimentoId { get; set; }
        public Usuario PrimeiroAtendimento { get; set; }

        public TipoRequerimento()
        {
            TipoFormulario = new TipoFormulario();
            PrimeiroAtendimento = new Usuario();
        }
    }
}