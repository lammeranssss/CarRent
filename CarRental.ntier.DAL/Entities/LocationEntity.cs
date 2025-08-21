using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.ntier.DAL.Entities
{
    public class LocationEntity
    {
        public Guid LocationId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }


        public ICollection<CarEntity> Cars { get; set; }
        public ICollection<RentalEntity> PickUpRentals { get; set; }
        public ICollection<RentalEntity> DropOffRentals { get; set; }
    }
}