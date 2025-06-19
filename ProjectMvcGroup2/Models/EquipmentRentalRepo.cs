using Microsoft.EntityFrameworkCore;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Data;

namespace ProjectMvcGroup2.Models
{
    public class EquipmentRentalRepo : IEquipmentRentalRepo
    {
        public readonly ApplicationDbContext _database;

        public EquipmentRentalRepo(ApplicationDbContext database)
        {
            _database = database;
        }

        public List<EquipmentRental> GetAllEquipmentRentals()
        {
            return _database.EquipmentRental.ToList();
        }

        public EquipmentRental GetEquipmentRentalById(int EquipmentRentalID)
        {
            return _database.EquipmentRental.Find(EquipmentRentalID);
        }

        public List<Rents> GetAllRents()
        {
            return _database.Rents
            .Include(r => r.Guest)
            .Include(r => r.EquipmentRental)
            .ToList();
        }

        public void AddRental(Rents newRental)
        {
            _database.Rents.Add(newRental);
            _database.SaveChanges();
        }

    }
}
