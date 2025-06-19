using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.Models
{
    public interface ILodgingRepo
    {
        List<Lodging> GetAllLodgings();
        Lodging GetLodgingById(int lodgingId);
        List<LodgingDates> GetAllLodgingDates();
        void AddLodgingBooking(LodgingDates newBooking);
    }
}
