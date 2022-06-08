using Models.Reservations;

namespace WebAPI.Hubs
{
    public class LastReservationChangeMessage
    {
        public ChangedReservationEvent NewReservation { get; set; }
    }
}
