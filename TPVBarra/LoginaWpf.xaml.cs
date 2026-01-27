using System.Windows;
using WpfMessageBox = System.Windows.MessageBox;
using TPVBarra.ApiKonexioak;

namespace TPVBarra
{
    public partial class LoginaWpf : Window
    {
        public LoginaWpf()
        {
            InitializeComponent();
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var izena = txtErabiltzailea.Text;
            var pasahitza = txtPasahitza.Password;

            var loginApi = new ApiLogina();
            var erabiltzailea = await loginApi.LoginAsync(izena, pasahitza);

            if (erabiltzailea == null)
            {
                WpfMessageBox.Show("Erabiltzaile izena edo pasahitza okerra da.");
                return;
            }

            if (erabiltzailea.rola.id != 2)
            {
                WpfMessageBox.Show("Ez duzu aplikaziora sartzeko baimenik hitz egin administratzailearekin");
                return;
            }

            bool txataDu = erabiltzailea.txat;

            WpfMessageBox.Show("Ongi etorri, " + erabiltzailea.erabiltzailea + "!");

            var orria = new OrriaWpf(erabiltzailea.id, erabiltzailea.erabiltzailea, txataDu);
            orria.Show();
            Close();
        }
    }
}
