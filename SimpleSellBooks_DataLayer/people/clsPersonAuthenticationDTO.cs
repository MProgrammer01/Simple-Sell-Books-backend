using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsPersonAuthenticationDTO
    {
        public int personID { get; set; }
        public string email { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;

        public clsPersonAuthenticationDTO() { }
        public clsPersonAuthenticationDTO(int personID, string email, string passwordHash,
            string role) {
            this.personID = personID;
            this.email= email;
            this.passwordHash = passwordHash;
            this.role = role;
        
        }

    }
}
