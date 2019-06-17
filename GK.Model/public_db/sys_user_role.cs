using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.public_db
{
    public class sys_user_role
    {
        public int id { get; set; }
        public int user_id { get; set; }
        public int role_id { get; set; }
    }
}
