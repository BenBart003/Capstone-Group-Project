using Microsoft.AspNetCore.Identity;

namespace ProjectLibraryGroup2
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AppUser() { }

        public AppUser(string firstName, string lastName, string phoneNumber, string email, string password)
        {

            this.FirstName = firstName;
            this.LastName = lastName;
            this.PhoneNumber = phoneNumber;
            this.Email = email;
            this.UserName = email;

            
            PasswordHasher<AppUser> passwordHasher = new PasswordHasher<AppUser>();
            this.PasswordHash = passwordHasher.HashPassword(this, password);
        }
    }
}
