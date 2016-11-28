using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class MoveList : UserControl
    {
        public MoveList()
        {
            InitializeComponent();
            mList.Clear();
        }
        public bool appendMove(string m, int c, int idx)
        {
            try
            {
                string entry = " ";
                if (c == 0) entry += Convert.ToString((idx-1)/2 + 1) + ".";
                mList.Text += entry + m;
            }
            catch (Exception any)
            {
                Log.WriteLine("..[MoveList] exception {0}", any.Message);
                return false;
            }
            return true;
        }
    }
}
