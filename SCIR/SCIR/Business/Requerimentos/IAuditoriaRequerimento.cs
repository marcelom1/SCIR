using SCIR.Business.Cadastros;
using SCIR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCIR.Business.Requerimentos
{
    interface IAuditoriaRequerimento<T>
    {
        AuditoriaServer GerarAuditoria(T requerimentoAntes, T requerimentoDepois, AuditoriaServer.TipoAuditoria tipo);
    }
}
