using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsChangePasswordDTO
    {
        public int personID { get; set; }
        public string currentPassword { get; set; } = string.Empty;
        public string newPassword { get; set; } = string.Empty;

        public clsChangePasswordDTO() { }
        public clsChangePasswordDTO(int personID, string currentPassword, string newPassword) { 
       
            this.personID = personID;
            this.currentPassword = currentPassword;
            this.newPassword = newPassword;
        }
    }
}
