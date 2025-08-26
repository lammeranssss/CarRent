using CarRental.ntier.BLL.Abstractions;
using CarRental.ntier.DAL.Models.Enums;

namespace CarRental.ntier.BLL.Models.Entities
{
    class BllLocationEntity : BaseModel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public ICollection<BllCarEntity> Cars { get; set; } = new List<BllCarEntity>();
        public ICollection<BllRentalEntity> PickUpRentals { get; set; } = new List<BllRentalEntity>();
        public ICollection<BllRentalEntity> DropOffRentals { get; set; } = new List<BllRentalEntity>();

        public int GetAvailableCarsCount()
        {
            return Cars.Count(c => c.CarStatus == CarStatusEnum.Available);
        }

        public bool CanAcceptReturns()
        {
            return true;
        }
    }
}
