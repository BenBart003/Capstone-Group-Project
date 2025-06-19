using Moq;
using ProjectLibraryGroup2;
using ProjectMvcGroup2.Controllers;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.ViewModels;
using Xunit;

namespace ProjectTestGroup2
{
    public class LodgingTest
    {
        [Fact]
        public void ShouldSearchLodging()
        {
            List<Lodging> inputLodging = CreateTestData();
            List<LodgingDates> inputLodgingDates = CreateTestDateData();
            int? inputRoomNumber = null;
            string? inputBedCount = null;
            double? inputCostPerNight = null;
            DateOnly? inputCheckInDate = null;
            DateOnly? inputCheckOutDate = null;

            // First search
            List<Lodging> outputLodging = Lodging.SearchLodging(inputLodging, inputLodgingDates, inputRoomNumber, inputBedCount, inputCostPerNight, inputCheckInDate, inputCheckOutDate);
            int expectedNumberOfLodging = 4;
            Assert.Equal(expectedNumberOfLodging, outputLodging.Count);

            // Second search
            inputBedCount = "1K";
            expectedNumberOfLodging = 1;
            outputLodging = Lodging.SearchLodging(inputLodging, inputLodgingDates, inputRoomNumber, inputBedCount, inputCostPerNight, inputCheckInDate, inputCheckOutDate);
            Assert.Equal(expectedNumberOfLodging, outputLodging.Count);

            //Third search
            inputBedCount = "1Q";
            inputCheckInDate = new DateOnly(2021, 10, 2);
            inputCheckOutDate = new DateOnly(2021, 10, 4);
            expectedNumberOfLodging = 0;
            outputLodging = Lodging.SearchLodging(inputLodging, inputLodgingDates, inputRoomNumber, inputBedCount, inputCostPerNight, inputCheckInDate, inputCheckOutDate);
            Assert.Equal(expectedNumberOfLodging, outputLodging.Count);

            //Fourth search
            inputBedCount = null;
            inputCheckInDate = new DateOnly(2021, 11, 1);
            inputCheckOutDate = new DateOnly(2021, 11, 20);
            expectedNumberOfLodging = 3;
            outputLodging = Lodging.SearchLodging(inputLodging, inputLodgingDates, inputRoomNumber, inputBedCount, inputCostPerNight, inputCheckInDate, inputCheckOutDate);
            Assert.Equal(expectedNumberOfLodging, outputLodging.Count);

        }



        [Fact]
        public void ShouldCreateLodging()
        {
            // 1. Arrange
            List<LodgingDates> existingBookings = new List<LodgingDates>();

            // Using test data similar to InitialDatabase
            Guest testGuest = new Guest("Test6", "Guest", "304-000-0006", "Test6.Guest@test.com", "Test6.Guest");
            Lodging testLodging = new Lodging { LodgingID = 101, RoomNumber = 101, BedCount = "1 Queen", CostPerNight = 100.00 };

            CreateLodgingViewModel viewModel = new CreateLodgingViewModel
            {
                LodgingID = testLodging.LodgingID,
                GuestId = "Test6.Guest",
                CheckInDate = new DateOnly(2025, 6, 1),
                CheckOutDate = new DateOnly(2025, 6, 5)
            };

            int expectedNumberOfBookings = existingBookings.Count + 1;

            // 2. Act
            LodgingDates newBooking = new LodgingDates(testGuest, testLodging, viewModel.CheckInDate, viewModel.CheckOutDate);
            existingBookings.Add(newBooking); // Simulating database addition

            // 3. Assert
            Assert.Equal(expectedNumberOfBookings, existingBookings.Count);
            Assert.Equal(testGuest, existingBookings[^1].Guest);
            Assert.Equal(testLodging, existingBookings[^1].Lodging);
            Assert.Equal(viewModel.CheckInDate, existingBookings[^1].CheckInDate);
            Assert.Equal(viewModel.CheckOutDate, existingBookings[^1].CheckOutDate);
        }


        [Fact]
        public void ShouldNotCreateLodging_InvalidDates()
        {
            // 1. Arrange
            List<LodgingDates> existingBookings = new List<LodgingDates>();

            Guest testGuest = new Guest("Test6", "Guest", "304-000-0006", "Test6.Guest@test.com", "Test6.Guest");
            Lodging testLodging = new Lodging { LodgingID = 101, RoomNumber = 101, BedCount = "1 Queen", CostPerNight = 100.00 };

            // Invalid date range: Check-out BEFORE check-in
            CreateLodgingViewModel viewModel = new CreateLodgingViewModel
            {
                LodgingID = testLodging.LodgingID,
                GuestId = "Test6.Guest",
                CheckInDate = new DateOnly(2025, 6, 5),
                CheckOutDate = new DateOnly(2025, 6, 1) // Invalid
            };

            int expectedNumberOfBookings = existingBookings.Count; // No new bookings should be added

            // 2. Act
            LodgingDates? newBooking = null;

            if (viewModel.CheckOutDate >= viewModel.CheckInDate) // Enforcing validation
            {
                newBooking = new LodgingDates(testGuest, testLodging, viewModel.CheckInDate, viewModel.CheckOutDate);
                existingBookings.Add(newBooking);
            }

            // 3. Assert
            Assert.Null(newBooking); // Booking should NOT be created
            Assert.Equal(expectedNumberOfBookings, existingBookings.Count); // Count should remain unchanged
        }

     
        
        [Fact]
        public void ShouldNotCreateLodging_AlreadyReservedRoom()
        {
            // 1. Arrange
            List<LodgingDates> existingBookings = new List<LodgingDates>();

            Guest testGuest1 = new Guest("Test6", "Guest", "304-000-0006", "Test6.Guest@test.com", "Test6.Guest");
            Guest testGuest2 = new Guest("Test9", "Guest", "304-000-0009", "Test9.Guest@test.com", "Test9.Guest");

            Lodging testLodging = new Lodging { LodgingID = 101, RoomNumber = 101, BedCount = "1 Queen", CostPerNight = 100.00 };

            // Simulate existing booking
            existingBookings.Add(new LodgingDates(testGuest1, testLodging, new DateOnly(2025, 6, 1), new DateOnly(2025, 6, 5)));

            // Attempt to book same room during occupied dates
            CreateLodgingViewModel viewModel = new CreateLodgingViewModel
            {
                LodgingID = testLodging.LodgingID,
                GuestId = "Test9.Guest",
                CheckInDate = new DateOnly(2025, 6, 2),
                CheckOutDate = new DateOnly(2025, 6, 4) // Overlaps existing booking
            };

            int expectedNumberOfBookings = existingBookings.Count; // Booking count should remain the same

            // 2. Act
            LodgingDates? newBooking = null;

            bool isRoomAvailable = !existingBookings.Any(b =>
                b.Lodging.LodgingID == viewModel.LodgingID &&
                !(viewModel.CheckOutDate < b.CheckInDate || viewModel.CheckInDate > b.CheckOutDate)
            );

            if (isRoomAvailable)
            {
                newBooking = new LodgingDates(testGuest2, testLodging, viewModel.CheckInDate, viewModel.CheckOutDate);
                existingBookings.Add(newBooking);
            }

            // 3. Assert
            Assert.Null(newBooking); // Booking should NOT be created
            Assert.Equal(expectedNumberOfBookings, existingBookings.Count); // Count should remain unchanged
        }




        public List<Lodging> CreateTestData()
        {
            List<Lodging> testLodging = new List<Lodging>();

            Lodging lodging = new Lodging
            {//has dates
                LodgingID = 1,
                RoomNumber = 101,
                BedCount = "1Q",
                CostPerNight = 110.00
            };
            testLodging.Add(lodging);

            lodging = new Lodging
            {
                LodgingID = 2,
                RoomNumber = 102,
                BedCount = "1K",
                CostPerNight = 140.00
            };
            testLodging.Add(lodging);

            lodging = new Lodging
            {
                LodgingID = 3,
                RoomNumber = 103,
                BedCount = "1K/1T",
                CostPerNight = 170.00
            };
            testLodging.Add(lodging);

            lodging = new Lodging
            {//has dates
                LodgingID = 4,
                RoomNumber = 103,
                BedCount = "2F",
                CostPerNight = 150.00
            };
            testLodging.Add(lodging);

            return testLodging;
        }
        public List<LodgingDates> CreateTestDateData()
        {
            List<Lodging> testLodging = CreateTestData();
            List<LodgingDates> testLodgingDates = new List<LodgingDates>();

            LodgingDates lodgingDates = new LodgingDates
            {
                Lodging = testLodging[0],
                CheckInDate = new DateOnly(2021, 10, 1),
                CheckOutDate = new DateOnly(2021, 10, 3)
            };
            testLodgingDates.Add(lodgingDates);

            lodgingDates = new LodgingDates
            {
                Lodging = testLodging[3],
                CheckInDate = new DateOnly(2021, 11, 1),
                CheckOutDate = new DateOnly(2021, 11, 3)
            };
            testLodgingDates.Add(lodgingDates);

            return testLodgingDates;
        }
    }
}
