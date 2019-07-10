using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.Parms.Menu
{
    public class menuparm:baseparm
    {
        public string code { get; set; }
        public string name { get; set; }
        public string pid { get; set; }
        public string url { get; set; }
        public int menutype { get; set; }
    }
}
