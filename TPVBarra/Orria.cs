using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TPVBarra
{
    public partial class Orria : Form
    {
        public Orria()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.Text = "TPV Barra - Orria Nagusia";

        }

        private void Orria_resize(object sender, EventArgs e)
        {
            EguneratuPanelak();
        }

        private void EguneratuPanelak()
        {
            EguneratuEskuinPanelak();
        }

        private void EguneratuEskuinPanelak()
        {
            int width = (int)(this.ClientSize.Width * 0.5);
            eskuinPanela.Size = new Size(width, this.ClientSize.Height);
        }
    }
}
