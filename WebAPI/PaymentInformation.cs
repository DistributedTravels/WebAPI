namespace WebAPI
{
    public class PaymentInformation
    {
        public string Number { get; set; }
        public int CVV { get; set; }
        public string ExpDate { get; set; }
        public string FullName { get; set; }
        public Guid ReservationId { get; set; }
    }
}
