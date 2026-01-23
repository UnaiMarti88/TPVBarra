using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using TPVBarra.ApiKonexioak;
using TPVBarra.Modeloak;

namespace TPVBarra
{
    public partial class Logina : Form
    {
        public Logina()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                Color.FromArgb(63, 122, 224),
                Color.FromArgb(44, 90, 160),
                LinearGradientMode.Vertical);

            e.Graphics.FillRectangle(brush, this.ClientRectangle);
        }
        private GraphicsPath GetRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);

            path.CloseFigure();
            return path;
        }
        private void panelLogin_Paint(object sender, PaintEventArgs e)
        {
            int radius = 15;
            var rect = panelLogin.ClientRectangle;

            var path = new GraphicsPath();
            int d = radius * 2;

            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (var brush = new SolidBrush(Color.White))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            String izena = txtErabiltzailea.Text;
            String pasahitza = txtPasahitza.Text;

            ApiLogina loginApi = new ApiLogina();
            var erabiltzailea = await loginApi.LoginAsync(izena, pasahitza);

            if (erabiltzailea == null)
            {
                MessageBox.Show("Erabiltzaile izena edo pasahitza okerra da.");
                return;
            }

            if (erabiltzailea.rola.id != 2)
            {
                MessageBox.Show("Ez duzu aplikaziora sartzeko baimenik hitz egin administratzailearekin");
                return;
            }

            bool txataDu = erabiltzailea.txat;

            MessageBox.Show("Ongi etorri, " + erabiltzailea.erabiltzailea + "!");

            Orria orria = new Orria(erabiltzailea.id,erabiltzailea.erabiltzailea, txataDu);

            this.Hide();
            orria.Show();

        }
    }
}

