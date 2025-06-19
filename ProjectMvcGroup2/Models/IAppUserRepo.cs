using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.Models
{
    public interface IAppUserRepo
    {
        public string GetLoggedInUserId();
        public Guest GetGuest(string userId);
    }
}
