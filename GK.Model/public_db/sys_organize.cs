using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;
namespace GK.Model.public_db
{
    public class sys_organize
    {
        public sys_organize()
        {
            this.entitytype = "sys_organize";
        }
        [SolrUniqueKey("uuid")]
        public string uuid { get; set; }
        [SolrField("entitytype")]
        public string entitytype { get; set; }
        [SolrField("id")]
        public int id { get; set; }
        [SolrField("status")]
        public int status { get; set; }
        [SolrField("pid")]
        public int pid { get; set; }
        public int orgtype { get; set; }
        [SolrField("code")]
        public string code { get; set; }
        [SolrField("title")]
        public string title { get; set; }
        [SolrField("tel")]
        public string tel { get; set; }
        [SolrField("fax")]
        public string fax { get; set; }
        [SolrField("email")]
        public string email { get; set; }
        public string leader { get; set; }
        [SolrField("logo")]
        public string logo { get; set; }
        [SolrField("address")]
        public string address { get; set; }
        [SolrField("inputdate")]
        public DateTime add_time { get; set; }
        [SolrField("modifydate")]
        public DateTime? modify_time { get; set; }
    }
}
