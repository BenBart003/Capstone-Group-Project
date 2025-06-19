using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryGroup2
{
    public class Guest : AppUser
    {
        public string PaymentInfo { get; set; }
        public Guest() { }
        public Guest(string firstName, string lastName, string phoneNumber, string email, string password)
            : base(firstName, lastName, phoneNumber, email, password)
        {

        }
    }
}
