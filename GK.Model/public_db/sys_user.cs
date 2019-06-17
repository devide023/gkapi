using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.public_db
{
    public class sys_user
    {
        public int id { get; set; }
        public int status { get; set; }
        public string username { get; set; }
        public string userpwd { get; set; }
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int login_way { get; set; }
        public DateTime? login_date { get; set; }
        public DateTime? logout_date { get; set; }
        public DateTime? add_time { get; set; }
        public DateTime? modify_date { get; set; }
    }
}
