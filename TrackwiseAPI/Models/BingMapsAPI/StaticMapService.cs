using System.Text;

namespace TrackwiseAPI.Models.BingMapsAPI
{
    public class StaticMapService
    {
        private const string BingMapsKey = "Ah63Z-rLDLN8UftrfVAKYtuQBMSK_EE57L2E7a6NTg5htVdU8gPnn5o7d_Yujc9j";
        private const string MapType = "Road"; // Change this to Aerial or Birdseye if desired
        private const string MapVersion = "v1"; // Change this to a different version if needed
        /*
        public static string GetStaticMapUrl(List<Location> waypoints)
        {
            // Set the map center to the first waypoint
            Location mapCenter = waypoints[0];

            // Create a list of pushpins for the map
            List<string> pushpins = new List<string>();
            for (int i = 0; i < waypoints.Count; i++)
            {
                pushpins.Add($"wp.{i};{waypoints[i].Latitude},{waypoints[i].Longitude};{i + 1}");
            }

            // Calculate the bounding box to define the map area
            double minLatitude = waypoints.Min(wp => wp.Latitude);
            double maxLatitude = waypoints.Max(wp => wp.Latitude);
            double minLongitude = waypoints.Min(wp => wp.Longitude);
            double maxLongitude = waypoints.Max(wp => wp.Longitude);

            // Add a buffer to the bounding box to ensure it's not too small or too large
            double latitudeBuffer = 0.01; // Adjust this value as needed
            double longitudeBuffer = 0.01; // Adjust this value as needed

            minLatitude -= latitudeBuffer;
            maxLatitude += latitudeBuffer;
            minLongitude -= longitudeBuffer;
            maxLongitude += longitudeBuffer;

            // Build the static map URL
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("https://dev.virtualearth.net/REST/v1/Imagery/Map/");
            urlBuilder.Append(MapType);
            urlBuilder.Append("?mapVersion=");
            urlBuilder.Append(MapVersion);
            urlBuilder.Append("&center=");
            urlBuilder.Append(mapCenter.Latitude);
            urlBuilder.Append(",");
            urlBuilder.Append(mapCenter.Longitude);
            urlBuilder.Append("&pushpins=");
            urlBuilder.Append(string.Join("&pushpins=", pushpins));
            urlBuilder.Append("&mapSize=800,600"); // Set the desired map size (in pixels)

            // Add the adjusted map area bounding box
            urlBuilder.Append("&mapArea=");
            urlBuilder.Append(minLatitude);
            urlBuilder.Append(",");
            urlBuilder.Append(minLongitude);
            urlBuilder.Append(",");
            urlBuilder.Append(maxLatitude);
            urlBuilder.Append(",");
            urlBuilder.Append(maxLongitude);

            // Add the transit route information to the URL
            urlBuilder.Append("&transitRoute=true");

            urlBuilder.Append("&key=");
            urlBuilder.Append(BingMapsKey);

            return urlBuilder.ToString();
        }
        */
        public static string GetStaticMapUrl(List<Location> waypoints, string apiKey)
        {
            // Set the map center to the first waypoint
            Location mapCenter = waypoints[0];

            // Create a list of pushpins for the map
            List<string> pushpins = new List<string>();
            for (int i = 0; i < waypoints.Count; i++)
            {
                // Custom icons 64 and 66 are chosen to display the endpoints which are identified as "1" and "2"
                pushpins.Add($"wp.{i}={waypoints[i].Latitude},{waypoints[i].Longitude};{64 + i};{i + 1}");
            }

            // Build the static map URL
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append("https://dev.virtualearth.net/REST/v1/Imagery/Map/");
            urlBuilder.Append(MapType);
            urlBuilder.Append("/Routes?");
            urlBuilder.Append(string.Join("&", pushpins));
            urlBuilder.Append("&mapSize=800,600"); // Set the desired map size (in pixels)
            urlBuilder.Append("&key=");
            urlBuilder.Append(apiKey);

            return urlBuilder.ToString();
        }
    }
}
    public class Location
    {
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
