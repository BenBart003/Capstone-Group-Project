using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.Models
{
    public interface IEquipmentRentalRepo
    {
        public List<EquipmentRental> GetAllEquipmentRentals();
        public EquipmentRental GetEquipmentRentalById(int EquipmentRentalID);
        public List<Rents> GetAllRents();
        public void AddRental(Rents newRental);
    }
}
