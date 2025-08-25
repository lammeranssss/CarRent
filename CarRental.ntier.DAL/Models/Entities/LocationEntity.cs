using CarRental.ntier.DAL.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRental.ntier.DAL.Models.Entities
{
    [Table("Locations")]
    public class LocationEntity : BaseEntity
    {

        public string Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public ICollection<CarEntity> Cars { get; set; } = new List<CarEntity>();
        public ICollection<RentalEntity> PickUpRentals { get; set; } = new List<RentalEntity>();
        public ICollection<RentalEntity> DropOffRentals { get; set; } = new List<RentalEntity>();
    }
}
