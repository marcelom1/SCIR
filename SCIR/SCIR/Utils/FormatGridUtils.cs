using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Utils
{
    public class FormatGridUtils<T>
    {
        public int Current { get; set; }
        public int RowCount { get; set; }
        public string CampoOrdenacao { get; set; }
        public string SearchPhrase { get; set; }
        public T Entidade {get; set;}

        public static FormatGridUtils<T> Format(HttpRequestBase Request, string searchPhrase, T Entidade, int current = 1, int rowCount = 10)
        {
            var chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).FirstOrDefault();
            var campoOrdenacao = "";
            if (chave != null)
            {
                var ordenacao = Request[chave];
                var campo = chave.Replace("sort[", string.Empty).Replace("]", string.Empty);

                campoOrdenacao = String.Format("{0} {1}", campo, ordenacao).ToUpper();
            }


            return new FormatGridUtils<T> {
                Current = current,
                RowCount = rowCount,
                CampoOrdenacao = campoOrdenacao,
                SearchPhrase = searchPhrase,
                Entidade = Entidade
            };
        }
    }
}