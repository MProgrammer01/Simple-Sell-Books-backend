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
        public string fullName { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string? phone { get; set; }
        public string? addressPerson { get; set; }
        //public bool isAdmin { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }


        public clsPersonDTO()
        {
            fullName = string.Empty;
            email = string.Empty;
            passwordHash = string.Empty;

        }

        //get All And Get By
        public clsPersonDTO(int personID, string fullName, 
            string email, string? phone, string? addressPerson, DateTime createdAt, DateTime updatedAt)
        {
            this.personID = personID;
            this.fullName = fullName;
            this.email = email;
            this.passwordHash = string.Empty;
            this.phone = phone;
            this.addressPerson = addressPerson;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;

        }

        //Add Person / Sign Up
        public clsPersonDTO(string fullName, string email, string passwordHash, 
            string? phone, string? addressPerson)
        {
            this.fullName = fullName;
            this.email = email;
            this.passwordHash = passwordHash;
            this.phone = phone;
            this.addressPerson = addressPerson;

        }

        //Update Person
        public clsPersonDTO(int personID, string fullName, string email, string? phone, 
            string? addressPerson)
        {
            this.personID = personID;
            this.fullName = fullName;
            this.email = email;
            this.passwordHash = string.Empty;
            this.phone = phone;
            this.addressPerson = addressPerson;

        }

        //Change Password DTO
        public clsPersonDTO(int personID, string newPasswordHash)
        {
            this.personID = personID;
            this.passwordHash = newPasswordHash;
            this.fullName = string.Empty;
            this.email = string.Empty;

        }

        //Sign In
        public clsPersonDTO(string email, string passwordHash)
        {
            this.email = email;
            this.passwordHash = passwordHash;
            this.fullName = string.Empty;

        }

        //Sign In Response
        public clsPersonDTO(int personID, string fullName, string email)
        {
            this.personID = personID;
            this.fullName = fullName;
            this.email = email;
            this.passwordHash = string.Empty;

        }
    }
}
