using System.ComponentModel.DataAnnotations;

namespace Cwiczenie5.Data.Models
{
    public class Country
    {

        [Key]
        public int IdCountry { get; set; }

        [Required]
        [MaxLength(120)]
        public string Name { get; set; }

        public IEnumerable<Country_Trip>? Country_Trips { get; set; }
    }
}