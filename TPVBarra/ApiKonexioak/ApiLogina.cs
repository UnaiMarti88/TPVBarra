using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TPVBarra.Modeloak;

namespace TPVBarra.ApiKonexioak
{
    internal class ApiLogina
    {
        private static readonly HttpClient client = new HttpClient();

        public async Task<Erabiltzailea?> LoginAsync(string erabiltzailea, string pasahitza)
        {
            var body = new
            {
                erabiltzailea = erabiltzailea,
                pasahitza = pasahitza
            };
            String json = JsonSerializer.Serialize(body);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
                
            HttpResponseMessage response = await client.PostAsync("http://localhost:7236/api/login", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            String jsonResponse = await response.Content.ReadAsStringAsync();

            var erab = JsonSerializer.Deserialize<Erabiltzailea>(jsonResponse);
            {

                return erab;
            }
        }
    }
}
