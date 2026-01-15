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

namespace TPVBarra
{
    public partial class Orria : Form
    {
        private int _loginId;
        private string _loginIzena;
        private int? _mahaiaIdAukeratua = null;

        public Orria(int erabiltzaileId, string erabiltzaileIzena)
        {
            _loginId = erabiltzaileId;
            _loginIzena = erabiltzaileIzena;

            InitializeComponent();
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "TPV Barra - Orria Nagusia";

            EguneratuPanelak();
            eguneratuGoikoPanela();

            //var chat = new ChatKontrollerra(_loginIzena);
            //erdiaGoian.Controls.Add(chat);
            //chat.Dock = DockStyle.Fill;

            Button kentzekoBotoia = new Button
            {
                Text = "Produktu hau kendu",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkRed,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            kentzekoBotoia.Click += (s, e) =>
            {
                if (produktuTaula.SelectedRows.Count > 0)
                {
                    foreach (DataGridViewRow lerroa in produktuTaula.SelectedRows)
                    {
                        if (!lerroa.IsNewRow)
                            produktuTaula.Rows.Remove(lerroa);
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

            eskaeraBotoia = new Button
            {
                Name = "eskaeraBotoia",
                Text = "Sortu eskaera",
                Height = 60,
                Dock = DockStyle.Top,
                BackColor = Color.DarkGreen,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12, FontStyle.Bold)
            };
            eskuinPanela.Controls.Add(eskaeraBotoia);

            // Eskubiko mahai botoia
            Button mahaiaBotoia = new Button
            {
                Name = "mahaiaBotoia",
                Text = "Editatu mahaia",
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.DarkBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            eskuinPanela.Controls.Add(mahaiaBotoia);
            eskuinPanela.Controls.SetChildIndex(mahaiaBotoia, 1);

            Button eskearaBotoia = new Button
            {
                Name = "eskearaBotoia",
                Text = "Kargatu eskaera",
                Height = 50,
                Dock = DockStyle.Top,
                BackColor = Color.DarkOrange,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11, FontStyle.Bold)
            };

            eskuinPanela.Controls.Add(eskearaBotoia);
            eskuinPanela.Controls.SetChildIndex(eskearaBotoia, 2);

            eskearaBotoia.Click += async (s, e) =>
            {
                var api = new ApiEskaerak();
                var erantzunaEskaerak = await api.LortuEskaerakAsync(_loginId);

                if (erantzunaEskaerak.Code != 200 || erantzunaEskaerak.Datuak == null || !erantzunaEskaerak.Datuak.Any())
                {
                    MessageBox.Show(erantzunaEskaerak.Message ?? "Ez dago eskaera existitzen.");
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
                    Dock = DockStyle.Bottom
                };

                ok.Click += async (sender2, e2) =>
                {
                    int eskaeraId = (int)combo.SelectedValue;
                    popup.Close();

                    produktuTaula.Rows.Clear();

                    var erantzunaProduktuak = await api.LortuEskaeraProduktuakAsync(eskaeraId);

                    if (erantzunaProduktuak.Code != 200 || erantzunaProduktuak.Datuak.Count == 0)
                    {
                        MessageBox.Show(erantzunaProduktuak.Message ?? "Errorea produktuak lortzean");
                        return;
                    }

                    foreach (var p in erantzunaProduktuak.Datuak)
                    {
                        produktuTaula.Rows.Add(p.ProduktuaId, p.ProduktuaIzena, p.PrezioUnitarioa);
                    }
                };

                popup.Controls.Add(combo);
                popup.Controls.Add(ok);
                popup.ShowDialog();
            };

            mahaiaBotoia.Click += async (s, e) =>
            {
                await AukeratuMahaiaAsync();
            };

            eskaeraBotoia.Click += async (s, e) =>
            {
                var api = new ApiEskaerak();
                var produktuak = new List<EskaeraProduktuaDTO>();

                foreach (DataGridViewRow row in produktuTaula.Rows)
                {
                    if (row.IsNewRow) continue;

                    var produktuaIdValue = row.Cells["ProduktuaId"].Value;
                    var prezioaValue = row.Cells["Prezioa"].Value;

                    if (produktuaIdValue == null || produktuaIdValue == DBNull.Value)
                    {
                        MessageBox.Show("ProduktuId hutsik dago.");
                        continue;
                    }

                    if (prezioaValue == null || prezioaValue == DBNull.Value)
                    {
                        MessageBox.Show($"Prezioa hutsik dago ProduktuaId = {produktuaIdValue}");
                        continue;
                    }

                    produktuak.Add(new EskaeraProduktuaDTO
                    {
                        ProduktuaId = Convert.ToInt32(row.Cells["ProduktuaId"].Value),
                        PrezioUnitarioa = Convert.ToDecimal(row.Cells["Prezioa"].Value)
                    });
                    
                }

                if (!produktuak.Any())
                {
                    MessageBox.Show("Ez duzu produkturik aukeratu.");
                    return;
                }

                if (_mahaiaIdAukeratua == null)
                {
                    MessageBox.Show("Lehenengo mahaia aukeratu behar duzu.");
                    return;
                }

                int mahaiaId = _mahaiaIdAukeratua.Value;

                try
                {
                    var erantzuna = await api.SortuEskaeraAsync(_loginId, produktuak, mahaiaId);
                    
                    if (erantzuna.Code == 200)
                    {
                        MessageBox.Show("Eskaera sortu da arrakastaz!");
                        produktuTaula.Rows.Clear();
                    }
                    else
                    {
                        var produktuakStockGabe =
                            erantzuna.Datuak != null &&
                            erantzuna.Datuak.Any()
                                ? string.Join(", ", erantzuna.Datuak)
                    :           "ezezagunak";

                        MessageBox.Show(
                            $"Errorea: {erantzuna.Message}\nProduktuak stock gabe: {produktuakStockGabe}"
                        );
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Arazoa: " + ex.Message);
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
            if (erdiaBehean.Controls.OfType<Button>().Any(b => b.Name == "erdiaMahaiaBotoia"))
                return;

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
                if (_mahaiaIdAukeratua == null)
                {
                    mahaiaBeheanBotoia.Width = erdiaBehean.ClientSize.Width;
                    mahaiaBeheanBotoia.Height = erdiaBehean.ClientSize.Height;
                }
            };

            mahaiaBeheanBotoia.Click += async (s, e) =>
            {
                await AukeratuMahaiaAsync();
            };

            
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
                Dock = DockStyle.Bottom
            };

            ok.Click += async (s, e) =>
            {
                _mahaiaIdAukeratua = (int)combo.SelectedValue;
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
            };

            popup.Controls.Add(combo);
            popup.Controls.Add(ok);
            popup.ShowDialog();

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

            if (_mahaiaIdAukeratua == null)
            {
                ErakutsiBotoiaAukeratuMahaia();
            }

            var produktuakBotoiak = erdiaBehean.Controls
                .OfType<Button>()
                .Where(b => b.Tag is ProduktuaDTO)
                .ToList();

            foreach (var b in produktuakBotoiak)
            {
                erdiaBehean.Controls.Remove(b);
            }

            if (_mahaiaIdAukeratua == null)
            {
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
                    Font = new Font("Segoe UI", 10, FontStyle.Bold)
                };

                botoia.Click += (s, e) =>
                {
                    produktuTaula.Rows.Add(produktua.id, produktua.izena, produktua.prezioa);
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
