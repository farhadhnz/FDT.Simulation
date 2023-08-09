using FDT.Simulation.API.DTOs;
using Newtonsoft.Json;

namespace FDT.Simulation.API.SyncDataServices
{
    public interface IManagementDataClient
    {
        Task<DigitalTwinDto> GetDigitalTwinById(int id);
    }

    public class HttpManagementDataClient : IManagementDataClient
    {
        private readonly HttpClient httpClient;
        private readonly IConfiguration configuration;
        

        public HttpManagementDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            this.httpClient = httpClient;
            this.configuration = configuration;
        }
        public async Task<DigitalTwinDto> GetDigitalTwinById(int id)
        {
            //var httpContent = new StringContent(
            //    JsonSerializer.Serialize(),
            //    Encoding.UTF8,
            //    "application/json"
            //    );

            try
            {
                string url = $"{configuration["ManagementServiceUrl"]}/dt/{id}";
                var response = await httpClient.GetAsync(url);

                using (var responseBodyStream = await response.Content.ReadAsStreamAsync())
                using (var streamReader = new StreamReader(responseBodyStream))
                {
                    var responseBody = await streamReader.ReadToEndAsync();

                    // Output the JSON response to the console.
                    Console.WriteLine("JSON Response:");
                    Console.WriteLine(responseBody);

                    var digitalTwin = JsonConvert.DeserializeObject<DigitalTwinDto>(responseBody);

                    // Ensure the digitalTwin object is not null before returning it.
                    if (digitalTwin != null)
                    {
                        return digitalTwin;
                    }
                    else
                    {
                        Console.WriteLine("Error: DigitalTwinDto object is null.");
                        throw new Exception();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new HttpRequestException(ex.Message);
            }
        }
    }
}
