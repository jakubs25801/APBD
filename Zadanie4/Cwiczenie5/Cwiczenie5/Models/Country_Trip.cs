using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cwiczenie5.Data.Models
{

    public class Country_Trip
    {
    [Key] [Column(Order = 1)] public int IdCountry { get; set; }

    [Key] [Column(Order = 2)] public int IdTrip { get; set; }
    
    [ForeignKey("IdCountry")] public Country Country { get; set; }

    [ForeignKey("IdTrip")] public Trip Trip { get; set; }
        
    }
}