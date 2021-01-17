using SCIR.Utils.TipoFormularioUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCIR.Controllers.Interfaces
{
    interface IFormulario
    {
        TipoFormularioUtils GetActionForm();
    }
}
