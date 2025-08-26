namespace CarRental.ntier.API.Models.Responses.Rentals
{
    public class RentalResponse
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Guid BookingId { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public Guid PickUpLocationId { get; set; }
        public Guid DropOffLocationId { get; set; }
        public decimal InitialMileage { get; set; }
        public decimal FinalMileage { get; set; }
        public decimal FinalPrice { get; set; }
        public string PickUpLocationName { get; set; }
        public string DropOffLocationName { get; set; }
        public decimal MileageUsed { get; set; }
        public bool IsActive { get; set; }
        public bool HasExceededMileageLimit { get; set; }
    }
}
