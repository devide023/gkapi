using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.Parms
{
    public class organizeparm:baseparm
    {
        public string leader { get; set; }
        public string code { get; set; }
        public string orgtype { get; set; }
        public string tel { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string pid { get; set; }
        public string address { get; set; }
    }
}
