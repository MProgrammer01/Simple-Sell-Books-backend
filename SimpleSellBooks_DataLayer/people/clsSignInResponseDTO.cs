using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsSignInResponseDTO
    {
        public int personID { get; set; }
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
        public clsSignInResponseDTO() { }
        public clsSignInResponseDTO(int personID,string email, string role) { 
            this.personID = personID;
            this.email = email;
            this.role = role;
        
        }

        public clsSignInResponseDTO(int personID, string email, string role, string token)
        {
            this.personID = personID;
            this.email = email;
            this.role = role;
            this.token = token;
        }
    }
}
