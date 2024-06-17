namespace TripApi.DTOs;

public class ClientTripDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }
    public DateTime? PaymentDate { get; set; }
}