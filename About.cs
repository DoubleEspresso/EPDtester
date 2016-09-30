using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    public class About
    {
        public static string compilationDate
        {
            get { return "date:unknown"; }
        }
        public static string gitBranch
        {
            get { return "branch:unknown"; }
        }
        public static string gitTag
        {
            get { return "gittag:unknown"; }
        }

    }
}
