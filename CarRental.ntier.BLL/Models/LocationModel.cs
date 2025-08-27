using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Models
{
    public class LocationModel : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public ICollection<CarModel> Cars { get; set; } = [];
        public ICollection<RentalModel> PickUpRentals { get; set; } = [];
        public ICollection<RentalModel> DropOffRentals { get; set; } = [];
    }
}
