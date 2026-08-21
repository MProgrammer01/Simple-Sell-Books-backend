using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsSignInDTO
    {

        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

        public clsSignInDTO() { }
        public clsSignInDTO(string email, string password) {
            this.email = email;
            this.password = password;
        
        }
    }
}
