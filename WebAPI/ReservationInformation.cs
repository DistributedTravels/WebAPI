namespace WebAPI;

public class ReservationInformation
{
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public int Adults { get; set; }
    public int Children_under_3 { get; set; }
    public int Children_under_10 { get; set; }
    public int Children_under_18 { get; set; }
    public int HotelId { get; set; }
    public int TransportId { get; set; }
    /* public Guid userId,*/
    public int Number_of_apartments { get; set; }
    public int Number_of_2_rooms { get; set; }
    public string Wifi { get; set; }
    public string Breakfast { get; set; }
    public string Transport { get; set; }
}
