﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.public_db
{
    public class sys_authority_code
    {
        public int id { get; set; }
        public string code { get; set; }
        public string title { get; set; }
        public int status { get; set; }
        public DateTime add_time { get; set; }
    }
}
