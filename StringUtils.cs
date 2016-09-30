using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace epdTester
{
    public class StringUtils
    {
        public static string TimeStamp()
        {
            DateTime time = DateTime.Now;
            return string.Format("{0}{1}{2}{3}{4}{5}", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
        }

        public static string LogTimeStamp()
        {
            DateTime time = DateTime.Now;
            return string.Format("{0}{1}{2} {3}.{4}.{5}", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
        }

    }
}
