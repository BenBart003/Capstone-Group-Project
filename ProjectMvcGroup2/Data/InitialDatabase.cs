using ProjectLibraryGroup2;
using Microsoft.AspNetCore.Identity;
using ProjectLibraryGroup2;
using System.Drawing;

namespace ProjectMvcGroup2.Data
{
    public class InitialDatabase
    {
        public static void SeedDatabase(IServiceProvider services)
        {
            // 1. Database service
            ApplicationDbContext database = services.GetRequiredService<ApplicationDbContext>();

            // 2. AppUser service
            UserManager<AppUser> userManager = services.GetRequiredService<UserManager<AppUser>>();

            // 3. Role service
            RoleManager<IdentityRole> roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Admin";
            string guestRole = "Guest";
            string employeeRole = "Employee";
            string studentRole = "Student";
            string managerRole = "Manager";

            if (!database.Roles.Any())
            {
                roleManager.CreateAsync(new IdentityRole(adminRole)).Wait();
                roleManager.CreateAsync(new IdentityRole(guestRole)).Wait();
                roleManager.CreateAsync(new IdentityRole(studentRole)).Wait();
                roleManager.CreateAsync(new IdentityRole(employeeRole)).Wait();
                roleManager.CreateAsync(new IdentityRole(managerRole)).Wait();
            }

            if (!database.Users.Any())
            {
                AppUser appUser = new AppUser("Test1", "NoRole", "304-000-0001", "Test1.NoRole@test.com", "Test1.NoRole");
                userManager.CreateAsync(appUser).Wait();

                    appUser = new AppUser("Test2", "Admin", "304-000-0002", "Test2.Admin@test.com", "Test2.Admin");
                    userManager.CreateAsync(appUser).Wait();
                    userManager.AddToRoleAsync(appUser, adminRole).Wait();
                    appUser.EmailConfirmed = true; // Mark email as verified
                    userManager.UpdateAsync(appUser).Wait();

                    Guest guest = new Guest("Test3", "Guest", "304-000-0003", "Test3.Guest@test.com", "Test3.Guest");
                    userManager.CreateAsync(guest).Wait();
                    userManager.AddToRoleAsync(guest, guestRole).Wait();
                    guest.EmailConfirmed = true; // Mark email as verified
                    userManager.UpdateAsync(guest).Wait();

                    guest = new Guest("Test4", "Guest", "304-000-0004", "Test4.Guest@test.com", "Test4.Guest");
                    userManager.CreateAsync(guest).Wait();
                    userManager.AddToRoleAsync(guest, guestRole).Wait();
                    guest.EmailConfirmed = true; // Mark email as verified
                    userManager.UpdateAsync(guest).Wait();

                    guest = new Guest("Test5", "Guest", "304-000-0005", "Test5.Guest@test.com", "Test5.Guest");
                    userManager.CreateAsync(guest).Wait();
                    userManager.AddToRoleAsync(guest, guestRole).Wait();
                    guest.EmailConfirmed = true; // Mark email as verified
                    userManager.UpdateAsync(guest).Wait();

                    guest = new Guest("Test6", "Guest", "304-000-0006", "Test6.Guest@test.com", "Test6.Guest");
                    userManager.CreateAsync(guest).Wait();
                    userManager.AddToRoleAsync(guest, guestRole).Wait();
                    guest.EmailConfirmed = true; // Mark email as verified
                    userManager.UpdateAsync(guest).Wait();

                    guest = new Guest("Test9", "Guest", "304-000-0009", "Test9.Guest@test.com", "Test9.Guest");
                    userManager.CreateAsync(guest).Wait();
                    userManager.AddToRoleAsync(guest, guestRole).Wait();
                    guest.EmailConfirmed = true; // Mark email as verified
                    userManager.UpdateAsync(guest).Wait();

                    //Student student = new Student("Test7", "Student", "304-000-0007", "Test7.Student@test.com", "Test7.Student");
                    //userManager.CreateAsync(student).Wait();
                    //userManager.AddToRoleAsync(student, studentRole).Wait();
                    //student.EmailConfirmed = true; // Mark email as verified
                    //userManager.UpdateAsync(student).Wait();

                    //student = new Student("Test8", "Student", "304-000-0008", "Test8.Student@test.com", "Test8.Student");
                    //userManager.CreateAsync(student).Wait();
                    //userManager.AddToRoleAsync(student, studentRole).Wait();
                    //student.EmailConfirmed = true; // Mark email as verified
                    //userManager.UpdateAsync(student).Wait();
                }

            if (!database.Lodging.Any())
            {
                database.Lodging.AddRange(new List<Lodging>
                    {
                        new Lodging { RoomNumber = 101, BedCount = "1 Queen", CostPerNight = 100.00 },
                        new Lodging { RoomNumber = 102, BedCount = "2 Queen", CostPerNight = 150.00 },
                        new Lodging { RoomNumber = 103, BedCount = "1 King", CostPerNight = 200.00 },
                        new Lodging { RoomNumber = 104, BedCount = "2 King", CostPerNight = 250.00 },
                        new Lodging { RoomNumber = 105, BedCount = "1 Queen", CostPerNight = 100.00 }
                    });
                database.SaveChanges();
            }

            if (!database.LodgingDates.Any())
            {
                List<Guest> guestList = database.Guest
                    .Where(g => new[] { "Test6.Guest@test.com", "Test9.Guest@test.com", "Test3.Guest@test.com", "Test4.Guest@test.com", "Test5.Guest@test.com" }
                    .Contains(g.UserName)).ToList();

                List<Lodging> lodgingList = database.Lodging.ToList();

                database.LodgingDates.AddRange(new List<LodgingDates>
                    {
                        new LodgingDates(guestList[0], lodgingList[0], new DateOnly(2022, 1, 1), new DateOnly(2022, 1, 2)),
                        new LodgingDates(guestList[1], lodgingList[1], new DateOnly(2022, 1, 3), new DateOnly(2022, 1, 4)),
                        new LodgingDates(guestList[2], lodgingList[2], new DateOnly(2022, 1, 5), new DateOnly(2022, 1, 6)),
                        new LodgingDates(guestList[3], lodgingList[3], new DateOnly(2022, 1, 7), new DateOnly(2022, 1, 8))
                    });
                database.SaveChanges();
            }

            if (!database.LiftTicket.Any())
            {
                database.LiftTicket.AddRange(new List<LiftTicket>
                    {
                        new LiftTicket(TicketType.DayPass, 50.00),
                        new LiftTicket(TicketType.WeekendPass, 80.00),
                        new LiftTicket(TicketType.SeasonPass, 400.00)
                    });
                database.SaveChanges();
            }

            if (!database.TicketDate.Any())
            {
                var guestList = database.Guest
                    .Where(g => new[] { "Test3.Guest@test.com", "Test4.Guest@test.com", "Test5.Guest@test.com" }
                    .Contains(g.UserName)).ToList();

                var tickets = database.LiftTicket.ToList();

                database.TicketDate.AddRange(new List<TicketDate>
                    {
                        new TicketDate(guestList[0], tickets.First(t => t.TicketType == TicketType.SeasonPass),
                            DateOnly.FromDateTime(DateTime.Today), new DateOnly(DateTime.Today.Year + 1, 3, 31)),
                        new TicketDate(guestList[1], tickets.First(t => t.TicketType == TicketType.WeekendPass),
                            new DateOnly(2024, 11, 1), new DateOnly(2024, 11, 3)),
                        new TicketDate(guestList[2], tickets.First(t => t.TicketType == TicketType.DayPass),
                            new DateOnly(2024, 12, 15), new DateOnly(2024, 12, 15))
                    });
                database.SaveChanges();
            }

            if (!database.EquipmentRental.Any())
            {
                database.EquipmentRental.AddRange(new List<EquipmentRental>
                    {
                        new EquipmentRental { Type = "Ski", Size = "100", Price = 100.00 },
                        new EquipmentRental { Type = "Ski", Size = "110", Price = 100.00 },
                        new EquipmentRental { Type = "Snowboard", Size = "150", Price = 85.00 },
                        new EquipmentRental { Type = "Snowboard", Size = "160", Price = 85.00 },
                        new EquipmentRental { Type = "Ski", Size = "155", Price = 115.00 }
                    });
                database.SaveChanges();
            }

            if (!database.Rents.Any())
            {
                List<Guest> guestList = database.Guest
                    .Where(g => new[] { "Test6.Guest@test.com", "Test9.Guest@test.com", "Test3.Guest@test.com", "Test4.Guest@test.com", "Test5.Guest@test.com" }
                    .Contains(g.UserName)).ToList();

                List<EquipmentRental> equipmentRentalList = database.EquipmentRental.ToList();

                database.Rents.AddRange(new List<Rents>
                    {
                        new Rents(guestList[0], equipmentRentalList[0], new DateTime(2024, 12, 1), new DateTime(2024, 12, 1)),
                        new Rents(guestList[1], equipmentRentalList[1], new DateTime(2024, 12, 2), new DateTime(2024, 12, 2)),
                        new Rents(guestList[2], equipmentRentalList[2], new DateTime(2024, 12, 3), new DateTime(2024, 12, 3)),
                        new Rents(guestList[3], equipmentRentalList[3], new DateTime(2024, 12, 4), new DateTime(2024, 12, 4)),
                        new Rents(guestList[4], equipmentRentalList[4], new DateTime(2024, 12, 5), new DateTime(2024, 12, 5))
                    });
                database.SaveChanges();
            }
        }
    }
}
