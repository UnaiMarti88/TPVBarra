namespace TPVBarra
{
    partial class ChatKontrollerra
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.ListBox lstMezuak;
        private System.Windows.Forms.TextBox txtMezua;
        private System.Windows.Forms.Button btnBidali;
        private System.Windows.Forms.Panel apikoPanela;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bezeroa?.Itxi();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            lstMezuak = new ListBox();
            txtMezua = new TextBox();
            btnBidali = new Button();
            apikoPanela = new Panel();
            apikoPanela.SuspendLayout();
            SuspendLayout();
            // 
            // lstMezuak
            // 
            lstMezuak.Dock = DockStyle.Fill;
            lstMezuak.FormattingEnabled = true;
            lstMezuak.Location = new Point(0, 0);
            lstMezuak.Name = "lstMezuak";
            lstMezuak.Size = new Size(1871, 802);
            lstMezuak.TabIndex = 0;
            // 
            // txtMezua
            // 
            txtMezua.Dock = DockStyle.Fill;
            txtMezua.Location = new Point(0, 0);
            txtMezua.Name = "txtMezua";
            txtMezua.Size = new Size(1791, 27);
            txtMezua.TabIndex = 0;
            // 
            // btnBidali
            // 
            btnBidali.Dock = DockStyle.Right;
            btnBidali.Location = new Point(1791, 0);
            btnBidali.Name = "btnBidali";
            btnBidali.Size = new Size(80, 40);
            btnBidali.TabIndex = 1;
            btnBidali.Text = "Bidali";
            // 
            // apikoPanela
            // 
            apikoPanela.Controls.Add(txtMezua);
            apikoPanela.Controls.Add(btnBidali);
            apikoPanela.Dock = DockStyle.Bottom;
            apikoPanela.Location = new Point(0, 802);
            apikoPanela.Name = "apikoPanela";
            apikoPanela.Size = new Size(1871, 40);
            apikoPanela.TabIndex = 1;
            // 
            // ChatKontrollerra
            // 
            Controls.Add(lstMezuak);
            Controls.Add(apikoPanela);
            Name = "ChatKontrollerra";
            Size = new Size(1871, 842);
            apikoPanela.ResumeLayout(false);
            apikoPanela.PerformLayout();
            ResumeLayout(false);
        }
    }
}
