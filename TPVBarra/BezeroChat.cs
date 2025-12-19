using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace TPVBarra
{
    public class BezeroChat
    {
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread hiloEntzuten;
        private bool konektatuta = false;
        private string erabIzena;

        public event Action<string> MezuaJasota;

        public BezeroChat(string host, int portua, string erabiltzailea)
        {
            erabIzena = erabiltzailea;

            try
            {
                client = new TcpClient(host, portua);
                reader = new StreamReader(client.GetStream());
                writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                konektatuta = true;

                hiloEntzuten = new Thread(Entzun);
                hiloEntzuten.IsBackground = true;
                hiloEntzuten.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arazoa txatera konektatzean: " + ex.Message);
            }
        }

        private void Entzun()
        {
            try
            {
                string mezua;
                while (konektatuta && (mezua = reader.ReadLine()) != null)
                {
                    MezuaJasota?.Invoke(mezua);
                }
            }
            catch { }
        }

        public void MezuaBidali(string mezua)
        {
            if (!konektatuta) return;
            writer.WriteLine($"{erabIzena}: {mezua}");
        }

        public void Itxi()
        {
            konektatuta = false;
            client?.Close();
        }
    }
}
