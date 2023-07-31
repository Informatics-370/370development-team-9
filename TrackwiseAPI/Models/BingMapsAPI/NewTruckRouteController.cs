using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace TrackwiseAPI.Models.BingMapsAPI
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewTruckRouteController : ControllerBase
    {
        private readonly NewTruckRouteService _newtruckRouteService;

        public NewTruckRouteController(NewTruckRouteService truckRouteService)
        {
            _newtruckRouteService = truckRouteService ?? throw new ArgumentNullException(nameof(truckRouteService));
        }
        private string GetBingMapsApiKey()
        {
            return "Ah63Z-rLDLN8UftrfVAKYtuQBMSK_EE57L2E7a6NTg5htVdU8gPnn5o7d_Yujc9j";
        }

        [HttpGet]
        [Route("getimage")]
        public async Task<IActionResult> GetTruckRouteMap()
        {
            string startLocation = "Bela-Bela, Limpopo, South Africa";
            string endLocation = "Lephalale, Limpopo, South Africa"; // Corrected the spelling of the location

            try
            {
                string apiKey = GetBingMapsApiKey();

                // Calculate the truck route using the TruckRouteService
                var truckRouteData = await _newtruckRouteService.CalculateTruckRouteAsync(startLocation, endLocation, apiKey);
                var routeResponse = JsonConvert.DeserializeObject<RouteResponse>(truckRouteData);

                // Extract latitude and longitude of start and end waypoints from the route response
                var startLatitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualStart.Coordinates[0];
                var startLongitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualStart.Coordinates[1];
                var endLatitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualEnd.Coordinates[0];
                var endLongitude = routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs.FirstOrDefault()?.ActualEnd.Coordinates[1];

                // Create a Location object to represent the start and end waypoints
                var startWaypoint = new Location
                {
                    Latitude = startLatitude,
                    Longitude = startLongitude
                };
                var endWaypoint = new Location
                {
                    Latitude = endLatitude,
                    Longitude = endLongitude
                };

                // Create a list of waypoints for the Static Map URL (start and end points)
                var waypoints = new List<Location> { startWaypoint, endWaypoint };

                // Generate the Static Map URL
                string staticMapUrl = StaticMapService.GetStaticMapUrl(waypoints, apiKey);

                // Return the URL to the client
                return Ok(staticMapUrl);

            }
            catch (Exception ex)
            {
                // Handle any exceptions, log the error, and return an error response
                return BadRequest(ex.Message);
            }
        }
        /*
        [HttpGet]
        [Route("getimage")]
        public async Task<IActionResult> GetTruckRouteMap()
        {
            string startLocation = "Bela-Bela, Limpopo, South Africa"; 
            string endLocation = "lephalale, Limpopo, South Africa"; 
            try
            {
                string apiKey = GetBingMapsApiKey();

                // Calculate the truck route using the TruckRouteService
                var truckRouteData = await _newtruckRouteService.CalculateTruckRouteAsync(startLocation, endLocation, apiKey);
                var routeResponse = JsonConvert.DeserializeObject<RouteResponse>(truckRouteData);

                var waypoints = new List<Location>();
                foreach (var leg in routeResponse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault()?.RouteLegs)
                {
                    // Extract latitude and longitude of each waypoint (coordinate)
                    var latitude = leg.ActualStart.Coordinates[0];
                    var longitude = leg.ActualStart.Coordinates[1];

                    // Create a Location object to represent the waypoint
                    var waypoint = new Location
                    {
                        Latitude = latitude,
                        Longitude = longitude
                    };

                    waypoints.Add(waypoint);
                }
                var mapCenter = waypoints[0];
                Console.WriteLine($"Map Center: Latitude: {mapCenter.Latitude}, Longitude: {mapCenter.Longitude}");
                foreach (var waypoint in waypoints)
                {
                    Console.WriteLine($"Waypoint: Latitude: {waypoint.Latitude}, Longitude: {waypoint.Longitude}");
                }
                string staticMapUrl = StaticMapService.GetStaticMapUrl(waypoints);

                // Return the URL to the client
                return Ok(staticMapUrl);

            }
            catch (Exception ex)
            {
                // Handle any exceptions, log the error, and return an error response
                return BadRequest(ex.Message);
            }
        }
        */
    }
}
