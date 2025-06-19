using Microsoft.EntityFrameworkCore;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Data;

namespace ProjectMvcGroup2.Models
{
     public class LodgingRepo : ILodgingRepo
    {
        private readonly ApplicationDbContext _database;

        public LodgingRepo(ApplicationDbContext database)
        {
            _database = database;
        }

        public List<Lodging> GetAllLodgings()
        {
            return _database.Lodging.ToList();
        }

        public Lodging GetLodgingById(int lodgingId)
        {
            return _database.Lodging.Find(lodgingId);
        }

        public List<LodgingDates> GetAllLodgingDates()
        {
            return _database.LodgingDates
                .Include(ld => ld.Lodging)
                .Include(ld => ld.Guest)
                .ToList();
        }

        public void AddLodgingBooking(LodgingDates newBooking)
        {
            _database.LodgingDates.Add(newBooking);
            _database.SaveChanges();
        }
    }
}
