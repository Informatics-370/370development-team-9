namespace TrackwiseAPI.Models.ViewModels
{
    public class JobVM
    {
        
        public DateTime StartDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Pickup_Location { get; set; }
        public string Dropoff_Location { get; set; }
        public double Total_Weight { get; set; }
        public string Admin_ID { get; set; }
        public string Job_Type_ID { get; set; }
        public string Job_Status_ID { get; set; }

    }
}
