using SimpleSellBooks_BusinessLayer.people;
using SimpleSellBooks_DataLayer.sellers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.sellers
{
    public class clsSellerBusiness
    {
        enum enMode { AddNewSeller = 1, UpdateSeller = 2 }
        enMode Mode = enMode.AddNewSeller;

        public int sellerID { get; set; }
        public int personID { get; set; }
        public string storeName { get; set; }
        public string? logoStore { get; set; }

        public clsPersonBusiness personInfo;


        public clsSellerDTO sellerDTO
        {
            get
            {
                return new clsSellerDTO(this.sellerID, this.personID, this.storeName, this.logoStore);
            }
        }


        clsSellerDTO sellerUpdateDTO
        {
            get
            {
                return new clsSellerDTO(this.sellerID, this.storeName, this.logoStore);
            }
        }

        public clsSellerBusiness()
        {
            this.sellerID = 0;
            this.personID = 0;
            this.storeName = string.Empty;
            this.logoStore = string.Empty;
            this.personInfo = new clsPersonBusiness();
            Mode = enMode.AddNewSeller;
        }

        clsSellerBusiness(clsSellerDTO sellerDTO)
        {
            this.sellerID = sellerDTO.sellerID;
            this.personID = sellerDTO.personID;
            this.storeName = sellerDTO.storeName;
            this.logoStore = sellerDTO.logoStore;
            this.personInfo = clsPersonBusiness.FindPerson(sellerDTO.personID) ?? new clsPersonBusiness();
            Mode = enMode.UpdateSeller;
        }

        public static IEnumerable<clsSellerDTO> GetAllSellers()
        {
            return clsSellerData.GetAllSellers();
        }


        public static clsSellerBusiness? FindSeller(int sellerID)
        {
            clsSellerDTO sellerDTO = clsSellerData.GetSellerByID(sellerID);

            if (sellerDTO != null && sellerDTO.sellerID > 0)
            {
                return new clsSellerBusiness(sellerDTO);
            }
            return null;
        }


        bool _AddNewSeller()
        {
            this.sellerID = clsSellerData.AddNewSeller(sellerDTO);
            return (this.sellerID > 0);
        }


        bool _UpdateSeller()
        {
            return clsSellerData.UpdateSeller(sellerUpdateDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewSeller:
                    if (_AddNewSeller())
                    {
                        Mode = enMode.UpdateSeller;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.UpdateSeller:
                    return _UpdateSeller();

            }

            return false;
        }



        public static bool DeleteSeller(int sellerID)
        {
            return clsSellerData.DeleteSeller(sellerID);
        }
    }
}
