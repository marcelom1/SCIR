using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Datacontract.Grid
{
    public class UnidadeCurricularGridDC
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public string Curso { get; set; }
    }
}