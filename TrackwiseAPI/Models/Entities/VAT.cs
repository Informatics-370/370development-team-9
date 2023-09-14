using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public class VAT
{
    [Key]
    public string VAT_ID { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")] // Specify the decimal format
    public decimal VAT_Amount { get; set; }
}
