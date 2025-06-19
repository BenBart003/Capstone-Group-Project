using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Data;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.Services;
using ProjectMvcGroup2.ViewModels;
using static ProjectLibraryGroup2.EquipmentRental;

namespace ProjectMvcGroup2.Controllers
{
    public class RentsController : Controller
    {
        //public readonly ApplicationDbContext _database;
        public readonly IEquipmentRentalRepo _equipmentRentalRepo;
        public readonly IAppUserRepo _appUserRepo;
        public readonly IEquipmentRentalEmailSender _emailSender;

        public RentsController(ApplicationDbContext database, IEquipmentRentalRepo equipmentRentalRepo, IAppUserRepo appUserRepo, IEquipmentRentalEmailSender emailSender)
        {
            //_database = database;
            _equipmentRentalRepo = equipmentRentalRepo;
            _appUserRepo = appUserRepo;
            _emailSender = emailSender;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SearchRents()
        {
            SearchRentsViewModel viewModel = new SearchRentsViewModel
            {
                RentsSearchResult = _equipmentRentalRepo.GetAllRents()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchRents(SearchRentsViewModel viewModel)
        {
            List<Rents> allRents = _equipmentRentalRepo.GetAllRents();
            if (!string.IsNullOrEmpty(viewModel.Email))
            {
                viewModel.RentsSearchResult = Rents.SearchRents(allRents, viewModel.Email);
            }
            else
            {
                viewModel.RentsSearchResult = allRents;
            }

            return View(viewModel);
        }
        
        [Authorize(Roles = "Guest")]
        [HttpGet]
        public IActionResult CreateRental()
        {
            var viewModel = new CreateRentalViewModel
            {
                SearchResults = _equipmentRentalRepo.GetAllEquipmentRentals()
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        public IActionResult CreateRental(CreateRentalViewModel viewModel)
        {
            var allEquipment = _equipmentRentalRepo.GetAllEquipmentRentals();
            var allRents = _equipmentRentalRepo.GetAllRents();

            // book rental logic
            if (viewModel.EquipmentRentalID != 0)
            {
                string loggedInUserId = _appUserRepo.GetLoggedInUserId();
                Guest guest = _appUserRepo.GetGuest(loggedInUserId);
                EquipmentRental equipmentRentalChoice = _equipmentRentalRepo.GetEquipmentRentalById(viewModel.EquipmentRentalID);
                DateTime eRentStartDate = viewModel.ERentStartDate;
                DateTime eRentEndDate = viewModel.ERentEndDate;

                // Prevent double-booking
                bool isBooked = allRents.Any(r =>
                    r.EquipmentRental.EquipmentRentalID == viewModel.EquipmentRentalID &&
                    !(eRentEndDate < r.ERentStartDate || eRentStartDate > r.ERentEndDate)
                );

                if (isBooked)
                {
                    ModelState.AddModelError("", "This equipment is already booked for the selected dates.");
                }
                else
                {
                    Rents newRental = new Rents(guest, equipmentRentalChoice, eRentStartDate, eRentEndDate);
                    _equipmentRentalRepo.AddRental(newRental);

                    ViewBag.SuccessMessage = $"Successfully rented {equipmentRentalChoice.Type} ({equipmentRentalChoice.Size}) from {eRentStartDate:MM/dd/yyyy} to {eRentEndDate:MM/dd/yyyy}.";

                    _emailSender.SendEquipmentRentalEmail(
                        controllerAndMethod: "SeeAllPurchases/FindAllPurchases",
                        email: guest.Email,
                        subject: "Your equipment rental is confirmed",
                        htmlMessage: $"You have successfully rented {equipmentRentalChoice.Type} ({equipmentRentalChoice.Size}) from {eRentStartDate:MM/dd/yyyy} to {eRentEndDate:MM/dd/yyyy}. " +
                                     $"Total cost: ${newRental.TotalCost:F2}."
                    );
                }
            }

            // filtering
            viewModel.SearchResults = allEquipment.Where(e =>
                (string.IsNullOrEmpty(viewModel.SearchSize) || e.Size == viewModel.SearchSize) &&
                (string.IsNullOrEmpty(viewModel.SearchType) || e.Type == viewModel.SearchType) &&
                !(viewModel.SearchStartDate.HasValue && viewModel.SearchEndDate.HasValue &&
                  allRents.Any(r =>
                      r.EquipmentRental.EquipmentRentalID == e.EquipmentRentalID &&
                      !(viewModel.SearchEndDate < r.ERentStartDate || viewModel.SearchStartDate > r.ERentEndDate)))
            ).ToList();

            return View(viewModel);
        }
    }
}
