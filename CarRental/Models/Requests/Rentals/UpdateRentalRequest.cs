namespace CarRental.ntier.API.Models.Requests.Rentals
{
    public class UpdateRentalRequest
    {
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public Guid PickUpLocationId { get; set; }
        public Guid DropOffLocationId { get; set; }
        public decimal FinalMileage { get; set; }
    }
}
