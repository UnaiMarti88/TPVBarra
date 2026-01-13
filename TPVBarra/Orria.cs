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

        public Orria(int erabiltzaileId, string erabiltzaileIzena)
        {
            _loginId = erabiltzaileId;
            _loginIzena = erabiltzaileIzena;

            InitializeComponent();

            var chat = new ChatKontrollerra(_loginIzena);
            erdiaGoian.Controls.Add(chat);
            chat.Dock = DockStyle.Fill;

            EguneratuPanelak();
            eguneratuGoikoPanela();

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
                        Kantitatea = 1,
                        PrezioUnitarioa = Convert.ToDecimal(row.Cells["Prezioa"].Value)
                    });
                    
                }

                if (!produktuak.Any())
                {
                    MessageBox.Show("Ez duzu produkturik aukeratu.");
                    return;
                }

                int mahaiaId = 1;

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
                            erantzuna.ProduktuakStockGabe != null &&
                            erantzuna.ProduktuakStockGabe.Any()
                                ? string.Join(", ", erantzuna.ProduktuakStockGabe)
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

            this.Shown += async (s, e) => { await KargatuKategoriak(); };

            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.WindowState = FormWindowState.Maximized;
            this.Text = "TPV Barra - Orria Nagusia";

            EguneratuPanelak();

            this.Resize += (s, e) =>
            {
                EguneratuPanelak();
                eguneratuGoikoPanela();
            };
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
                    Font = new Font("Segoe UI", 12, FontStyle.Bold)
                };

                botoia.Click += async (s, e) => { await KargatuProduktuakAsync(item.id); };

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

            int zabaleraTaula = (int)(zabaleraEskuragarri * 0.65);
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
