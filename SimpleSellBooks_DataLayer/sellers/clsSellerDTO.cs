using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.sellers
{
    public class clsSellerDTO
    {
        public int sellerID { get; set; }
        public int personID { get; set; }
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
