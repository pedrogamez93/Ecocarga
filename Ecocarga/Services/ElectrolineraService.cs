using Ecocarga.Models;
using Newtonsoft.Json;

namespace Ecocarga.Services
{ 
public class ElectrolineraService
{
    private readonly HttpClient _httpClient;

    public ElectrolineraService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

        public async Task<ElectrolineraResponse> GetElectrolinerasAsync(int page = 1)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, $"https://backend.electromovilidadenlinea.cl/locations?page={page}");
                request.Headers.Add("User-Agent", "Mozilla/5.0");

                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                var electrolineraResponse = JsonConvert.DeserializeObject<ElectrolineraResponse>(jsonString);

                return electrolineraResponse;
            }
            catch (HttpRequestException httpEx)
            {
                // Log de error HTTP
                Console.WriteLine($"HTTP Request error: {httpEx.Message}");
                throw new ApplicationException("No se pudo obtener la información de las electrolineras.", httpEx);
            }
            catch (Exception ex)
            {
                // Log de cualquier otro tipo de error
                Console.WriteLine($"Error general: {ex.Message}");
                throw new ApplicationException("Ocurrió un error inesperado al obtener la información de las electrolineras.", ex);
            }
        }

    }
}