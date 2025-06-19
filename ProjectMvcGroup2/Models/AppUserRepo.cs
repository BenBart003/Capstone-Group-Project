using Microsoft.AspNetCore.Identity;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Data;

namespace ProjectMvcGroup2.Models
{
    public class AppUserRepo : IAppUserRepo
    {
        public readonly ApplicationDbContext _database;
        public readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<AppUser> _userManager;

        public AppUserRepo(ApplicationDbContext database, IHttpContextAccessor httpContextAccessor, UserManager<AppUser> userManager)
        {
            _database = database;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public string GetLoggedInUserId()
        {
            string userId = _userManager.GetUserId(_httpContextAccessor.HttpContext.User);
            return userId;
        }
        public Guest GetGuest(string userId)
        {
            Guest guest = _database.Guest.Find(userId);

            return guest;
        }
    }
}
