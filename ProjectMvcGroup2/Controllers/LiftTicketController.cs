using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.Services;
using ProjectMvcGroup2.ViewModels;
using System.Data;

namespace ProjectMvcGroup2.Controllers
{
    public class LiftTicketController : Controller
    {
        private readonly IAppUserRepo _appUserRepo;
        private readonly ILiftTicketRepo _liftTicketRepo;
        private readonly ILiftTicketEmailSender _emailSender;

        public LiftTicketController(IAppUserRepo appUserRepo, ILiftTicketRepo liftTicketRepo, ILiftTicketEmailSender emailSender)
        {
            _appUserRepo = appUserRepo;
            _liftTicketRepo = liftTicketRepo;
            _emailSender = emailSender;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult SearchLiftTickets()
        {
            var viewModel = new SearchLiftTicketsViewModel
            {
                LiftTicketSearchResult = _liftTicketRepo.GetAllTicketDates()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchLiftTickets(SearchLiftTicketsViewModel viewModel)
        {
            var allTickets = _liftTicketRepo.GetAllTicketDates();

            if (!string.IsNullOrEmpty(viewModel.Email))
            {
                viewModel.LiftTicketSearchResult = allTickets
                    .Where(t => t.Guest.Email.ToLower().Contains(viewModel.Email.ToLower()))
                    .ToList();
            }
            else
            {
                viewModel.LiftTicketSearchResult = allTickets;
            }

            return View(viewModel);
        }

        [Authorize(Roles = "Guest")]
        [HttpGet]
        public IActionResult BuyTicket()
        {
            var viewModel = new CreateTicketViewModel();

            ViewBag.TicketOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "SeasonPass", Value = "SeasonPass" },
                    new SelectListItem { Text = "WeekendPass (Friday–Sunday)", Value = "WeekendPass" },
                    new SelectListItem { Text = "DayPass", Value = "DayPass" }
                };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Guest")]
        public IActionResult BuyTicket(CreateTicketViewModel viewModel)
        {
            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            // Validation logic
            if (viewModel.SelectedTicketType == TicketType.WeekendPass)
            {
                if (!viewModel.PassStartDate.HasValue)
                {
                    ModelState.AddModelError(nameof(viewModel.PassStartDate), "Start date is required for weekend passes.");
                }
                else
                {
                    if (viewModel.PassStartDate.Value.DayOfWeek != DayOfWeek.Friday)
                    {
                        ModelState.AddModelError(nameof(viewModel.PassStartDate), "Weekend passes must start on a Friday.");
                    }
                    if (viewModel.PassStartDate.Value < today)
                    {
                        ModelState.AddModelError(nameof(viewModel.PassStartDate), "Cannot purchase a pass for a past date.");
                    }
                }
            }
            else if (viewModel.SelectedTicketType == TicketType.DayPass)
            {
                if (!viewModel.PassStartDate.HasValue)
                {
                    ModelState.AddModelError(nameof(viewModel.PassStartDate), "Start date is required for day passes.");
                }
                else if (viewModel.PassStartDate.Value < today)
                {
                    ModelState.AddModelError(nameof(viewModel.PassStartDate), "Cannot purchase a pass for a past date.");
                }
            }

            if (!ModelState.IsValid)
            {
                ViewBag.TicketOptions = new List<SelectListItem>
                    {
                        new SelectListItem { Text = "SeasonPass", Value = "SeasonPass" },
                        new SelectListItem { Text = "WeekendPass (Friday–Sunday)", Value = "WeekendPass" },
                        new SelectListItem { Text = "DayPass", Value = "DayPass" }
                    };
                return View(viewModel);
            }

            string userId = _appUserRepo.GetLoggedInUserId();
            Guest guest = _appUserRepo.GetGuest(userId);

            double cost = viewModel.SelectedTicketType switch
            {
                TicketType.DayPass => 50.00,
                TicketType.WeekendPass => 80.00,
                TicketType.SeasonPass => 400.00,
                _ => throw new Exception("Invalid ticket type")
            };

            DateOnly startDate, endDate;
            switch (viewModel.SelectedTicketType)
            {
                case TicketType.SeasonPass:
                    startDate = today;
                    endDate = new DateOnly(today.Year + 1, 3, 31);
                    break;
                case TicketType.WeekendPass:
                    startDate = viewModel.PassStartDate!.Value;
                    endDate = startDate.AddDays(2);
                    break;
                case TicketType.DayPass:
                    startDate = viewModel.PassStartDate!.Value;
                    endDate = startDate;
                    break;
                default:
                    throw new Exception("Unhandled ticket type");
            }

            LiftTicket ticket = new LiftTicket(viewModel.SelectedTicketType, cost);
            TicketDate newTicket = new TicketDate(guest, ticket, startDate, endDate);
            _liftTicketRepo.AddTicketPurchase(newTicket);

            ViewBag.SuccessMessage = $"Purchased {ticket.TicketType} from {startDate} to {endDate} for ${cost}.";

            _emailSender.SendLiftTicketEmail(
                controllerAndMethod: "SeeAllPurchases/FindAllPurchases",
                email: guest.Email,
                subject: "Your lift ticket purchase is confirmed",
                htmlMessage: $"You purchased a {ticket.TicketType} from {startDate} to {endDate} for ${cost:F2}."
            );

            ViewBag.TicketOptions = new List<SelectListItem>
                {
                    new SelectListItem { Text = "SeasonPass", Value = "SeasonPass" },
                    new SelectListItem { Text = "WeekendPass (Friday–Sunday)", Value = "WeekendPass" },
                    new SelectListItem { Text = "DayPass", Value = "DayPass" }
                };

            return View(new CreateTicketViewModel());
        }


    }
}
