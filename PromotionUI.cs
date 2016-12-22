using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace epdTester
{
    public partial class PromotionUI : Form
    {
        public PromotionUI()
        {
            InitializeComponent();
        }
        public int selectedPiece = -1;
        public void setButtonImages(int color, Position pos)
        {
            if (color == 0)
            {
                // white
                knightButton.BackgroundImage = Properties.Resources.wn;
                knightButton.BackgroundImageLayout = ImageLayout.Stretch;
                bishopButton.BackgroundImage = Properties.Resources.wb;
                bishopButton.BackgroundImageLayout = ImageLayout.Stretch;
                rookButton.BackgroundImage = Properties.Resources.wr;
                rookButton.BackgroundImageLayout = ImageLayout.Stretch;
                queenButton.BackgroundImage = Properties.Resources.wq;
                queenButton.BackgroundImageLayout = ImageLayout.Stretch;
            }
            else
            {
                // black
                knightButton.BackgroundImage = Properties.Resources.bn;
                knightButton.BackgroundImageLayout = ImageLayout.Stretch;
                bishopButton.BackgroundImage = Properties.Resources.bb;
                bishopButton.BackgroundImageLayout = ImageLayout.Stretch;
                rookButton.BackgroundImage = Properties.Resources.br;
                rookButton.BackgroundImageLayout = ImageLayout.Stretch;
                queenButton.BackgroundImage = Properties.Resources.bq;
                queenButton.BackgroundImageLayout = ImageLayout.Stretch;

            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnFormClosing(e);
        }
        private void knightButton_Click(object sender, EventArgs e)
        {
            selectedPiece = 1; // knight
            Close();
        }
        private void bishopButton_Click(object sender, EventArgs e)
        {
            selectedPiece = 2; // bishop
            Close();
        }
        private void rookButton_Click(object sender, EventArgs e)
        {
            selectedPiece = 3; // rook
            Close();
        }
        private void queenButton_Click(object sender, EventArgs e)
        {
            selectedPiece = 4; // queen
            Close();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) Close();
            return true;
        }
    }
}
