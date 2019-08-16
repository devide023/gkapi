using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;
namespace GK.Model.public_db
{
    public class sys_user
    {
        [SolrUniqueKey("id")]
        public int id { get; set; }
        public int status { get; set; }
        [SolrField("username")]
        public string username { get; set; }
        [SolrField("usercode")]
        public string usercode { get; set; }
        public int rkey { get; set; }
        public int sex { get; set; }
        public string userpwd { get; set; }
        [SolrField("address")]
        public string address { get; set; }
        [SolrField("tel")]
        public string tel { get; set; }
        public int company_id { get; set; }
        public int department_id { get; set; }
        public int login_way { get; set; }
        public DateTime? login_date { get; set; }
        public DateTime? logout_date { get; set; }
        [SolrField("inputdate")]
        public DateTime? add_time { get; set; }
        [SolrField("modifydate")]
        public DateTime? modify_date { get; set; }
        public DateTime? birthday { get; set; }
        public string headimg { get; set; }
    }
}
