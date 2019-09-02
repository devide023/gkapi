using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;
namespace GK.Model.public_db
{
    public class sys_role
    {
        public sys_role()
        {
            this.entitytype = "sys_role";
        }
        [SolrUniqueKey("uuid")]
        public string uuid { get; set; }
        [SolrField("entitytype")]
        public string entitytype { get; set; }
        [SolrField("id")]
        public int id { get; set; }
        [SolrField("status")]
        public int status { get; set; }
        [SolrField("code")]
        public string code { get; set; }
        [SolrField("title")]
        public string title { get; set; }
        [SolrField("inputdate")]
        public DateTime add_time { get; set; }
    }
}
