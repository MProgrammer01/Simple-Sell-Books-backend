using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsUpdatePersonDTO
    {
        public int personID { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? phone { get; set; }
        public string? addressPerson { get; set; }

        public clsUpdatePersonDTO() { }
        public clsUpdatePersonDTO(int personID, string fullName, string email,
            string? phone, string? addressPerson) {
            this.personID = personID;
            this.fullName = fullName;
            this.email = email;
            this.phone = phone;
            this.addressPerson = addressPerson;
        }

    }
}
