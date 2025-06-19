using Microsoft.AspNetCore.Mvc;
using Moq;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Controllers;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.Services;
using ProjectMvcGroup2.ViewModels;
using System;
using Xunit;

namespace ProjectTestGroup2
{
    public class LiftTicketTest
    {
        private readonly Mock<IAppUserRepo> mockAppUserRepo = new Mock<IAppUserRepo>();
        private readonly Mock<ILiftTicketRepo> mockLiftTicketRepo = new Mock<ILiftTicketRepo>();
        private readonly Mock<ILiftTicketEmailSender> mockEmailSender = new Mock<ILiftTicketEmailSender>();

        private readonly LiftTicketController liftTicketController;

        public LiftTicketTest()
        {
            liftTicketController = new LiftTicketController(mockAppUserRepo.Object, mockLiftTicketRepo.Object, mockEmailSender.Object);
        }

        [Fact]
        public void ShouldCreateLiftTicket()
        {
            string mockUserId = "G1";
            Guest mockGuest = new Guest { Id = mockUserId };
            mockAppUserRepo.Setup(repo => repo.GetLoggedInUserId()).Returns(mockUserId);
            mockAppUserRepo.Setup(repo => repo.GetGuest(mockUserId)).Returns(mockGuest);

            CreateTicketViewModel viewModel = new CreateTicketViewModel
            {
                SelectedTicketType = TicketType.DayPass,
                PassStartDate = DateOnly.FromDateTime(DateTime.Today)
            };

            TicketDate createdTicket = null;
            mockLiftTicketRepo.Setup(repo => repo.AddTicketPurchase(It.IsAny<TicketDate>()))
                .Callback<TicketDate>(ticket => createdTicket = ticket);

            liftTicketController.BuyTicket(viewModel);

            Assert.NotNull(createdTicket);
            Assert.Equal(mockGuest, createdTicket.Guest);
            Assert.Equal(TicketType.DayPass, createdTicket.LiftTicket.TicketType);
            Assert.Equal(50.00, createdTicket.LiftTicket.Cost);
            Assert.Equal(viewModel.PassStartDate.Value, createdTicket.PassStartDate);
            Assert.Equal(viewModel.PassStartDate.Value, createdTicket.PassEndDate);
        }

        [Fact]
        public void ShouldNotCreateWeekendPassStartingOnThursday()
        {
            string mockGuestId = "G1";
            Guest mockGuest = new Guest { Id = mockGuestId};

            mockAppUserRepo.Setup(r => r.GetLoggedInUserId()).Returns(mockGuestId);
            mockAppUserRepo.Setup(r => r.GetGuest(mockGuestId)).Returns(mockGuest);

            CreateTicketViewModel viewModel = new CreateTicketViewModel
            {
                SelectedTicketType = TicketType.WeekendPass,
                PassStartDate = new DateOnly(2025, 5, 8) // Thursday
            };

            IActionResult result = liftTicketController.BuyTicket(viewModel);

            ViewResult viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(liftTicketController.ModelState.IsValid);
            Assert.True(liftTicketController.ModelState[nameof(viewModel.PassStartDate)].Errors
                .Any(e => e.ErrorMessage.Contains("must start on a Friday")));
        }
    }
}
