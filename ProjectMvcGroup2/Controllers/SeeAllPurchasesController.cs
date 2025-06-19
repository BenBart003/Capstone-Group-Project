using Microsoft.AspNetCore.Mvc;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.ViewModels;

namespace ProjectMvcGroup2.Controllers
{
    public class SeeAllPurchasesController : Controller
    {
        private readonly IEquipmentRentalRepo _equipmentRepo;
        private readonly ILiftTicketRepo _liftTicketRepo;
        private readonly ILodgingRepo _lodgingRepo;
        private readonly IAppUserRepo _appUserRepo;

        public SeeAllPurchasesController(IEquipmentRentalRepo equipmentRepo, ILiftTicketRepo liftTicketRepo, ILodgingRepo lodgingRepo, IAppUserRepo appUserRepo)
        {
            _equipmentRepo = equipmentRepo;
            _liftTicketRepo = liftTicketRepo;
            _lodgingRepo = lodgingRepo;
            _appUserRepo = appUserRepo;
        }

        [HttpGet]
        [HttpPost]
        public IActionResult FindAllPurchases(SeeAllPurchasesViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Email))
            {
                string userId = _appUserRepo.GetLoggedInUserId();
                Guest guest = _appUserRepo.GetGuest(userId);

                if (guest != null)
                {
                    viewModel.Email = guest.Email;
                }
            }

            if (!string.IsNullOrEmpty(viewModel.Email))
            {
                viewModel.Rents = _equipmentRepo.GetAllRents()
                    .Where(r => r.Guest.Email == viewModel.Email)
                    .ToList();

                viewModel.Tickets = _liftTicketRepo.GetAllTicketDates()
                    .Where(t => t.Guest.Email == viewModel.Email)
                    .ToList();

                viewModel.Lodgings = _lodgingRepo.GetAllLodgingDates()
                    .Where(ld => ld.Guest.Email == viewModel.Email)
                    .ToList();
            }
            else
            {
                viewModel.Rents = new List<Rents>();
                viewModel.Tickets = new List<TicketDate>();
                viewModel.Lodgings = new List<LodgingDates>();
            }

            return View(viewModel);
        }
    }
}
