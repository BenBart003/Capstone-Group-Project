using ProjectLibraryGroup2;

namespace ProjectMvcGroup2.ViewModels
{
    public class SearchRentsViewModel
    {
        public string? Email { get; set; }
        public List<Rents> RentsSearchResult { get; set; }
    }
}
