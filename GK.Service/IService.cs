using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Service
{
    interface IService<T> where T:class
    {
        int Add(T entry);
        int Del(int id);
        int Update(T entry);
        T Find(int id);
        IEnumerable<T> List();
    }
}
