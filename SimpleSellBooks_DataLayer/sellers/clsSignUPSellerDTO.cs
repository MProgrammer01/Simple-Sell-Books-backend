using SimpleSellBooks_DataLayer.people;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.sellers
{
    public class clsSignUPSellerDTO : clsSignUpDTO
    {
        public string storeName { get; set; } = string.Empty;
        public string? logoStore { get; set; }

        public clsSignUPSellerDTO(
            string fullName,
            string email,
            string password,
            string? phone,
            string? addressPerson,
            string storeName,
            string? logoStore)
        : base(fullName, email, password, phone, addressPerson)
        {
            this.storeName = storeName;
            this.logoStore = logoStore;
        }
    }
}
