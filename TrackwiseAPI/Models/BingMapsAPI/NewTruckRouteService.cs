using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace TrackwiseAPI.Models.BingMapsAPI
{
    public class NewTruckRouteService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://dev.virtualearth.net/REST/v1/Routes/Truck";

        public NewTruckRouteService(HttpClient httpClient)
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
        /*
        public async Task<string> CalculateTruckRouteAsync(string startLocation, string endLocation, string apiKey)
        {
            // Construct the URL for truck route calculation
            string url = $"{BaseUrl}?wayPoint.1={Uri.EscapeDataString(startLocation)}&wayPoint.2={Uri.EscapeDataString(endLocation)}&key={apiKey}";

            try
            {
                // Send the HTTP GET request
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content as a JSON string
                    string jsonResult = await response.Content.ReadAsStringAsync();

                    // Parse the JSON and extract the waypoints from the route calculation result
                    List<Location> waypoints = ParseWaypointsFromJson(jsonResult);

                    //return waypoints;
                    return jsonResult;
                }
                else
                {
                    // Handle error scenarios
                    throw new HttpRequestException($"Failed to calculate truck route. Status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                // For logging, you can use a logging framework like Serilog or NLog
                // For simplicity, we are just printing the error message to the console here
                Console.WriteLine($"Error occurred during truck route calculation: {ex.Message}");

                // Rethrow the exception or return a default value or error response to the caller
                throw;
            }
        }
        */
        /*
        private List<Location> ParseWaypointsFromJson(string jsonResult)
        {
            var parsedResult = JsonConvert.DeserializeObject<JObject>(jsonResult);
            var routeToken = parsedResult.SelectToken("resourceSets[0].resources[0].route.routeLegs[0]");
            if (routeToken == null)
            {
                // Handle the case when route information is missing in the response
                throw new InvalidOperationException("Route information not found in the JSON response.");
            }

            var waypointsArray = routeToken.SelectToken("coordinates")?.ToObject<JArray>();
            if (waypointsArray == null)
            {
                // Handle the case when waypoints are missing in the response
                throw new InvalidOperationException("Waypoints not found in the JSON response.");
            }

            List<Location> waypoints = new List<Location>();
            foreach (var waypoint in waypointsArray)
            {
                double latitude = (double)waypoint[0];
                double longitude = (double)waypoint[1];

                // Handle invalid or missing latitude/longitude values
                if (double.IsNaN(latitude) || double.IsNaN(longitude))
                {
                    // Log or handle the error as needed
                    // For simplicity, we are just skipping the invalid waypoint here
                    continue;
                }

                waypoints.Add(new Location
                {
                    Latitude = latitude,
                    Longitude = longitude
                });
            }

            return waypoints;
        }*/
    }
}