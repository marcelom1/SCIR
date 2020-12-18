using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Datacontract.Grid
{
    public class ResponseGrid<T>
    {
        public IPagedList<T> Entidades { get; set; }
        public int QuantidadeRegistros { get; set; }

        public ResponseGrid(){
            QuantidadeRegistros = 0;
        }

            
    }
}