

namespace TPVBarra
{
    partial class Logina
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelLogin;
        private Label lblLogo;
        private TextBox txtErabiltzailea;
        private TextBox txtPasahitza;
        private Button btnLogina;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            panelLogin = new Panel();
            lblLogo = new Label();
            txtErabiltzailea = new TextBox();
            txtPasahitza = new TextBox();
            btnLogina = new Button();
            panelLogin.SuspendLayout();
            SuspendLayout();
            // 
            // panelLogin
            // 
            panelLogin.BackColor = Color.White;
            panelLogin.Controls.Add(lblLogo);
            panelLogin.Controls.Add(txtErabiltzailea);
            panelLogin.Controls.Add(txtPasahitza);
            panelLogin.Controls.Add(btnLogina);
            panelLogin.Location = new Point(200, 133);
            panelLogin.Margin = new Padding(3, 4, 3, 4);
            panelLogin.Name = "panelLogin";
            panelLogin.Size = new Size(286, 333);
            panelLogin.TabIndex = 0;
            panelLogin.Paint += panelLogin_Paint;
            // 
            // lblLogo
            // 
            lblLogo.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
            lblLogo.ForeColor = Color.FromArgb(63, 122, 224);
            lblLogo.Location = new Point(0, 13);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(286, 67);
            lblLogo.TabIndex = 0;
            lblLogo.Text = "JAUS";
            lblLogo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtErabiltzailea
            // 
            txtErabiltzailea.BorderStyle = BorderStyle.FixedSingle;
            txtErabiltzailea.Location = new Point(29, 107);
            txtErabiltzailea.Margin = new Padding(3, 4, 3, 4);
            txtErabiltzailea.Name = "txtErabiltzailea";
            txtErabiltzailea.PlaceholderText = "Erabiltzailea";
            txtErabiltzailea.Size = new Size(229, 27);
            txtErabiltzailea.TabIndex = 1;
            // 
            // txtPasahitza
            // 
            txtPasahitza.BorderStyle = BorderStyle.FixedSingle;
            txtPasahitza.Location = new Point(29, 160);
            txtPasahitza.Margin = new Padding(3, 4, 3, 4);
            txtPasahitza.Name = "txtPasahitza";
            txtPasahitza.PlaceholderText = "Pasahitza";
            txtPasahitza.Size = new Size(228, 27);
            txtPasahitza.TabIndex = 2;
            txtPasahitza.UseSystemPasswordChar = true;
            // 
            // btnLogina
            // 
            btnLogina.BackColor = Color.FromArgb(63, 122, 224);
            btnLogina.FlatAppearance.BorderSize = 0;
            btnLogina.FlatAppearance.MouseDownBackColor = Color.FromArgb(44, 90, 160);
            btnLogina.FlatStyle = FlatStyle.Flat;
            btnLogina.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnLogina.ForeColor = Color.White;
            btnLogina.Location = new Point(57, 227);
            btnLogina.Margin = new Padding(3, 4, 3, 4);
            btnLogina.Name = "btnLogina";
            btnLogina.Size = new Size(171, 47);
            btnLogina.TabIndex = 3;
            btnLogina.Text = "Saioa Hasi";
            btnLogina.UseVisualStyleBackColor = false;
            btnLogina.Click += btnLogin_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(686, 600);
            Controls.Add(panelLogin);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FormLogin";
            panelLogin.ResumeLayout(false);
            panelLogin.PerformLayout();
            ResumeLayout(false);
        }
    }
}
