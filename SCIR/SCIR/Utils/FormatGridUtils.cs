using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SCIR.Utils
{
    public class FormatGridUtils
    {
        public int Current { get; set; }
        public int RowCount { get; set; }
        public string CampoOrdenacao { get; set; }
        public string SearchPhrase { get; set; }

        public static FormatGridUtils Format(HttpRequestBase Request, string searchPhrase, int current = 1, int rowCount = 10)
        {
            var chave = Request.Form.AllKeys.Where(k => k.StartsWith("sort")).First();
            var ordenacao = Request[chave];
            var campo = chave.Replace("sort[", string.Empty).Replace("]", string.Empty);

            var campoOrdenacao = String.Format("{0} {1}", campo, ordenacao);

            return new FormatGridUtils {
                Current = current,
                RowCount = rowCount,
                CampoOrdenacao = campoOrdenacao,
                SearchPhrase = searchPhrase
            };
        }
    }
}