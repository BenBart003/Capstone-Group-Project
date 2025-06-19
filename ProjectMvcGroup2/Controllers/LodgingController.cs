using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.Services;
using ProjectMvcGroup2.ViewModels;

namespace ProjectMvcGroup2.Controllers
{
    public class LodgingController : Controller
    {
        private readonly ILodgingRepo _lodgingRepo;
        private readonly IAppUserRepo _appUserRepo;
        private readonly ILodgingEmailSender _emailSender;

        public LodgingController(ILodgingRepo lodgingRepo, IAppUserRepo appUserRepo, ILodgingEmailSender emailSender)
        {
            _lodgingRepo = lodgingRepo;
            _appUserRepo = appUserRepo;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult SearchLodging()
        {
            var viewModel = new SearchLodgingViewModel
            {
                LodgingSearchResult = _lodgingRepo.GetAllLodgingDates()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchLodging(SearchLodgingViewModel viewModel)
        {
            var allBookings = _lodgingRepo.GetAllLodgingDates();

            if (viewModel.ShowAll)
            {
                viewModel.LodgingSearchResult = allBookings;
            }
            else
            {
                var filtered = allBookings.AsQueryable();

                if (viewModel.CheckInDate.HasValue && viewModel.CheckOutDate.HasValue)
                {
                    filtered = filtered.Where(ld =>
                        ld.CheckInDate >= viewModel.CheckInDate.Value &&
                        ld.CheckOutDate <= viewModel.CheckOutDate.Value);
                }

                if (!string.IsNullOrEmpty(viewModel.GuestEmail))
                {
                    filtered = filtered.Where(ld =>
                        ld.Guest.Email.Contains(viewModel.GuestEmail, StringComparison.OrdinalIgnoreCase));
                }

                viewModel.LodgingSearchResult = filtered.ToList();
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Guest")]
        [HttpGet]
        public IActionResult CreateLodging()
        {
            var viewModel = new CreateLodgingViewModel
            {
                SearchResults = _lodgingRepo.GetAllLodgings()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        public IActionResult CreateLodging(CreateLodgingViewModel viewModel)
        {
            var allLodgings = _lodgingRepo.GetAllLodgings();
            var allBookings = _lodgingRepo.GetAllLodgingDates();

            if (viewModel.LodgingID != 0)
            {
                string userId = _appUserRepo.GetLoggedInUserId();
                Guest guest = _appUserRepo.GetGuest(userId);
                Lodging selectedLodging = _lodgingRepo.GetLodgingById(viewModel.LodgingID);

                // Prevent double-booking
                bool isBooked = allBookings.Any(b =>
                    b.Lodging.LodgingID == viewModel.LodgingID &&
                    !(viewModel.CheckOutDate < b.CheckInDate || viewModel.CheckInDate > b.CheckOutDate)
                );

                if (viewModel.CheckOutDate <= viewModel.CheckInDate)
                {
                    ModelState.AddModelError("", "Check-out must be after check-in.");
                }

                else if (isBooked)
                {
                    ModelState.AddModelError("", "This room is already booked for the selected dates.");
                }
                else
                {
                    LodgingDates newBooking = new LodgingDates(guest, selectedLodging, viewModel.CheckInDate, viewModel.CheckOutDate);
                    _lodgingRepo.AddLodgingBooking(newBooking);

                    ViewBag.SuccessMessage = $"Successfully booked Room {selectedLodging.RoomNumber} from {viewModel.CheckInDate} to {viewModel.CheckOutDate}.";

                    // Send email
                    _emailSender.SendLodgingEmail(
                        controllerAndMethod: "SeeAllPurchases/FindAllPurchases",
                        email: guest.Email,
                        subject: "Your lodging reservation is confirmed",
                        htmlMessage: $"Your booking for Room {selectedLodging.RoomNumber} from {viewModel.CheckInDate} to {viewModel.CheckOutDate} was successful. " +
                                     $"Total cost: ${newBooking.TotalCost:F2}."
                    );
                }
            }

            // Filter available lodging results
            viewModel.SearchResults = allLodgings.Where(l =>
                (string.IsNullOrEmpty(viewModel.SearchBedCount) || l.BedCount == viewModel.SearchBedCount) &&
                !(viewModel.SearchCheckInDate.HasValue && viewModel.SearchCheckOutDate.HasValue &&
                  allBookings.Any(b =>
                      b.Lodging.LodgingID == l.LodgingID &&
                      !(viewModel.SearchCheckOutDate < b.CheckInDate || viewModel.SearchCheckInDate > b.CheckOutDate)))
            ).ToList();

            return View(viewModel);
        }

    }
}
