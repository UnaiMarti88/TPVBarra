namespace TPVBarra
{
    partial class Orria
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView produktuTaula;
        private Panel ezkerraGoian;
        private Panel eskuinPanela;
        private FlowLayoutPanel ezkerraBehean;
        private FlowLayoutPanel erdiaBehean;
        private Panel erdiaGoian;
        private Button eskaeraBotoia;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            ezkerraGoian = new Panel();
            eskuinPanela = new Panel();
            ezkerraBehean = new FlowLayoutPanel();
            erdiaBehean = new FlowLayoutPanel();
            erdiaGoian = new Panel();
            produktuTaula = new DataGridView();
            eskaeraBotoia = new Button();

            SuspendLayout();

            // ezkerraGoian
            ezkerraGoian.BackColor = Color.LightYellow;
            ezkerraGoian.Name = "ezkerraGoian";
            ezkerraGoian.Height = 200;
            ezkerraGoian.Location = new Point(0, 0);

            // eskuinPanela
            eskuinPanela.BackColor = Color.LightSlateGray;
            eskuinPanela.Name = "eskuinPanela";
            eskuinPanela.Width = 200;
            eskuinPanela.Location = new Point(0, 0);

            // ezkerraBehean
            ezkerraBehean.BackColor = Color.LightBlue;
            ezkerraBehean.Name = "ezkerraBehean";
            ezkerraBehean.FlowDirection = FlowDirection.LeftToRight;
            ezkerraBehean.WrapContents = true;
            ezkerraBehean.AutoScroll = true;

            // erdiaBehean
            erdiaBehean.BackColor = Color.LightGreen;
            erdiaBehean.FlowDirection = FlowDirection.LeftToRight;
            erdiaBehean.WrapContents = true;
            erdiaBehean.AutoScroll = true;

            // erdiaGoian
            erdiaGoian.BackColor = Color.WhiteSmoke;
            erdiaGoian.Height = 200;

            // produktuTaula
            produktuTaula.Name = "produktuTaula";
            produktuTaula.AllowUserToAddRows = false;
            produktuTaula.AllowUserToDeleteRows = false;
            produktuTaula.ReadOnly = true;
            produktuTaula.RowHeadersVisible = false;
            produktuTaula.ColumnHeadersHeight = 35;
            produktuTaula.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            produktuTaula.BorderStyle = BorderStyle.None;
            produktuTaula.CellBorderStyle = DataGridViewCellBorderStyle.None;
            produktuTaula.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            produktuTaula.BackgroundColor = Color.LightYellow;

            produktuTaula.Columns.Add("ProduktuaId", "ID");
            produktuTaula.Columns["ProduktuaId"].Visible = false;
            produktuTaula.Columns.Add("Izena", "Produktua");
            produktuTaula.Columns.Add("Prezioa", "Prezioa");

            ezkerraGoian.Controls.Add(produktuTaula);

            // Orria
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Name = "Orria";
            Text = "TPV Orri nagusia";
            this.WindowState = FormWindowState.Maximized;

            Controls.Add(erdiaGoian);
            Controls.Add(erdiaBehean);
            Controls.Add(ezkerraBehean);
            Controls.Add(ezkerraGoian);
            Controls.Add(eskuinPanela);

            ResumeLayout(false);
            this.DoubleBuffered = true;
        }
        #endregion
    }
}
