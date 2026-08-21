using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsSignUpDTO
    {
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string? phone { get; set; }
        public string? addressPerson { get; set; }

        public clsSignUpDTO() { }
        public clsSignUpDTO(string fullName, string email, string password, string? phone,
            string? addressPerson) {

            this.fullName = fullName;
            this.email = email;
            this.password = password;
            this.phone = phone;
            this.addressPerson = addressPerson;
        
        }
    }
}
