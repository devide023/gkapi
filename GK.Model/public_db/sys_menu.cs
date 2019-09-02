using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolrNet.Attributes;
namespace GK.Model.public_db
{
    public class sys_menu
    {
        public sys_menu()
        {
            this.entitytype = "sys_menu";
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
        [SolrField("title")]
        public string title { get; set; }
        [SolrField("code")]
        public string code { get; set; }
        [SolrField("codes")]
        public string menucode { get; set; }
        [SolrField("icon")]
        public string icon { get; set; }
        [SolrField("type")]
        public int menutype { get; set; }
        [SolrField("seq")]
        public int seq { get; set; }
        [SolrField("url")]
        public string path { get; set; }
        [SolrField("path")]
        public string viewpath { get; set; }
        [SolrField("inputdate")]
        public DateTime add_time { get; set; }
    }
}
