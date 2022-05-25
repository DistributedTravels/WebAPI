namespace WebAPI;

public class ReservationInformation
{
    // should be in format "MM-dd-yyyy"
    public string StartDate { get; set; }
    // should be in format "MM-dd-yyyy"
    public string EndDate { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public int Adults { get; set; }
    public int Children_under_3 { get; set; }
    public int Children_under_10 { get; set; }
    public int Children_under_18 { get; set; }
    public int HotelId { get; set; }
    public string HotelName { get; set; }
    public int TransportId { get; set; }
    public string UserId { get; set; } 
    public int Number_of_apartments { get; set; }
    public int Number_of_2_room { get; set; }
    public string Wifi { get; set; }
    public string Breakfast { get; set; }
    public string Transport { get; set; }
    public string PromotionCode { get; set; }
}
