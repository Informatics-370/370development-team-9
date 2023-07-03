using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrackwiseAPI.Models.Entities
{
    public class Help
    {
        [Key]
        public string Help_ID { get; set; }
        public string Description { get; set; }

        //Foreign Key for Help Category
        public string Help_Category_ID { get; set; }
        [ForeignKey("Help_Category_ID")]
        public HelpCategory HelpCategory { get; set; }
    }
}
