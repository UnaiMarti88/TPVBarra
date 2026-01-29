using System.Windows;
using WpfMessageBox = System.Windows.MessageBox;
using TPVBarra.ApiKonexioak;
using System.Net.Http;
using System.Text;
using System.Text.Json;

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
                await GordeLogaAsync($"Login faltsua: {izena}");
                return;
            }

            if (erabiltzailea.rola.id != 2)
            {
                WpfMessageBox.Show("Ez duzu aplikaziora sartzeko baimenik hitz egin administratzailearekin");
                await GordeLogaAsync($"Login baimenik gabe: {izena}");
                return;
            }

            bool txataDu = erabiltzailea.txat;

            WpfMessageBox.Show("Ongi etorri, " + erabiltzailea.erabiltzailea + "!");
            await GordeLogaAsync($"Login ondo eginda: {erabiltzailea.id} - {erabiltzailea.erabiltzailea}");

            var orria = new OrriaWpf(erabiltzailea.id, erabiltzailea.erabiltzailea, txataDu);
            orria.Show();
            Close();
        }

        private async Task GordeLogaAsync(string ekintza)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7236/");

                var log = new
                {
                    Erabiltzailea = 0,
                    Ekintza = ekintza
                };

                var json = JsonSerializer.Serialize(log);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("api/LogKontrollerra/gorde", content);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Errorea loga gordetzean: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Errorea loga bidaltzean: " + ex.Message);
            }
        }
    }
}
