namespace TripApi.Model;

public class Client
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PESEL { get; set; }
    public ICollection<ClientTrip> ClientTrips { get; set; }
}
