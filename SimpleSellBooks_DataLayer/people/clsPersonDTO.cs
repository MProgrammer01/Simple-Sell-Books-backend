using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsPersonDTO
    {
        public int personID { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string? phone { get; set; }
        public string? addressPerson { get; set; }
        public string role { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }


        public clsPersonDTO()
        {
            fullName = string.Empty;
            email = string.Empty;
            role = string.Empty;

        }

        //get All And Get By ID
        public clsPersonDTO(int personID, string fullName, 
            string email, string? phone, string? addressPerson, string role, DateTime createdAt, DateTime updatedAt)
        {
            this.personID = personID;
            this.fullName = fullName;
            this.email = email;
            this.phone = phone;
            this.addressPerson = addressPerson;
            this.role = role;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
        }

        ////Get By Email
        //public clsPersonDTO(int personID, string fullName,
        //    string email, string password, string? phone, string? addressPerson, string role, 
        //    DateTime createdAt, DateTime updatedAt)
        //{
        //    this.personID = personID;
        //    this.fullName = fullName;
        //    this.email = email;
        //    this.password = password;
        //    this.phone = phone;
        //    this.addressPerson = addressPerson;
        //    this.role = role;
        //    this.createdAt = createdAt;
        //    this.updatedAt = updatedAt;
        //}

        ////Add Person / Sign Up
        //public clsPersonDTO(string fullName, string email, string passwordHash, 
        //    string? phone, string? addressPerson
        //    //, string? role
        //    )
        //{
        //    this.fullName = fullName;
        //    this.email = email;
        //    this.password = passwordHash;
        //    this.phone = phone;
        //    this.addressPerson = addressPerson;
        //    this.role = String.Empty;
        //}

        ////Update Person
        //public clsPersonDTO(int personID, string fullName, string email, string? phone, 
        //    string? addressPerson
        //    //, string? role
        //    )
        //{
        //    this.personID = personID;
        //    this.fullName = fullName;
        //    this.email = email;
        //    this.password = string.Empty;
        //    this.phone = phone;
        //    this.addressPerson = addressPerson;
        //    this.role = String.Empty;
        //}

        ////Change Password DTO
        //public clsPersonDTO(int personID, string newPasswordHash)
        //{
        //    this.personID = personID;
        //    this.password = newPasswordHash;
        //    this.fullName = string.Empty;
        //    this.email = string.Empty;
        //    this.role = String.Empty;

        //}

        ////Sign In
        //public clsPersonDTO(string email, string password)
        //{
        //    this.email = email;
        //    this.password = password;
        //    this.fullName = string.Empty;
        //    this.role = String.Empty;

        //}

        ////Sign In Response
        //public clsPersonDTO(int personID, string email, string password, string role)
        //{
        //    this.personID = personID;
        //    this.email = email;
        //    this.password = password;
        //    this.role = role;
        //    this.fullName = string.Empty;
        //}
    }
}
