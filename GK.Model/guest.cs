using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model
{
    public class guest
    {
        public string guestid { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string enname { get; set; }
        public string sex { get; set; }
        public string birthdate { get; set; }
        public string nationlity { get; set; }
        public string portin { get; set; }
        public string certitype { get; set; }
        public string certino { get; set; }
        public string visatype { get; set; }
        public string visaexpiredate { get; set; }
        public string resonofstay { get; set; }
        public string wherefrom { get; set; }
        public string whereto { get; set; }
        public string address { get; set; }
        public string roomno { get; set; }
        public string lodgeid { get; set; }
        public DateTime lodgedate { get; set; }
        public DateTime leavedate { get; set; }
        public string vip { get; set; }
        public string company { get; set; }
        public string tag1 { get; set; }
        public string remark { get; set; }
        public string operant { get; set; }
        public string classdate { get; set; }
        public string classid { get; set; }
        /// <summary>
        /// 标志	0在店1离店
        /// </summary>
        public string tag { get; set; }
        public string agreeno { get; set; }
        public string receivedby { get; set; }
        public string agentno { get; set; }
        public string bookno { get; set; }
        public string checkno { get; set; }
        public string rcno { get; set; }
        public string cardno { get; set; }
        public string tel { get; set; }
        public string stopdate { get; set; }
        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal tcurr { get; set; }
        /// <summary>
        /// 已结账金额
        /// </summary>
        public decimal jcurr { get; set; }
        public DateTime modifydate { get; set; }
        public string cruisesno { get; set; }
        public string cannet { get; set; }
        public string bktype { get; set; }
        public string nation { get; set; }

    }
}
