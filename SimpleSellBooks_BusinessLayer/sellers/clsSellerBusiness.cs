using SimpleSellBooks_BusinessLayer.people;
using SimpleSellBooks_DataLayer.people;
using SimpleSellBooks_DataLayer.sellers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.sellers
{
    public class clsSellerBusiness : clsPersonBusiness
    {
        enum enMode { AddNewSeller = 1, UpdateSeller = 2 }
        enMode Mode = enMode.AddNewSeller;

        public int sellerID { get; set; }
        public string storeName { get; set; }
        public string? logoStore { get; set; }

        public clsSellerDTO sellerDTO
        {
            get
            {
                return new clsSellerDTO(
                    this.personID,
                    this.fullName,
                    this.email,
                    this.phone,
                    this.addressPerson,
                    this.storeName,
                    this.logoStore);
            }
        }

        public clsSellerDTO addSellerDTO
        {
            get
            {
                return new clsSellerDTO(
                    this.fullName,
                    this.email,
                    this.password,
                    this.phone,
                    this.addressPerson,
                    this.storeName,
                    this.logoStore);
            }
        }

        public clsSellerDTO sellerByPersonIDDTO
        {
            get
            {
                return new clsSellerDTO(
                    this.personID,
                    this.fullName,
                    this.email,
                    this.phone,
                    this.addressPerson,
                    this.role,
                    this.storeName, 
                    this.logoStore);
            }
        }


        clsSellerDTO sellerUpdateDTO
        {
            get
            {
                return new clsSellerDTO(
                    this.personID,
                    this.fullName,
                    this.email,
                    this.phone,
                    this.addressPerson, 
                    this.storeName,
                    this.logoStore);
            }
        }

        public clsSellerBusiness()
        {
            this.sellerID = 0;
            this.personID = 0;
            this.fullName = string.Empty;
            this.email = string.Empty;
            this.password = string.Empty;
            this.phone = string.Empty;
            this.addressPerson = string.Empty;
            this.role = string.Empty;
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;
            this.storeName = string.Empty;
            this.logoStore = string.Empty;
            Mode = enMode.AddNewSeller;
        }

        clsSellerBusiness(int sellerID, int personID, string storeName, 
            string? logoStore)
        {
            this.sellerID = sellerID;
            this.personID = personID;
            this.storeName = storeName;
            this.logoStore = logoStore;
            Mode = enMode.UpdateSeller;
        }

        clsSellerBusiness(int personID, string fullName,
            string email, string? phone, string? addressPerson, 
            string role, string storeName, string? logoStore)
        {
            this.personID = personID;
            this.storeName = storeName;
            this.logoStore = logoStore;
            this.fullName = fullName;
            this.email = email;
            this.phone = phone;
            this.addressPerson = addressPerson;
            this.role = role;
            Mode = enMode.UpdateSeller;
        }

        public static IEnumerable<clsSellerDTO> GetAllSellers()
        {
            return clsSellerData.GetAllSellers();
        }


        public static clsSellerBusiness? FindSellerByID(int sellerID)
        {
            clsSellerDTO sellerDTO = clsSellerData.GetSellerByID(sellerID);

            if (sellerDTO != null && sellerDTO.sellerID > 0)
            {
                return new clsSellerBusiness(sellerDTO.sellerID, sellerDTO.personID, 
                    sellerDTO.storeName, sellerDTO.logoStore);
            }
            return null;
        }

        public static clsSellerBusiness? FindSellerByPersonID(int personID)
        {
            clsSellerDTO sellerDTO = clsSellerData.GetSellerByPersonID(personID);

            if (sellerDTO != null && sellerDTO.personID > 0)
            {
                return new clsSellerBusiness(sellerDTO.personID, sellerDTO.fullName, sellerDTO.email,
                        sellerDTO.phone, sellerDTO.addressPerson, 
                        sellerDTO.role, sellerDTO.storeName, sellerDTO.logoStore);
            }
            return null;
        }


        bool _AddNewSeller()
        {
            this.password = BCrypt.Net.BCrypt.HashPassword(this.password);
            this.personID = clsSellerData.AddNewSeller(addSellerDTO);
            return (this.personID > 0);
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


        public static bool SignUp(clsSellerDTO signUPSellerDTO)
        {
            signUPSellerDTO.password = BCrypt.Net.BCrypt.HashPassword(signUPSellerDTO.password);

            return clsSellerData.SellerSignUp(signUPSellerDTO);
        }

        public static bool DeleteSeller(int sellerID)
        {
            return clsSellerData.DeleteSeller(sellerID);
        }


    }
}
