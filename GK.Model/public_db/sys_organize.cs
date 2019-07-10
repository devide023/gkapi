using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.public_db
{
    public class sys_organize
    {
        public int id { get; set; }
        public int status { get; set; }
        public int pid { get; set; }
        public int orgtype { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public string tel { get; set; }
        public string fax { get; set; }
        public string email { get; set; }
        public string leader { get; set; }
        public string logo { get; set; }
        public string address { get; set; }
        public DateTime add_time { get; set; }
        public DateTime? modify_time { get; set; }
    }
}
