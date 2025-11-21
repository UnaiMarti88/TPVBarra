namespace TPVBarra
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // Komponenteen deklarazioa
        private Panel panelLogin;
        private Label lblLogo;
        private TextBox txtErabiltzailea;
        private TextBox txtPasahitza;
        private Button btnLogina;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        public void initializeComponent()
        {
            this.panelLogin = new Panel();
            this.lblLogo = new Label();
            this.txtErabiltzailea = new TextBox();
            this.txtPasahitza = new TextBox();
            this.btnLogina = new Button();
            this.panelLogin.SuspendLayout();
            this.SuspendLayout();

            ´// panelLogin propietateak

            this.panelLogin.BackColor = System.Drawing.Color.White;
            this.panelLogin.Controls.Add(this.lblLogo);
            this.panelLogin.Controls.Add(this.txtErabiltzailea);
            this.panelLogin.Controls.Add(this.txtPasahitza);
            this.panelLogin.Controls.Add(this.btnLogina);
            this.panelLogin.Location = new System.Drawing.Point(175, 100);
            this.panelLogin.Name = "panelLogina";
            this.panelLogin.Size = new System.Drawing.Size(250, 250);
            this.panelLogin.TabIndex = 0;

            // lblLogo propietateak

            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 26F, System.Drawing.FontStyle.Bold);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(63, 122, 224);
            this.lblLogo.Location = new System.Drawing.Point(0, 10);
            this.lblLogo.Name = "lblLogoa";
            this.lblLogo.Size = new System.Drawing.Size(250, 50);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "JAUS";
            this.lblLogo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // txtErabiltzailea propietateak

            this.txtErabiltzailea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtErabiltzailea.Location = new System.Drawing.Point(25, 80);
            this.txtErabiltzailea.Name = "txtErabiltzailea";
            this.txtErabiltzailea.PlaceholderText = "Erabiltzailea";
            this.txtErabiltzailea.Size = new System.Drawing.Size(200, 23);
            this.txtErabiltzailea.TabIndex = 1;

        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";
        }

        #endregion
    }
}
