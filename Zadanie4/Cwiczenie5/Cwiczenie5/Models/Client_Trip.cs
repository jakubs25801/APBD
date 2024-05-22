using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cwiczenie5.Data.Models
{
    public class Client_Trip
    {
        [Key] [Column(Order = 1)] public int IdClient { get; set; }

        [Key] [Column(Order = 2)] public int IdTrip { get; set; }

        [Required] public DateTime RegisteredAt { get; set; }

        public DateTime? PaymentDate { get; set; }

        // Klucze obce
        [ForeignKey("IdClient")] public Client Client { get; set; }

        [ForeignKey("IdTrip")] public Trip Trip { get; set; }
    }
}