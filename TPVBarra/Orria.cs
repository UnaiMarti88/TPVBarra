using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TPVBarra.ApiKonexioak;
using TPVBarra.DTOak;
using TPVBarra.Modeloak;

namespace TPVBarra
{
    public partial class Orria : Form
    {
        private Button mahaiaBotoia;
        private Button komentsalKopuruaBotoia;
        private Button ezabatuEskaeraBotoia;
        private Button kentzekoBotoia;
        private Button sortuEskaeraBotoia;
        private Button eguneratuEskaeraBotoia;
        private int _loginId;
        private string _loginIzena;
        private bool _txat;
        private int? mahaiaIdAukeratua = null;
        private Label lblErabiltzaileaData;
        private System.Windows.Forms.Timer timerDataOrdua;
        private int? eskeraIdAukeratua = null;
        private int? komensalKopurua = null;

        public Orria(int erabiltzaileId, string erabiltzaileIzena, bool txataDu)
        {
            _loginId = erabiltzaileId;
            _loginIzena = erabiltzaileIzena;
            _txat = txataDu;

            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "TPV Barra - Orria Nagusia";

            EguneratuPanelak();
            eguneratuGoikoPanela();

            if (_txat)
            {
                var chat = new ChatKontrollerra(_loginIzena);
                erdiaGoian.Controls.Add(chat);
                chat.Dock = DockStyle.Fill;
            }
            

            // Produktu kentzeko botoia
            kentzekoBotoia = new Button
            {
                Text = "Produktua eskaeratik kendu",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Enabled = false
            };

            // Produktu kentzeko ekintza
            kentzekoBotoia.Click += async (s, e) =>
            {
                if (produktuTaula.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow lerroa in produktuTaula.SelectedRows)
                    {
                        if (!lerroa.IsNewRow)
                        {
                            await GordeLogaAsync($"Produktua kendu da: {lerroa.Cells["ProduktuaId"].Value}");
                            produktuTaula.Rows.Remove(lerroa);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mesedez, hautatu produktu bat kentzeko.");
                }
            };

            eskuinPanela.Controls.Add(kentzekoBotoia);
            eskuinPanela.Controls.SetChildIndex(kentzekoBotoia, 0);

            // --- HONELA EGIN DAITEKE BI KLIK-EZ KENTZEA ---
            produktuTaula.CellDoubleClick += (s, e) =>
            {
                if (e.RowIndex >= 0 && !produktuTaula.Rows[e.RowIndex].IsNewRow)
                {
                    produktuTaula.Rows.RemoveAt(e.RowIndex);
                }
            };

            // Ezabatu eskaera botoia
            ezabatuEskaeraBotoia = new Button
            {
                Name = "ezabatuEskaeraBotoia",
                Text = "Ezabatu eskaera",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Enabled = false
            };

            eskuinPanela.Controls.Add(ezabatuEskaeraBotoia);
            eskuinPanela.Controls.SetChildIndex(ezabatuEskaeraBotoia, 3);

            // Ezabatu eskaera ekintza
            ezabatuEskaeraBotoia.Click += async (s, e) =>
            {
                if (eskeraIdAukeratua == null)
                {
                    MessageBox.Show("Lehenengo eskaera bat kargatu behar duzu ezabatzeko.");
                    return;
                }

                var api = new ApiEskaerak();
                try
                {
                    var erantzuna = await api.EzabatuEskaeraAsync(eskeraIdAukeratua.Value);

                    if (erantzuna.Code == 200)
                    {
                        await GordeLogaAsync($"Eskaera ezabatu da. Eskaera ID: {eskeraIdAukeratua}");
                        MessageBox.Show("Eskaera ezabatu da arrakastaz!");

                        produktuTaula.Rows.Clear();

                        eskeraIdAukeratua = null;
                        komensalKopurua = null;
                        mahaiaIdAukeratua = null;

                        mahaiaBotoia.Enabled = false;
                        komentsalKopuruaBotoia.Enabled = false;
                        ezabatuEskaeraBotoia.Enabled = false;
                        kentzekoBotoia.Enabled = false;
                        eguneratuEskaeraBotoia.Enabled = false;

                        sortuEskaeraBotoia.Enabled = true;

                        ErakutsiBotoiaAukeratuMahaia();
                    }
                    else
                    {
                        MessageBox.Show($"Errorea: {erantzuna.Message}");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Arazoa: " + ex.Message);
                }
            };

            // Sortu eskaera botoia
            sortuEskaeraBotoia = new Button
            {
                Name = "sortuEskaeraBotoia",
                Text = "Sortu eskaera",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            eskuinPanela.Controls.Add(sortuEskaeraBotoia);

            // Eskubiko mahai botoia
            mahaiaBotoia = new Button
            {
                Name = "mahaiaBotoia",
                Text = "Editatu mahaia",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Enabled = false
            };

            eskuinPanela.Controls.Add(mahaiaBotoia);
            eskuinPanela.Controls.SetChildIndex(mahaiaBotoia, 1);

            // Kargatu eskaera botoia
            Button eskearaBotoia = new Button
            {
                Name = "eskearaBotoia",
                Text = "Kargatu eskaera",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            eskuinPanela.Controls.Add(eskearaBotoia);
            eskuinPanela.Controls.SetChildIndex(eskearaBotoia, 2);

            // Kargatu eskaera ekintza
            eskearaBotoia.Click += async (s, e) =>
            {
                var api = new ApiEskaerak();

                eskaeraBotoia.Enabled = false;
                try
                {
                    var erantzunaEskaerak = await api.LortuEskaerakAsync(_loginId);

                    if (erantzunaEskaerak.Code != 200 || erantzunaEskaerak.Datuak.Count == 0)
                    {
                        MessageBox.Show("Ez dago eskaera existitzen.");
                        return;
                    }

                    Form popup = new Form
                    {
                        Text = "Eskaera aukeratu",
                        Size = new Size(400, 200),
                        StartPosition = FormStartPosition.CenterParent
                    };

                    ComboBox combo = new ComboBox
                    {
                        DataSource = erantzunaEskaerak.Datuak,
                        DisplayMember = "Izena",
                        ValueMember = "Id",
                        Dock = DockStyle.Top,
                        DropDownStyle = ComboBoxStyle.DropDownList
                    };

                    Button ok = new Button
                    {
                        Text = "Kargatu",
                        Height = 40,
                        Dock = DockStyle.Bottom
                    };
                    ok.Click += async (sender2, e2) =>
                    {
                        if (combo.SelectedItem is not EskaeraDTO eskaera)
                        {
                            MessageBox.Show("Ez duzu eskaera bat aukeratu.");
                            return;
                        }

                        eskeraIdAukeratua = eskaera.Id;
                        mahaiaIdAukeratua = eskaera.MahaiaId;

                        popup.Close();

                        produktuTaula.Rows.Clear();

                        var erantzunaProduktuak = await api.LortuEskaeraProduktuakAsync(eskeraIdAukeratua.Value);

                        foreach (var p in erantzunaProduktuak.Datuak)
                        {
                            for (int i = 0; i < p.Kantitatea; i++)
                                produktuTaula.Rows.Add(p.ProduktuaId, p.ProduktuaIzena, p.PrezioUnitarioa);
                        }
                        await AktibatuModuMahaiaAsync();

                        mahaiaBotoia.Enabled = true;
                        komentsalKopuruaBotoia.Enabled = true;
                        ezabatuEskaeraBotoia.Enabled = true;
                        kentzekoBotoia.Enabled = true;
                        eguneratuEskaeraBotoia.Enabled = true;

                        sortuEskaeraBotoia.Enabled = false;
                    };

                    popup.Controls.Add(combo);
                    popup.Controls.Add(ok);
                    popup.ShowDialog();

                }finally
                {
                    eskaeraBotoia.Enabled = true;
                }
            };

            // Mahaia editatzeko botoia
            mahaiaBotoia.Click += async (s, e) =>
            {
                await AukeratuMahaiaAsync();
            };

            // Komentsal kopurua botoia
            komentsalKopuruaBotoia = new Button
            {
                Name = "komentsalKopuruaBotoia",
                Text = "Komentsal kopurua",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkSlateBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                Enabled = false
            };

            eskuinPanela.Controls.Add(komentsalKopuruaBotoia);
            eskuinPanela.Controls.SetChildIndex(komentsalKopuruaBotoia, 1);

            // Komentsal kopurua ekintza
            komentsalKopuruaBotoia.Click += async (s, e) =>
            {
                var api = new ApiEskaerak();

                if (mahaiaIdAukeratua == null)
                {
                    MessageBox.Show("Lehenengo mahaia aukeratu behar duzu.");
                    return;
                }

                int mahaiaId = mahaiaIdAukeratua.Value;

                int maxKomensalak;
                try
                {
                    maxKomensalak = await api.LortuMahaiKapasitateaAsync(mahaiaId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Errorea mahaiaren datuak lortzean:\n" + ex.Message);
                    return;
                }

                if (maxKomensalak <= 0)
                {
                    MessageBox.Show("Mahai honek ez du kapazitate egokirik.");
                    return;
                }

                Form inputForm = new Form()
                {
                    Width = 250,
                    Height = 150,
                    FormBorderStyle = FormBorderStyle.FixedDialog,
                    MaximizeBox = false,
                    MinimizeBox = false,
                    ShowIcon = false,
                    Text = "Komentsal kopurua",
                    StartPosition = FormStartPosition.CenterParent
                };

                NumericUpDown numeric = new NumericUpDown()
                {
                    Minimum = 1,
                    Maximum = maxKomensalak,
                    Value = 1,
                    Location = new Point(50, 20),
                    Width = 120
                };
                inputForm.Controls.Add(numeric);

                Button okButton = new Button()
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                    Location = new Point(50, 60),
                    Width = 80
                };
                inputForm.Controls.Add(okButton);

                inputForm.AcceptButton = okButton;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    komensalKopurua = (int)numeric.Value;
                    await GordeLogaAsync($"Komentsal kopurua aukeratua: {komensalKopurua}");
                    MessageBox.Show($"Komentsal kopurua aukeratua: {komensalKopurua}");
                }
            };


            // Sortu eskaera ekintza
            sortuEskaeraBotoia.Click += async (s, e) =>
            {
                var api = new ApiEskaerak();
                var produktuakDict = new Dictionary<int, EskaeraProduktuaDTO>();

                foreach (DataGridViewRow row in produktuTaula.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells["ProduktuaId"].Value == null || row.Cells["Prezioa"].Value == null)
                        continue;

                    var produktuaId = Convert.ToInt32(row.Cells["ProduktuaId"].Value);
                    var prezioa = Convert.ToDecimal(row.Cells["Prezioa"].Value);

                    if (produktuakDict.ContainsKey(produktuaId))
                    {
                        produktuakDict[produktuaId].Kantitatea++;
                    }
                    else
                    {
                        produktuakDict[produktuaId] = new EskaeraProduktuaDTO
                        {
                            ProduktuaId = produktuaId,
                            PrezioUnitarioa = prezioa,
                            Kantitatea = 1
                        };
                    }
                }

                    var produktuak = produktuakDict.Values.ToList();

                if (!produktuak.Any())
                {
                    MessageBox.Show("Ez duzu produkturik aukeratu.");
                    return;
                }

                if (mahaiaIdAukeratua == null || komensalKopurua == null)
                {
                    MessageBox.Show("Lehenengo mahaia eta komentsalak aukeratu behar dituzu.");
                    return;
                }

                var erantzuna = await api.SortuEskaeraAsync(_loginId, produktuak, mahaiaIdAukeratua.Value, komensalKopurua.Value);

                if (erantzuna.Code == 200)
                {
                    await GordeLogaAsync($"Eskaera sortu da. Mahaia: {mahaiaIdAukeratua}, Komentsalak: {komensalKopurua}");
                    MessageBox.Show("Eskaera sortu da arrakastaz!");
                    produktuTaula.Rows.Clear();

                    eskeraIdAukeratua = null;
                    komensalKopurua = null;
                    mahaiaIdAukeratua = null;

                    mahaiaBotoia.Enabled = false;
                    komentsalKopuruaBotoia.Enabled = false;
                    ezabatuEskaeraBotoia.Enabled = false;
                    kentzekoBotoia.Enabled = false;

                    ezkerraBehean.Controls.Clear();
                    erdiaBehean.Controls.Clear();

                    ErakutsiBotoiaAukeratuMahaia();
                }
                else
                {
                    var produktuakStockGabe = erantzuna.Datuak != null && erantzuna.Datuak.Any() ? string.Join(", ", erantzuna.Datuak) : "ezezagunak";
                    MessageBox.Show($"Errorea: {erantzuna.Message}\nProduktuak stock gabe: {produktuakStockGabe}");

                }
            };

            eguneratuEskaeraBotoia = new Button
            {
                Name = "eguneratuEskaeraBotoia",
                Text = "Eguneratu eskaera",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Enabled = false
            };

            eskuinPanela.Controls.Add(eguneratuEskaeraBotoia);
            eskuinPanela.Controls.SetChildIndex(eguneratuEskaeraBotoia, 4);

            // Eguneratu eskaera ekintza
            eguneratuEskaeraBotoia.Click += async (s, e) =>
            {
                if (eskeraIdAukeratua == null)
                {
                    MessageBox.Show("Lehenengo eskaera aukeratu behar duzu.");
                    return;
                }

                var api = new ApiEskaerak();
                var produktuak = new List<EskaeraProduktuaEditatuDTO>();

                foreach (DataGridViewRow row in produktuTaula.Rows)
                {
                    if (row.IsNewRow) continue;

                    if (row.Cells["ProduktuaId"].Value == null || row.Cells["Prezioa"].Value == null)
                        continue;

                    if (!int.TryParse(row.Cells["ProduktuaId"].Value.ToString(), out int produktuaId))
                        continue;

                    produktuak.Add(new EskaeraProduktuaEditatuDTO
                    {
                        ProduktuaId = produktuaId,
                        Kantitatea = 1
                    });
                }

                produktuak = produktuak
                    .GroupBy(p => p.ProduktuaId)
                    .Select(g => new EskaeraProduktuaEditatuDTO
                    {
                        ProduktuaId = g.Key,
                        Kantitatea = g.Sum(x => x.Kantitatea)
                    })
                    .ToList();

                if (!produktuak.Any())
                {
                    MessageBox.Show("Ez duzu produkturik aukeratu.");
                    return;
                }

                var erantzuna = await api.EguneratuEskaeraAsync(eskeraIdAukeratua.Value, produktuak);

                if (erantzuna.Code == 200)
                {
                    await GordeLogaAsync($"Eskaera eguneratu da. Eskaera ID: {eskeraIdAukeratua}");
                    MessageBox.Show("Eskaera eguneratu da arrakastaz!");
                    produktuTaula.Rows.Clear();
                    eskeraIdAukeratua = null;
                    ezabatuEskaeraBotoia.Enabled = false;
                    kentzekoBotoia.Enabled = false;
                    eguneratuEskaeraBotoia.Enabled = false;

                    ezkerraBehean.Controls.Clear();
                    erdiaBehean.Controls.Clear();
                    ErakutsiBotoiaAukeratuMahaia();
                }else if (erantzuna.Code == 400)
                {
                    var produktuakStockGabe = erantzuna.Datuak != null && erantzuna.Datuak.Any() ? string.Join(", ", erantzuna.Datuak) : "ezezagunak";

                    MessageBox.Show(
                        $"Stock arazoa:\n{erantzuna.Message}\n\nProduktuak: {produktuakStockGabe}");

                }
                else if (erantzuna.Code == 404)
                {
                    MessageBox.Show("Eskaera ez da existitzen.");
                }
                else
                {
                    MessageBox.Show("Errore orokorra: " + erantzuna.Message);
                }
                
            };

            EguneratuPanelak();

            this.Resize += (s, e) =>
            {
                if (!IsHandleCreated) return; EguneratuPanelak(); eguneratuGoikoPanela();
            };

            ErakutsiBotoiaAukeratuMahaia();
        }

        private void ErakutsiBotoiaAukeratuMahaia()
        {

            var exist = erdiaBehean.Controls.OfType<Button>().FirstOrDefault(b => b.Name == "erdiaMahaiaBotoia");

            if (exist != null)
            {
                erdiaBehean.Controls.Remove(exist);
            }

            Button mahaiaBeheanBotoia = new Button
            {
                Name = "erdiaMahaiaBotoia",
                Text = "Aukeratu mahaia",
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };

            erdiaBehean.Controls.Add(mahaiaBeheanBotoia);

            mahaiaBeheanBotoia.Width = erdiaBehean.ClientSize.Width;
            mahaiaBeheanBotoia.Height = erdiaBehean.ClientSize.Height;


            erdiaBehean.Resize += (s, e) =>
            {
                if (mahaiaIdAukeratua == null)
                {
                    mahaiaBeheanBotoia.Width = erdiaBehean.ClientSize.Width;
                    mahaiaBeheanBotoia.Height = erdiaBehean.ClientSize.Height;
                }
            };

            mahaiaBeheanBotoia.Click += async (s, e) =>
            {
                await AukeratuMahaiaAsync();
            };

            SortuBehekoInfoPanela();
        }

        private void SortuBehekoInfoPanela()
        {
            Panel behekoPanela = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 60,
                BackColor = Color.FromArgb(240, 240, 240),
                Padding = new Padding(10)
            };

            lblErabiltzaileaData = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 9, FontStyle.Bold)
            };

            behekoPanela.Controls.Add(lblErabiltzaileaData);
            eskuinPanela.Controls.Add(behekoPanela);

            timerDataOrdua = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };

            timerDataOrdua.Tick += (s, e) =>
            {
                lblErabiltzaileaData.Text =
                    $"Erabiltzailea: {_loginIzena}\n{DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            };

            timerDataOrdua.Start();
        }

        private async Task AukeratuMahaiaAsync()
        {
            var api = new ApiMahaiak();
            var mahaiak = await api.LortuMahaiLibreAsync();

            if (mahaiak == null || !mahaiak.Any())
            {
                MessageBox.Show("Ez dago mahai librerik.");
                return;
            }

            Form popup = new Form
            {
                Text = "Mahaia aukeratu",
                Size = new Size(300, 150),
                StartPosition = FormStartPosition.CenterParent
            };

            ComboBox combo = new ComboBox
            {
                DataSource = mahaiak,
                DisplayMember = "Zenbakia",
                ValueMember = "Id",
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            Button ok = new Button
            {
                Text = "Aukeratu",
                Height = 60,
                Dock = DockStyle.Bottom
            };

            ok.Click += async (s, e) =>
            {

                mahaiaIdAukeratua = (int)combo.SelectedValue;
                await GordeLogaAsync($"Mahaia aukeratua: {mahaiaIdAukeratua}");
                popup.Close();

                foreach (Control c in eskuinPanela.Controls)
                {
                    if (c is Button b && b.Name == "mahaiaBotoia")
                    {
                        b.Text = $"Mahaia: {combo.Text}";
                        break;
                    }
                }

                ezkerraBehean.Controls.Clear();
                erdiaBehean.Controls.Clear();

                await KargatuKategoriak();

                if (ezkerraBehean.Controls.Count > 0 && ezkerraBehean.Controls[0] is Button firstCat && firstCat.Tag is KategoriaDTO cat)
                {
                    await KargatuProduktuakAsync(cat.id);
                }

                mahaiaBotoia.Enabled = true;
                komentsalKopuruaBotoia.Enabled = true;
                sortuEskaeraBotoia.Enabled = true;
                kentzekoBotoia.Enabled = true;

            };

            popup.Controls.Add(combo);
            popup.Controls.Add(ok);
            popup.ShowDialog();

        }

        private async Task AktibatuModuMahaiaAsync()
        {
            ezkerraBehean.Controls.Clear();
            erdiaBehean.Controls.Clear();

            await KargatuKategoriak();

            if (ezkerraBehean.Controls.Count > 0 &&
                ezkerraBehean.Controls[0] is Button firstCat &&
                firstCat.Tag is KategoriaDTO cat)
            {
                await KargatuProduktuakAsync(cat.id);
            }
        }
        private void Orria_resize(object sender, EventArgs e)
        {
            EguneratuPanelak();
            eguneratuGoikoPanela();
        }

        private void EguneratuPanelak()
        {
            EguneratuEskuinPanelak();
            EguneratuErdiEskuinPanelak();
            EguneratuGoikoEzkerPanela();
        }

        private async Task KargatuProduktuakAsync(int kategoriaId)
        {
            var api = new ApiProduktuak();
            var produktuak = await api.LortuProduktuakKategoriagatik(kategoriaId);

            erdiaBehean.Controls.Clear();

            if (mahaiaIdAukeratua == null)
            {
                ErakutsiBotoiaAukeratuMahaia();
                return;
            }

            int botoiakIlarako = 4;
            int espazioa = 10;
            int zabaleraPosiblea = erdiaBehean.ClientSize.Width - erdiaBehean.Padding.Horizontal - (espazioa * 2 * botoiakIlarako);
            int botoiZabalera = zabaleraPosiblea / botoiakIlarako;
            int botoiAltuera = 100;

            foreach (var produktua in produktuak)
            {
                Button botoia = new Button
                {
                    Text = $"{produktua.izena}\n{produktua.prezioa:C}\nStock: {produktua.stock_aktuala}",
                    Width = botoiZabalera,
                    Height = botoiAltuera,
                    Margin = new Padding(espazioa),
                    BackColor = Color.LightYellow,
                    Font = new Font("Segoe UI", 10, FontStyle.Bold),
                    Tag = produktua
                };

                botoia.Click += async (s, e) =>
                {
                    produktuTaula.Rows.Add(produktua.id, produktua.izena, produktua.prezioa);
                    await GordeLogaAsync($"Produktua gehitu da: {produktua.id}, Mahaia:" + mahaiaIdAukeratua);
                };

                erdiaBehean.Controls.Add(botoia);
            }
        }

        private async Task KargatuKategoriak()
        {
            var api = new ApiKategoriak();
            var kategoriak = await api.LortuKategoriak();

            ezkerraBehean.Controls.Clear();

            int botoiakIlarako = 3;
            int espazioa = 10;
            int zabaleraPosiblea = ezkerraBehean.ClientSize.Width - ezkerraBehean.Padding.Horizontal - (espazioa * 2 * botoiakIlarako);
            int botoiZabalera = zabaleraPosiblea / botoiakIlarako;

            foreach (var item in kategoriak)
            {
                Button botoia = new Button
                {
                    Text = item.izena,
                    Width = botoiZabalera,
                    Height = 80,
                    Margin = new Padding(espazioa),
                    BackColor = Color.AliceBlue,
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Tag = item
                };

                botoia.Click += async (s, e) => {

                    if(botoia.Tag is KategoriaDTO cat)
                    {
                        await KargatuProduktuakAsync(cat.id);
                    }
                };

                ezkerraBehean.Controls.Add(botoia);
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

                var json = System.Text.Json.JsonSerializer.Serialize(log);
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



        private void EguneratuEskuinPanelak()
        {
            int width = (int)(this.ClientSize.Width * 0.2);
            int height = this.ClientSize.Height;
            int x = this.ClientSize.Width - width;
            int y = 0;

            eskuinPanela.Bounds = new Rectangle(x, y, width, height);
        }

        private void EguneratuErdiEskuinPanelak()
        {
            int widthTotala = this.ClientSize.Width;
            int heightTotala = this.ClientSize.Height;

            int widthEskuragarria = widthTotala - eskuinPanela.Width;
            int altuera = (int)(heightTotala * 0.5);
            int y = heightTotala - altuera;

            int zabaleraKategoriak = (int)(widthEskuragarria * 0.3);
            int zabaleraProduktuak = widthEskuragarria - zabaleraKategoriak;

            ezkerraBehean.Bounds = new Rectangle(0, y, zabaleraKategoriak, altuera);
            erdiaBehean.Bounds = new Rectangle(zabaleraKategoriak, y, zabaleraProduktuak, altuera);
        }

        private void eguneratuGoikoPanela()
        {
            int zabaleraTotala = this.ClientSize.Width;
            int zabaleraEskuragarri = zabaleraTotala - eskuinPanela.Width;
            int altuera = 200;

            int zabaleraTaula = (int)(zabaleraEskuragarri * 0.69);
            int zabaleraChat = zabaleraEskuragarri - zabaleraTaula;

            produktuTaula.Bounds = new Rectangle(0, 0, zabaleraTaula, altuera);
            erdiaGoian.Bounds = new Rectangle(zabaleraTaula, 0, zabaleraChat, altuera);
        }

        private void EguneratuGoikoEzkerPanela()
        {
            int widthTotala = this.ClientSize.Width;
            int heigthTotala = this.ClientSize.Height;

            int width = (int)(widthTotala * 0.55);
            int height = (int)(heigthTotala * 0.5);

            ezkerraGoian.Bounds = new Rectangle(0, 0, width, height);
        }
    }
}
