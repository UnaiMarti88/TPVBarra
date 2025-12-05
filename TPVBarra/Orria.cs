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

            this.DoubleBuffered = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.WindowState = FormWindowState.Maximized;
            this.Text = "TPV Barra - Orria Nagusia";

            EguneratuPanelak();

            this.Resize += Orria_resize;

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
            int width = (int)(this.ClientSize.Width * 0.2);
            int height = (int)(this.ClientSize.Height);
            int x = this.ClientSize.Width - width;
            int y = 0;

            eskuinPanela.Bounds = new Rectangle(x, y, width, height);        }
    }
}
