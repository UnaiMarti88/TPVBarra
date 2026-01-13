namespace TPVBarra
{
    public partial class ChatKontrollerra : UserControl
    {
        private BezeroChat bezeroa;

        public ChatKontrollerra(string erab)
        {
            InitializeComponent();

            bezeroa = new BezeroChat("192.168.1.205", 50001, erab);

            bezeroa.MezuaJasota += mensaje =>
            {
                if (InvokeRequired)
                    Invoke(new Action(() => lstMezuak.Items.Add(mensaje)));
                else
                    lstMezuak.Items.Add(mensaje);
            };

            btnBidali.Click += (s, e) =>
            {
                if (!string.IsNullOrWhiteSpace(txtMezua.Text))
                {
                    bezeroa.MezuaBidali(txtMezua.Text);
                    txtMezua.Clear();
                }
            };
        }

    }
}
