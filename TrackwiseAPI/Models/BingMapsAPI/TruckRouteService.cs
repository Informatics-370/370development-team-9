namespace TrackwiseAPI.Models.BingMapsAPI
{
    public class TruckRouteService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://dev.virtualearth.net/REST/v1/Routes/Truck";

        public TruckRouteService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<string> CalculateTruckRouteAsync(string startLocation, string endLocation, string apiKey)
        {
            // Construct the URL for truck route calculation
            string url = $"{BaseUrl}?wayPoint.1={Uri.EscapeDataString(startLocation)}&wayPoint.2={Uri.EscapeDataString(endLocation)}&key={apiKey}";

            // Send the HTTP GET request
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            // Read the response content as a string
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                // Handle error scenarios
                throw new HttpRequestException($"Failed to calculate truck route. Status code: {response.StatusCode}");
            }
        }
    }
}
