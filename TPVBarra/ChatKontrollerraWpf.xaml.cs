using System.Windows;
using System.Windows.Controls;

namespace TPVBarra
{
    public partial class ChatKontrollerraWpf : System.Windows.Controls.UserControl
    {
        private readonly BezeroChat _bezeroa;

        public ChatKontrollerraWpf(string erab)
        {
            InitializeComponent();

            _bezeroa = new BezeroChat("192.168.2.103", 50001, erab);
            _bezeroa.MezuaJasota += mensaje =>
            {
                Dispatcher.Invoke(() => lstMezuak.Items.Add(mensaje));
            };
        }

        private void BtnBidali_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMezua.Text))
            {
                _bezeroa.MezuaBidali(txtMezua.Text);
                txtMezua.Clear();
            }
        }
    }
}
