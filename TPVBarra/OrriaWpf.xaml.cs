using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WpfApplication = System.Windows.Application;
using WpfButton = System.Windows.Controls.Button;
using WpfComboBox = System.Windows.Controls.ComboBox;
using WpfMessageBox = System.Windows.MessageBox;
using WpfTextBox = System.Windows.Controls.TextBox;
using TPVBarra.ApiKonexioak;
using TPVBarra.DTOak;

namespace TPVBarra
{
    public partial class OrriaWpf : Window
    {
        private readonly int _loginId;
        private readonly string _loginIzena;
        private readonly bool _txat;
        private bool _logoutRequested;

        private int? mahaiaIdAukeratua = null;
        private int? eskeraIdAukeratua = null;
        private int? komensalKopurua = null;

        private readonly ObservableCollection<OrderRow> _orderRows = new();
        private DispatcherTimer? _timerDataOrdua;
        
        public OrriaWpf(int erabiltzaileId, string erabiltzaileIzena, bool txataDu)
        {
            _loginId = erabiltzaileId;
            _loginIzena = erabiltzaileIzena;
            _txat = txataDu;

            InitializeComponent();

            lblUsuario.Text = _loginIzena;
            dgEskaera.ItemsSource = _orderRows;
            dgEskaera.MouseDoubleClick += DgEskaera_MouseDoubleClick;

            if (_txat)
            {
                chatHost.Content = new ChatKontrollerraWpf(_loginIzena);
            }
            else
            {
                chatHost.Content = new TextBlock
                {
                    Text = "Txata ez dago aktibatuta",
                    Foreground = System.Windows.Media.Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                };
            }

            EguneratuEgoeraTextua(null);
            BotoiakHasieran();
            ErakutsiBotoiaAukeratuMahaia();
            StartDataTimer();
        }

        private void StartDataTimer()
        {
            _timerDataOrdua = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };

            _timerDataOrdua.Tick += (s, e) =>
            {
                lblErabiltzaileaData.Text = $"{DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            };

            _timerDataOrdua.Start();
        }

        private void DgEskaera_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgEskaera.SelectedItem is OrderRow row)
            {
                KenduProduktua(row);
            }
        }

        private void EguneratuEgoeraTextua(string? egoera)
        {
            var testua = string.IsNullOrWhiteSpace(egoera) ? "-" : egoera;
            lblEgoera.Text = $"Egoera: {testua}";
        }

        private void BotoiakHasieran()
        {
            btnSortuEskaera.IsEnabled = false;
            btnKargatuEskaera.IsEnabled = true;
            btnFaktura.IsEnabled = true;
            btnEguneratuEskaera.IsEnabled = false;
            btnEzabatuEskaera.IsEnabled = false;
            btnKendu.IsEnabled = false;
            btnOrdaindu.IsEnabled = false;
            btnMahaia.IsEnabled = false;
            btnKomentsal.IsEnabled = false;

            eskeraIdAukeratua = null;
            mahaiaIdAukeratua = null;
            komensalKopurua = null;
            EguneratuEgoeraTextua(null);
            _orderRows.Clear();
        }

        private void BotoiakEskaeraAukeratuta()
        {
            btnSortuEskaera.IsEnabled = false;
            btnKargatuEskaera.IsEnabled = true;
            btnFaktura.IsEnabled = true;
            btnEguneratuEskaera.IsEnabled = true;
            btnEzabatuEskaera.IsEnabled = true;
            btnKendu.IsEnabled = true;
            btnOrdaindu.IsEnabled = true;
            btnMahaia.IsEnabled = true;
            btnKomentsal.IsEnabled = true;
        }

        private void BotoiakEskaeraOrdainduta()
        {
            btnSortuEskaera.IsEnabled = true;
            btnKargatuEskaera.IsEnabled = true;
            btnFaktura.IsEnabled = true;
            btnEguneratuEskaera.IsEnabled = false;
            btnEzabatuEskaera.IsEnabled = false;
            btnKendu.IsEnabled = false;
            btnOrdaindu.IsEnabled = false;
            btnMahaia.IsEnabled = false;
            btnKomentsal.IsEnabled = false;

            eskeraIdAukeratua = null;
            mahaiaIdAukeratua = null;
            komensalKopurua = null;
            EguneratuEgoeraTextua(null);
            _orderRows.Clear();
        }

        private void BotoiakProduktuAukeratu()
        {
            btnSortuEskaera.IsEnabled = mahaiaIdAukeratua != null && komensalKopurua != null;
            btnKendu.IsEnabled = true;
        }

        private void BotoiakMahaiaAukeratuta()
        {
            btnSortuEskaera.IsEnabled = true;
            btnKomentsal.IsEnabled = true;
            btnMahaia.IsEnabled = true;
            eskeraIdAukeratua = null;
        }

        private void ErakutsiBotoiaAukeratuMahaia()
        {
            panelCategorias.Children.Clear();
            panelProductos.Children.Clear();
            btnAukeratuMahaia.Visibility = Visibility.Visible;
        }

        private void EzkutatuBotoiaAukeratuMahaia()
        {
            btnAukeratuMahaia.Visibility = Visibility.Collapsed;
        }

        private async Task KargatuProduktuakAsync(int kategoriaId)
        {
            var api = new ApiProduktuak();
            var produktuak = await api.LortuProduktuakKategoriagatik(kategoriaId);

            panelProductos.Children.Clear();

            if (mahaiaIdAukeratua == null)
            {
                ErakutsiBotoiaAukeratuMahaia();
                return;
            }

            EzkutatuBotoiaAukeratuMahaia();

            foreach (var produktua in produktuak)
            {
                var botoia = new System.Windows.Controls.Button
                {
                    Content = $"{produktua.izena}\n{produktua.prezioa:C}\nStocka: {produktua.stock_aktuala}",
                    Width = 180,
                    Height = 100,
                    Margin = new Thickness(8),
                    Background = System.Windows.Media.Brushes.WhiteSmoke,
                    Foreground = System.Windows.Media.Brushes.Black,
                    BorderBrush = System.Windows.Media.Brushes.Gainsboro,
                    BorderThickness = new Thickness(1),
                    Tag = produktua
                };
                botoia.Click += async (s, e) =>
                {
                    if (produktua.stock_aktuala <= 0)
                    {
                        WpfMessageBox.Show("Produktu honek ez du stockik.");
                        return;
                    }

                    var row = _orderRows.FirstOrDefault(r => r.ProduktuaId == produktua.id);
                    if (row != null && row.Kantitatea >= produktua.stock_aktuala)
                    {
                        WpfMessageBox.Show("Ezin da gehiago gehitu: stocka agortuta dago.");
                        return;
                    }

                    GehituProduktua(produktua.id, produktua.izena, produktua.prezioa);
                    await GordeLogaAsync($"Produktua gehitu da: {produktua.id}, Mahaia:" + mahaiaIdAukeratua);
                    BotoiakProduktuAukeratu();
                };

                panelProductos.Children.Add(botoia);
            }
        }

        private async Task KargatuKategoriak()
        {
            var api = new ApiKategoriak();
            var kategoriak = await api.LortuKategoriak();

            panelCategorias.Children.Clear();

            foreach (var item in kategoriak)
            {
                var botoia = new System.Windows.Controls.Button
                {
                    Content = item.izena,
                    Width = 180,
                    Height = 70,
                    Margin = new Thickness(8),
                    Background = System.Windows.Media.Brushes.AliceBlue,
                    Foreground = System.Windows.Media.Brushes.Black,
                    BorderBrush = System.Windows.Media.Brushes.Gainsboro,
                    BorderThickness = new Thickness(1),
                    Tag = item
                };

                botoia.Click += async (s, e) =>
                {
                    if (botoia.Tag is KategoriaDTO cat)
                    {
                        await KargatuProduktuakAsync(cat.id);
                    }
                };

                panelCategorias.Children.Add(botoia);
            }
        }

        private async Task GordeLogaAsync(string ekintza)
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7236/");

                var log = new
                {
                    Erabiltzailea = _loginId,
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

        private async Task AukeratuMahaiaAsync()
        {
            var api = new ApiMahaiak();
            var mahaiak = await api.LortuMahaiLibreAsync();

            if (mahaiak == null || !mahaiak.Any())
            {
                WpfMessageBox.Show("Ez dago mahai librerik.");
                return;
            }

            var eskaeraApi = new ApiEskaerak();
            foreach (var mahaia in mahaiak.Where(m => m.Kapazitatea <= 0))
            {
                try
                {
                    mahaia.Kapazitatea = await eskaeraApi.LortuMahaiKapasitateaAsync(mahaia.Id);
                }
                catch
                {
                    // Huts egiten badu, jatorrizko balioa mantentzen da.
                }
            }

            var selected = ShowMahaiakDialog(mahaiak);
            if (selected == null)
            {
                return;
            }

            mahaiaIdAukeratua = selected.Id;
            await GordeLogaAsync($"Mahaia aukeratua: {mahaiaIdAukeratua}");

            btnMahaia.Content = $"Mahaia: {selected.Zenbakia}";

            var komentsalAukeratua = await AukeratuKomentsalakAsync();
            if (!komentsalAukeratua)
            {
                mahaiaIdAukeratua = null;
                btnMahaia.Content = "Editatu mahaia";
                komensalKopurua = null;
                WpfMessageBox.Show("Komentsalak ez dira gorde. Mahaia aukeratu behar duzu eskaera egiteko.");
                return;
            }

            panelCategorias.Children.Clear();
            panelProductos.Children.Clear();

            await KargatuKategoriak();

            if (panelCategorias.Children.OfType<WpfButton>().FirstOrDefault()?.Tag is KategoriaDTO cat)
            {
                await KargatuProduktuakAsync(cat.id);
            }

            BotoiakMahaiaAukeratuta();
        }

        private async Task AktibatuModuMahaiaAsync()
        {
            panelCategorias.Children.Clear();
            panelProductos.Children.Clear();

            await KargatuKategoriak();

            if (panelCategorias.Children.OfType<WpfButton>().FirstOrDefault()?.Tag is KategoriaDTO cat)
            {
                await KargatuProduktuakAsync(cat.id);
            }
        }

        private void SaioaItxi_Click(object sender, RoutedEventArgs e)
        {
            _logoutRequested = true;
            var login = new LoginaWpf();
            login.Show();
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _timerDataOrdua?.Stop();
            if (!_logoutRequested)
            {
                WpfApplication.Current.Shutdown();
            }
        }

        private void BtnAukeratuMahaia_Click(object sender, RoutedEventArgs e)
        {
            _ = AukeratuMahaiaAsync();
        }

        private async void BtnMahaia_Click(object sender, RoutedEventArgs e)
        {
            await AukeratuMahaiaAsync();
        }

        private async void BtnKomentsal_Click(object sender, RoutedEventArgs e)
        {
            await AukeratuKomentsalakAsync();
        }

        private async Task<bool> AukeratuKomentsalakAsync()
        {
            var api = new ApiEskaerak();

            if (mahaiaIdAukeratua == null)
            {
                WpfMessageBox.Show("Lehenengo mahaia aukeratu behar duzu.");
                return false;
            }

            int maxKomensalak;
            try
            {
                maxKomensalak = await api.LortuMahaiKapasitateaAsync(mahaiaIdAukeratua.Value);
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("Errorea mahaiaren datuak lortzean:\n" + ex.Message);
                return false;
            }

            if (maxKomensalak <= 0)
            {
                WpfMessageBox.Show("Mahai honek ez du kapazitate egokirik.");
                return false;
            }

            var value = ShowKomentsalDialog(1, maxKomensalak);
            if (value == null)
            {
                return false;
            }

            komensalKopurua = value.Value;
            await GordeLogaAsync($"Komentsal kopurua aukeratua: {komensalKopurua}");
            WpfMessageBox.Show($"Komentsal kopurua gordeta: {komensalKopurua}");

            BotoiakMahaiaAukeratuta();
            return true;
        }

        private async void BtnSortuEskaera_Click(object sender, RoutedEventArgs e)
        {
            var api = new ApiEskaerak();
            var produktuak = _orderRows
                .Select(row => new EskaeraProduktuaDTO
                {
                    ProduktuaId = row.ProduktuaId,
                    PrezioUnitarioa = row.Prezioa,
                    Kantitatea = row.Kantitatea
                })
                .ToList();

            if (!produktuak.Any())
            {
                WpfMessageBox.Show("Ez duzu produkturik aukeratu.");
                return;
            }

            if (mahaiaIdAukeratua == null || komensalKopurua == null)
            {
                WpfMessageBox.Show("Lehenengo mahaia eta komentsalak aukeratu behar dituzu.");
                return;
            }

            var erantzuna = await api.SortuEskaeraAsync(_loginId, produktuak, mahaiaIdAukeratua.Value, komensalKopurua.Value);

            if (erantzuna.Code == 200)
            {
                await GordeLogaAsync($"Eskaera sortu da. Mahaia: {mahaiaIdAukeratua}, Komentsalak: {komensalKopurua}");
                WpfMessageBox.Show("Eskaera sortu da arrakastaz!");
                _orderRows.Clear();

                EguneratuEgoeraTextua(null);
                BotoiakHasieran();
                ErakutsiBotoiaAukeratuMahaia();
            }
            else
            {
                var produktuakStockGabe = erantzuna.Datuak != null && erantzuna.Datuak.Any() ? string.Join(", ", erantzuna.Datuak) : "ezezagunak";
                WpfMessageBox.Show($"Errorea: {erantzuna.Message}\nStockik gabe dauden produktuak: {produktuakStockGabe}");
            }
        }

        private async void BtnEguneratuEskaera_Click(object sender, RoutedEventArgs e)
        {
            if (eskeraIdAukeratua == null)
            {
                WpfMessageBox.Show("Lehenengo eskaera aukeratu behar duzu.");
                return;
            }

            var api = new ApiEskaerak();
            var produktuak = _orderRows
                .Select(row => new EskaeraProduktuaEditatuDTO
                {
                    ProduktuaId = row.ProduktuaId,
                    Kantitatea = row.Kantitatea
                })
                .ToList();

            if (!produktuak.Any())
            {
                WpfMessageBox.Show("Ez duzu produkturik aukeratu.");
                return;
            }

            var erantzuna = await api.EguneratuEskaeraAsync(eskeraIdAukeratua.Value, produktuak);

            if (erantzuna.Code == 200)
            {
                await GordeLogaAsync($"Eskaera eguneratu da. Eskaera ID: {eskeraIdAukeratua}");
                WpfMessageBox.Show("Eskaera eguneratu da arrakastaz!");

                var erantzunaProduktuak = await api.LortuEskaeraProduktuakAsync(eskeraIdAukeratua.Value);
                _orderRows.Clear();
                foreach (var p in erantzunaProduktuak.Datuak)
                {
                    _orderRows.Add(new OrderRow
                    {
                        ProduktuaId = p.ProduktuaId,
                        Izena = p.ProduktuaIzena,
                        Prezioa = p.PrezioUnitarioa,
                        Kantitatea = p.Kantitatea
                    });
                }

                var erantzunaEskaerak = await api.LortuEskaerakAsync(_loginId);
                if (erantzunaEskaerak.Code == 200)
                {
                    var eskaera = erantzunaEskaerak.Datuak?.FirstOrDefault(x => x.Id == eskeraIdAukeratua.Value);
                    if (eskaera != null)
                    {
                        EguneratuEgoeraTextua(eskaera.SukaldeaEgoera);
                    }
                }
                BotoiakEskaeraAukeratuta();
            }
            else if (erantzuna.Code == 400)
            {
                var produktuakStockGabe = erantzuna.Datuak != null && erantzuna.Datuak.Any() ? string.Join(", ", erantzuna.Datuak) : "ezezagunak";
                WpfMessageBox.Show($"Stock arazoa:\n{erantzuna.Message}\n\nProduktuak: {produktuakStockGabe}");
            }
            else if (erantzuna.Code == 404)
            {
                WpfMessageBox.Show("Ez da eskaera aurkitu.");
            }
            else
            {
                WpfMessageBox.Show("Errorea: " + erantzuna.Message);
            }
        }

        private async void BtnEzabatuEskaera_Click(object sender, RoutedEventArgs e)
        {
            if (eskeraIdAukeratua == null)
            {
                WpfMessageBox.Show("Lehenengo eskaera bat kargatu behar duzu ezabatzeko.");
                return;
            }

            var api = new ApiEskaerak();
            try
            {
                var erantzuna = await api.EzabatuEskaeraAsync(eskeraIdAukeratua.Value);

                if (erantzuna.Code == 200)
                {
                    await GordeLogaAsync($"Eskaera ezabatu da. Eskaera ID: {eskeraIdAukeratua}");
                    WpfMessageBox.Show("Eskaera ezabatu da arrakastaz!");

                    _orderRows.Clear();
                    BotoiakHasieran();
                    ErakutsiBotoiaAukeratuMahaia();
                }
                else
                {
                    WpfMessageBox.Show($"Errorea: {erantzuna.Message}");
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("Arazoa: " + ex.Message);
            }
        }

        private async void BtnOrdaindu_Click(object sender, RoutedEventArgs e)
        {
            if (eskeraIdAukeratua == null)
            {
                WpfMessageBox.Show("Ez dago eskaerarik aukeratuta.");
                return;
            }

            var api = new ApiEskaerak();
            var erantzuna = await api.OrdainduEskaeraAsync(eskeraIdAukeratua.Value);

            if (erantzuna.Code == 200)
            {
                WpfMessageBox.Show("Eskaera ordaintzera bidali da.");
                await GordeLogaAsync($"Eskaera ordaindu da. Eskaera ID: {eskeraIdAukeratua}, Mahaia: {mahaiaIdAukeratua}");

                BotoiakEskaeraOrdainduta();
            }
            else
            {
                WpfMessageBox.Show("Errorea: " + erantzuna.Message);
            }
        }

        private async void BtnFaktura_Click(object sender, RoutedEventArgs e)
        {
            var api = new ApiEskaerak();
            btnFaktura.IsEnabled = false;

            try
            {
                var erantzunaEskaerak = await api.LortuEskaerakOrdaintzekoAsync();
                if (erantzunaEskaerak.Code != 200 || erantzunaEskaerak.Datuak.Count == 0)
                {
                    WpfMessageBox.Show("Ez dago ordainketarako eskaerarik.");
                    return;
                }

                var selected = ShowEskaerakDialog("Eskaera aukeratu fakturarako", erantzunaEskaerak.Datuak);
                if (selected == null)
                {
                    WpfMessageBox.Show("Ez duzu eskaera bat aukeratu.");
                    return;
                }

                var erantzunaFaktura = await api.SortuFakturaAsync(selected.Id);

                if (erantzunaFaktura.Code == 200)
                {
                    await GordeLogaAsync($"Faktura sortuta. Eskaera: {selected.Id}, Mahaia: {selected.MahaiaId}");
                    WpfMessageBox.Show("Faktura sortu duzu!\n" + string.Join("\n", erantzunaFaktura.Datuak));
                }
                else
                {
                    WpfMessageBox.Show("Arazoa faktura sortzean: " + erantzunaFaktura.Message);
                }
            }
            catch (Exception ex)
            {
                WpfMessageBox.Show("Errorea faktura sortzean: " + ex.Message);
            }
            finally
            {
                btnFaktura.IsEnabled = true;
            }
        }

        private async void BtnKargatuEskaera_Click(object sender, RoutedEventArgs e)
        {
            var api = new ApiEskaerak();
            btnKargatuEskaera.IsEnabled = false;

            try
            {
                var erantzunaEskaerak = await api.LortuEskaerakAsync(_loginId);

                if (erantzunaEskaerak.Code != 200 || erantzunaEskaerak.Datuak.Count == 0)
                {
                    WpfMessageBox.Show("Ez dago eskaerarik.");
                    return;
                }

                var selected = ShowEskaerakDialog("Eskaera aukeratu", erantzunaEskaerak.Datuak);
                if (selected == null)
                {
                    WpfMessageBox.Show("Ez duzu eskaera bat aukeratu.");
                    return;
                }

                eskeraIdAukeratua = selected.Id;
                mahaiaIdAukeratua = selected.MahaiaId;
                komensalKopurua = null;

                var erantzunaProduktuak = await api.LortuEskaeraProduktuakAsync(eskeraIdAukeratua.Value);
                _orderRows.Clear();
                foreach (var p in erantzunaProduktuak.Datuak)
                {
                    _orderRows.Add(new OrderRow
                    {
                        ProduktuaId = p.ProduktuaId,
                        Izena = p.ProduktuaIzena,
                        Prezioa = p.PrezioUnitarioa,
                        Kantitatea = p.Kantitatea
                    });
                }

                EguneratuEgoeraTextua(selected.SukaldeaEgoera);
                btnMahaia.Content = $"Mahaia: {selected.MahaiaId}";

                await AktibatuModuMahaiaAsync();
                BotoiakEskaeraAukeratuta();
            }
            finally
            {
                btnKargatuEskaera.IsEnabled = true;
            }
        }

        private async void BtnKendu_Click(object sender, RoutedEventArgs e)
        {
            if (dgEskaera.SelectedItem is OrderRow row)
            {
                await GordeLogaAsync($"Produktua kendu da: {row.ProduktuaId}");
                KenduProduktua(row);
            }
            else
            {
                WpfMessageBox.Show("Mesedez, hautatu kentzeko produktu bat.");
            }
        }

        private async void BtnBueltaHasiera_Click(object sender, RoutedEventArgs e)
        {
            var logMezua = "Bueltatu hasierara";
            var detaleak = new List<string>();

            if (eskeraIdAukeratua != null)
            {
                detaleak.Add($"Eskaera: {eskeraIdAukeratua}");
            }
            if (mahaiaIdAukeratua != null)
            {
                detaleak.Add($"Mahaia: {mahaiaIdAukeratua}");
            }
            if (komensalKopurua != null)
            {
                detaleak.Add($"Komentsalak: {komensalKopurua}");
            }
            if (_orderRows.Count > 0)
            {
                detaleak.Add($"Produktuak: {_orderRows.Count}");
            }

            if (detaleak.Any())
            {
                logMezua += " - Ezeztuta: " + string.Join(", ", detaleak);
            }

            _orderRows.Clear();
            eskeraIdAukeratua = null;
            mahaiaIdAukeratua = null;
            komensalKopurua = null;

            EguneratuEgoeraTextua(null);
            btnMahaia.Content = "Editatu mahaia";
            panelCategorias.Children.Clear();
            panelProductos.Children.Clear();

            if (_txat)
            {
                chatHost.Content = new ChatKontrollerraWpf(_loginIzena);
            }
            else
            {
                chatHost.Content = new TextBlock
                {
                    Text = "Txata ez dago aktibatuta",
                    Foreground = System.Windows.Media.Brushes.Gray,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Center
                };
            }

            BotoiakHasieran();
            ErakutsiBotoiaAukeratuMahaia();

            await GordeLogaAsync(logMezua);
        }

        private void GehituProduktua(int produktuaId, string izena, decimal prezioa)
        {
            var row = _orderRows.FirstOrDefault(r => r.ProduktuaId == produktuaId);
            if (row == null)
            {
                _orderRows.Add(new OrderRow
                {
                    ProduktuaId = produktuaId,
                    Izena = izena,
                    Prezioa = prezioa,
                    Kantitatea = 1
                });
                return;
            }

            row.Kantitatea++;
        }

        private void KenduProduktua(OrderRow row)
        {
            var selectedIndex = dgEskaera.SelectedIndex;

            if (row.Kantitatea > 1)
            {
                row.Kantitatea--;
                dgEskaera.SelectedItem = row;
                return;
            }

            _orderRows.Remove(row);

            if (_orderRows.Count == 0)
            {
                return;
            }

            if (selectedIndex >= _orderRows.Count)
            {
                selectedIndex = _orderRows.Count - 1;
            }

            dgEskaera.SelectedIndex = selectedIndex;
        }

        private MahaiaDTO? ShowMahaiakDialog(IList<MahaiaDTO> mahaiak)
        {
            var window = new Window
            {
                Title = "Mahaia aukeratu",
                Width = 520,
                Height = 360,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this,
                Background = System.Windows.Media.Brushes.White
            };

            var root = new DockPanel { Margin = new Thickness(16) };
            var header = new TextBlock
            {
                Text = "Aukeratu mahaia",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 12)
            };
            DockPanel.SetDock(header, Dock.Top);
            root.Children.Add(header);

            var wrap = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Left };
            MahaiaDTO? selected = null;

            foreach (var mahaia in mahaiak)
            {
                var btn = new WpfButton
                {
                    Width = 140,
                    Height = 90,
                    Margin = new Thickness(6),
                    Background = System.Windows.Media.Brushes.WhiteSmoke,
                    BorderBrush = System.Windows.Media.Brushes.Gainsboro,
                    BorderThickness = new Thickness(1),
                    Content = $"Mahaia {mahaia.Zenbakia}\nKapazitatea: {mahaia.Kapazitatea}",
                    Tag = mahaia
                };
                btn.Click += (s, e) =>
                {
                    selected = (MahaiaDTO)btn.Tag;
                    window.DialogResult = true;
                };
                wrap.Children.Add(btn);
            }

            var scroll = new ScrollViewer { Content = wrap, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            root.Children.Add(scroll);

            window.Content = root;

            return window.ShowDialog() == true ? selected : null;
        }

        private int? ShowKomentsalDialog(int min, int max)
        {
            int value = min;

            var window = new Window
            {
                Title = "Komentsal kopurua",
                Width = 360,
                Height = 300,
                MinHeight = 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this,
                Background = System.Windows.Media.Brushes.White
            };

            var stack = new StackPanel { Margin = new Thickness(16) };
            stack.Children.Add(new TextBlock
            {
                Text = "Aukeratu komentsal kopurua",
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 12)
            });

            var display = new TextBlock
            {
                Text = value.ToString(),
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 12)
            };

            var controls = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
            var minus = new WpfButton { Content = "-", Width = 48, Height = 40, Margin = new Thickness(6) };
            var plus = new WpfButton { Content = "+", Width = 48, Height = 40, Margin = new Thickness(6) };

            minus.Click += (s, e) =>
            {
                if (value > min) value--;
                display.Text = value.ToString();
            };
            plus.Click += (s, e) =>
            {
                if (value < max) value++;
                display.Text = value.ToString();
            };

            controls.Children.Add(minus);
            controls.Children.Add(plus);

            var slider = new Slider
            {
                Minimum = min,
                Maximum = max,
                Value = value,
                TickFrequency = 1,
                IsSnapToTickEnabled = true,
                Margin = new Thickness(0, 8, 0, 12)
            };
            slider.ValueChanged += (s, e) =>
            {
                value = (int)slider.Value;
                display.Text = value.ToString();
            };

            var buttons = new StackPanel
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Right,
                Margin = new Thickness(0, 8, 0, 0)
            };

            var cancel = new WpfButton
            {
                Content = "Utzi",
                Height = 36,
                Width = 110,
                Margin = new Thickness(0, 0, 8, 0),
                Background = System.Windows.Media.Brushes.LightGray,
                Foreground = System.Windows.Media.Brushes.Black,
                BorderThickness = new Thickness(0)
            };
            cancel.Click += (s, e) => window.DialogResult = false;

            var ok = new WpfButton
            {
                Content = "Gorde",
                Height = 36,
                Width = 110,
                Background = System.Windows.Media.Brushes.DodgerBlue,
                Foreground = System.Windows.Media.Brushes.White,
                BorderThickness = new Thickness(0)
            };
            ok.Click += (s, e) => window.DialogResult = true;

            buttons.Children.Add(cancel);
            buttons.Children.Add(ok);

            stack.Children.Add(display);
            stack.Children.Add(controls);
            stack.Children.Add(slider);
            stack.Children.Add(buttons);

            window.Content = stack;

            return window.ShowDialog() == true ? value : null;
        }

        private EskaeraDTO? ShowEskaerakDialog(string title, IList<EskaeraDTO> eskaerak)
        {
            var window = new Window
            {
                Title = title,
                Width = 520,
                Height = 360,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                ResizeMode = ResizeMode.NoResize,
                Owner = this,
                Background = System.Windows.Media.Brushes.White
            };

            var root = new DockPanel { Margin = new Thickness(16) };
            var header = new TextBlock
            {
                Text = title,
                FontSize = 16,
                FontWeight = FontWeights.SemiBold,
                Margin = new Thickness(0, 0, 0, 12)
            };
            DockPanel.SetDock(header, Dock.Top);
            root.Children.Add(header);

            var wrap = new WrapPanel { HorizontalAlignment = System.Windows.HorizontalAlignment.Left };
            EskaeraDTO? selected = null;

            foreach (var eskaera in eskaerak)
            {
                var btn = new WpfButton
                {
                    Width = 200,
                    Height = 90,
                    Margin = new Thickness(6),
                    Background = System.Windows.Media.Brushes.WhiteSmoke,
                    BorderBrush = System.Windows.Media.Brushes.Gainsboro,
                    BorderThickness = new Thickness(1),
                    Content = $"{eskaera.Izena}\nMahaia: {eskaera.MahaiaId}\n{eskaera.Data}",
                    Tag = eskaera
                };
                btn.Click += (s, e) =>
                {
                    selected = (EskaeraDTO)btn.Tag;
                    window.DialogResult = true;
                };
                wrap.Children.Add(btn);
            }

            var scroll = new ScrollViewer { Content = wrap, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            root.Children.Add(scroll);

            window.Content = root;

            return window.ShowDialog() == true ? selected : null;
        }
    }

    public class OrderRow : INotifyPropertyChanged
    {
        private int _kantitatea = 1;

        public int ProduktuaId { get; set; }
        public string Izena { get; set; } = string.Empty;
        public decimal Prezioa { get; set; }

        public int Kantitatea
        {
            get => _kantitatea;
            set
            {
                if (_kantitatea == value) return;
                _kantitatea = value;
                OnPropertyChanged(nameof(Kantitatea));
                OnPropertyChanged(nameof(Guztira));
            }
        }

        public decimal Guztira => Prezioa * Kantitatea;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
