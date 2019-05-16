using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK.Model.Parms
{
    public class baseparm
    {
        private int _pagesize = int.MaxValue;
        private int _pageindex = 1;
        public int pagesize {
            get
            {
                return _pagesize;
            }
            set
            {
                _pagesize = value;
            }
        }
        public int pageindex {
            get
            {
                return _pageindex;
            }
            set
            {
                _pageindex = value;
            }
        }
        public int resultcount { get; set; }
        public int id { get; set; }
        public string key { get; set; }
    }
}
