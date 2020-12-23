using PagedList;
using SCIR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCIR.DAO
{
    public interface ICadastrosDao<T,T2>
    {
        void Insert(T entidade);
        IPagedList<T2> ListGrid(FormatGridUtils request);
        int TotalRegistros();
        T BuscarPorId(int id);
        void Update(T entidade);
        void Delete(T entidade);
        bool Exist(T entidade);
    }
}
