using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.public_db
{
    public class sys_menu
    {
        public int id { get; set; }
        public int status { get; set; }
        public int pid { get; set; }
        public string title { get; set; }
        public string code { get; set; }
        public string icon { get; set; }
        public int menutype { get; set; }
        public int seq { get; set; }
        public string path { get; set; }
        public DateTime add_time { get; set; }
    }
}
