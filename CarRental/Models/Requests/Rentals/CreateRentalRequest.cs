namespace CarRental.ntier.API.Models.Requests.Rentals
{
    public class CreateRentalRequest
    {
        public Guid BookingId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public Guid PickUpLocationId { get; set; }
        public Guid DropOffLocationId { get; set; }
        public decimal InitialMileage { get; set; }
    }
}
