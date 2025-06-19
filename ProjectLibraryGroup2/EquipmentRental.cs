using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectLibraryGroup2
{
    public class EquipmentRental
    {
        public int EquipmentRentalID { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }

        public EquipmentRental() { }
        public EquipmentRental(string type, string size, double price)
        {
            this.Size = size;
            this.Type = type;
            this.Price = price;
        }
    }
}
        