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
        private static readonly HttpClient client;

        static ApiLogina()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            client = new HttpClient(handler);
        }

        public async Task<Erabiltzailea?> LoginAsync(string erabiltzailea, string pasahitza)
        {
            var body = new
            {
                erabiltzailea,
                pasahitza
            };

            String json = JsonSerializer.Serialize(body);

            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await client.PostAsync("https://localhost:7236/api/LoginKontrollera", content);

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            String jsonResponse = await response.Content.ReadAsStringAsync();

            var erab = JsonSerializer.Deserialize<Erabiltzailea>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});

            return erab;
        }
    }
}