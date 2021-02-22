using PagedList;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCIR.DAO
{
    interface IFormulariosDao<T, T2>
    {
        void Insert(T entidade);
        IPagedList<T2> ListGrid(FormatGridUtils<T> request);
        T BuscarPorId(int id);
        T Update(T entidade);
        void Delete(T entidade);
        IList<T> FiltroPorColuna(string coluna, string searchPhrase);
    }
}
