using SimpleSellBooks_DataLayer.people;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.people
{
    public class clsPersonBusiness
    {
        enum enMode { AddNewPerson = 1, UpdatePerson = 2 }
        enMode Mode = enMode.AddNewPerson;

        public int personID { get; set; }
        public string fullName { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string? phone { get; set; }
        public string? addressPerson { get; set; }
        public string? role { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }


        public clsPersonDTO personDTO
        {
            get
            {
                return new clsPersonDTO(
                    this.personID,
                    this.fullName,
                    this.email,
                    this.phone,
                    this.addressPerson,
                    this.role,
                    this.createdAt,
                    this.updatedAt);
            }
        }

        clsPersonDTO personAddOrSignUpDTO
        {
            get
            {
                return new clsPersonDTO(
                this.fullName,
                this.email,
                this.passwordHash,
                this.phone,
                this.addressPerson
                //, this.role
                );
            }
        }

        clsPersonDTO personUpdateDTO
        {
            get
            {
                return new clsPersonDTO(
                    this.personID,
                this.fullName,
                this.email,
                this.phone,
                this.addressPerson
                //, this.role
                );
            }
        }

        clsPersonDTO changePasswordDTO
        {
            get
            {
                return new clsPersonDTO(
                    this.personID,
                this.passwordHash);
            }
        }

        clsPersonDTO signInDTO
        {
            get
            {
                return new clsPersonDTO(
                     this.email,
                this.passwordHash);
            }
        }

        clsPersonDTO signInResponse
        {
            get
            {
                return new clsPersonDTO(
                     this.personID,
                this.fullName,
                this.email);
            }
        }

        public clsPersonBusiness()
        {
            this.personID = 0;
            this.fullName = string.Empty;
            this.email = string.Empty;
            this.passwordHash = string.Empty;
            this.phone = string.Empty;
            this.addressPerson = string.Empty;
            this.role = string.Empty;
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;

            Mode = enMode.AddNewPerson;
        }

        clsPersonBusiness(clsPersonDTO personDTO)
        {
            this.personID = personDTO.personID;
            this.fullName = personDTO.fullName;
            this.email = personDTO.email;
            this.passwordHash = String.Empty;
            this.phone = personDTO.phone;
            this.addressPerson = personDTO.addressPerson;
            this.role = personDTO.role;
            this.createdAt = personDTO.createdAt;
            this.updatedAt = personDTO.updatedAt;

            Mode = enMode.UpdatePerson;
        }

        public static IEnumerable<clsPersonDTO> GetAllPersons()
        {
            return clsPersonData.GetAllPeople();
        }


        public static clsPersonBusiness? FindPerson(int personID)
        {
            clsPersonDTO personDTO = clsPersonData.GetPersonByID(personID);

            if (personDTO != null && personDTO.personID > 0)
            {
                return new clsPersonBusiness(personDTO);
            }
            return null;
        }


        bool _AddNewPerson()
        {
            this.passwordHash = BCrypt.Net.BCrypt.HashPassword(this.passwordHash);
            this.personID = clsPersonData.AddNewPerson(personAddOrSignUpDTO);
            return (this.personID > 0);
        }


        bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(personUpdateDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewPerson:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.UpdatePerson;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.UpdatePerson:
                    return _UpdatePerson();

            }

            return false;
        }



        public static bool DeletePerson(int personID)
        {
            return clsPersonData.DeletePerson(personID);
        }
    }
}
