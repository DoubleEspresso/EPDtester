using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace epdTester
{
    public class Clock : Timer
    {
        public Clock(bool enabled = true)
        {
            Enabled = enabled;
        }
        public Clock(int interval, bool enabled = true)
        {
            Interval = interval;
            Enabled = enabled;
        }

        public void SetElapsedCB(ElapsedEventHandler e)
        {
            Elapsed += e;
        }
 

    }
}
