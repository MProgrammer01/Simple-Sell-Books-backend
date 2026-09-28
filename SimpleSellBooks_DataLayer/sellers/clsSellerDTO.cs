using SimpleSellBooks_DataLayer.people;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.sellers
{
    public class clsSellerDTO : clsPersonDTO
    {
        public int sellerID { get; set; }
        //public int personID { get; set; }
        public string storeName { get; set; }
        public string? logoStore { get; set; }


        public clsSellerDTO()
        {
            storeName = string.Empty;
        }

        //retrive and add new
        public clsSellerDTO(int sellerID, int personID, string storeName, string? logoStore)
        {
            this.sellerID = sellerID;
            this.personID = personID;
            this.storeName = storeName;
            this.logoStore = logoStore;

        }

        //retrive by personID
        public clsSellerDTO(int sellerID, string fullName,
            string email, string? phone, string? addressPerson, string role, 
            string storeName, string? logoStore)
        {
            this.sellerID = sellerID;
            this.fullName = fullName;
            this.email = email;
            this.phone = phone;
            this.addressPerson = addressPerson;
            this.role = role;
            this.storeName = storeName;
            this.logoStore = logoStore;

        }

        //update
        public clsSellerDTO(int sellerID, string storeName, string? logoStore)
        {
            this.sellerID = sellerID;
            //this.personID = personID;
            this.storeName = storeName;
            this.logoStore = logoStore;

        }
    }
}
