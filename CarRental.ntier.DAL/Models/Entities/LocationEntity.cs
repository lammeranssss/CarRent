using CarRental.ntier.DAL.Abstractions;

namespace CarRental.ntier.DAL.Models.Entities;
public class LocationEntity : BaseEntity
{
    public string Name { get; set; }

    public string Address { get; set; }

    public string Phone { get; set; }

        public ICollection<CarEntity> Cars { get; set; } = [];
        public ICollection<RentalEntity> PickUpRentals { get; set; } = [];
        public ICollection<RentalEntity> DropOffRentals { get; set; } = [];
    }
}
