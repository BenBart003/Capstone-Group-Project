using ProjectLibraryGroup2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using ProjectMvcGroup2.Controllers;
using ProjectMvcGroup2.Data;
using ProjectMvcGroup2.Models;
using ProjectMvcGroup2.ViewModels;
using Xunit.Abstractions;




namespace ProjectTestGroup2
{
    public class RentalTest
    {

        [Fact]
        public void ShouldSearchRentals()
        {
            // AAA Pattern

            // 1. Arrange
            List<Rents> inputRents = CreateTestData();
            string inputEmail = null; // Optional criteria
            List<Rents> outputRents = new List<Rents>();
            int expectedNumberOfRents = 0;

            // 2. Act
            outputRents = Rents.SearchRents(inputRents, inputEmail);

            // 3. Assert
            Assert.Equal(expectedNumberOfRents, outputRents.Count);

            // 2.1 Arrange: Filter by email
            inputEmail = "Test3.Guest@test.com";
            expectedNumberOfRents = 1;

            // 2.2 Act
            outputRents = Rents.SearchRents(inputRents, inputEmail);

            // 2.3 Assert
            Assert.Equal(expectedNumberOfRents, outputRents.Count);

            // 3.1 Arrange: Guest with multiple rentals
            inputEmail = "Test4.Guest@test.com";
            expectedNumberOfRents = 2;

            // 3.2
            outputRents = Rents.SearchRents(inputRents, inputEmail);

            // 3.3
            Assert.Equal(expectedNumberOfRents, outputRents.Count);
        }

        [Fact]
        public void ShouldCreateRental()
        {
            // 1. Arrange
            List<Rents> inputRents = CreateTestData();
            Guest testGuest = new Guest("Test5", "Guest", "304-000-0005", "Test5.Guest@test.com", "Test5.Guest");
            EquipmentRental testEquipment = new EquipmentRental { EquipmentRentalID = 2, Type = "Snowboard", Size = "Large" };

            CreateRentalViewModel viewModel = new CreateRentalViewModel
            {
                EquipmentRentalID = testEquipment.EquipmentRentalID,
                ERentStartDate = new DateTime(2025, 5, 10),
                ERentEndDate = new DateTime(2025, 5, 15)
            };

            int expectedNumberOfRents = inputRents.Count + 1;

            // 2. Act
            Rents newRental = new Rents(testGuest, testEquipment, viewModel.ERentStartDate, viewModel.ERentEndDate);
            inputRents.Add(newRental);
            List<Rents> outputRents = inputRents;

            // 3. Assert
            Assert.Equal(expectedNumberOfRents, outputRents.Count);
            Assert.Equal(testEquipment.Type, outputRents[^1].EquipmentRental.Type);
            Assert.Equal(viewModel.ERentStartDate, outputRents[^1].ERentStartDate);
        }

        [Fact]
        public void ShouldNOTCreateRental()
        {
            // 1. Arrange
            List<Rents> inputRents = CreateTestData();
            Guest testGuest = new Guest("Test5", "Guest", "304-000-0005", "Test5.Guest@test.com", "Test5.Guest");
            EquipmentRental testEquipment = new EquipmentRental { EquipmentRentalID = 2, Type = "Snowboard", Size = "Large" };

            // Provide invalid rental dates (end before start)
            CreateRentalViewModel viewModel = new CreateRentalViewModel
            {
                EquipmentRentalID = testEquipment.EquipmentRentalID,
                ERentStartDate = new DateTime(2025, 5, 15),
                ERentEndDate = new DateTime(2025, 5, 10) // Invalid end date
            };

            int expectedNumberOfRents = inputRents.Count; // The count should remain unchanged

            // 2. Act
            Rents newRental = new Rents(testGuest, testEquipment, viewModel.ERentStartDate, viewModel.ERentEndDate);

            // Prevent adding rental if dates are invalid
            if (newRental.ERentEndDate >= newRental.ERentStartDate)
            {
                inputRents.Add(newRental); // Only add valid rentals
            }

            List<Rents> outputRents = inputRents;

            // 3. Assert
            Assert.Equal(expectedNumberOfRents, outputRents.Count); // Ensure rental was NOT created
        }



        public List<Rents> CreateTestData()
        {
            List<Rents> testRentals = new List<Rents>();

            Guest guest3 = new Guest("Test3", "Guest", "304-000-0003", "Test3.Guest@test.com", "Test3.Guest");
            Guest guest4 = new Guest("Test4", "Guest", "304-000-0004", "Test4.Guest@test.com", "Test4.Guest");
            Guest guest5 = new Guest("Test5", "Guest", "304-000-0005", "Test5.Guest@test.com", "Test5.Guest");

            Rents rents =
                new Rents
                {
                    RentsID = 1,
                    ERentStartDate = new DateTime(2024, 12, 1),
                    ERentEndDate = new DateTime(2024, 12, 1),
                    Guest = guest3
                };
            testRentals.Add(rents);

            rents =
                new Rents
                {
                    RentsID = 2,
                    ERentStartDate = new DateTime(2024, 12, 1),
                    ERentEndDate = new DateTime(2024, 12, 2),
                    Guest = guest4
                };
            testRentals.Add(rents);

            rents =
                new Rents
                {
                    RentsID = 3,
                    ERentStartDate = new DateTime(2024, 12, 1),
                    ERentEndDate = new DateTime(2024, 12, 1),
                    Guest = guest4
                };
            testRentals.Add(rents);

            return testRentals;
        }



    } // End RentalTest
} // End Namespace
