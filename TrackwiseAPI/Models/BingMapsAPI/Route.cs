using Newtonsoft.Json;

namespace TrackwiseAPI.Models.BingMapsAPI
{
    public class Route
    {
        public double TravelDistance { get; set; }
        public double TravelDuration { get; set; }

        public Waypoint ActualStart { get; set; }
        public Waypoint ActualEnd { get; set; }

    }
    public class Waypoint
    {
        public double[] Coordinates { get; set; }

        // You might have other properties related to the waypoint here
    }
}
