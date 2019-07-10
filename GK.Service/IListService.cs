using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK.Model.Parms;
namespace GK.Service
{
    interface IListService<T,U> where U:baseparm
    {
        IEnumerable<T> List(U parm, out int recordcount);
    }
}
