using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GK.Service.Helper
{
    public class ToolHelper
    {
        public ToolHelper()
        {

        }

        public string RandCode()
        {
            string code = string.Empty;
            Utils.Tool tool = new Utils.Tool();
            code = tool.RandNum().ToString();
            return code;
        }
    }
}
