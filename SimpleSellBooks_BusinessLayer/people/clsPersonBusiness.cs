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
        public string password { get; set; }
        public string? phone { get; set; }
        public string? addressPerson { get; set; }
        public string role { get; set; }
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

        clsSignUpDTO personSignUpDTO
        {
            get
            {
                return new clsSignUpDTO(
                this.fullName,
                this.email,
                this.password,
                this.phone,
                this.addressPerson
                );
            }
        }

        clsUpdatePersonDTO personUpdateDTO
        {
            get
            {
                return new clsUpdatePersonDTO(
                    this.personID,
                this.fullName,
                this.email,
                this.phone,
                this.addressPerson
                );
            }
        }

        clsSignInDTO signInDTO
        {
            get
            {
                return new clsSignInDTO(this.email, this.password);
            }
        }

        // Empty constructor
        public clsPersonBusiness()
        {
            this.personID = 0;
            this.fullName = string.Empty;
            this.email = string.Empty;
            this.password = string.Empty;
            this.phone = string.Empty;
            this.addressPerson = string.Empty;
            this.role = string.Empty;
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;

            Mode = enMode.AddNewPerson;
        }

        // Constructor for existing Person
        clsPersonBusiness(clsPersonDTO personDTO)
        {
            this.personID = personDTO.personID;
            this.fullName = personDTO.fullName;
            this.email = personDTO.email;
            this.password = string.Empty;
            this.phone = personDTO.phone;
            this.addressPerson = personDTO.addressPerson;
            this.role = personDTO.role;
            this.createdAt = personDTO.createdAt;
            this.updatedAt = personDTO.updatedAt;

            Mode = enMode.UpdatePerson;
        }

        // Add New Person


        public static IEnumerable<clsPersonDTO> GetAllPersons()
        {
            return clsPersonData.GetAllPeople();
        }


        public static clsPersonBusiness? FindPersonByID(int personID)
        {
            clsPersonDTO personDTO = clsPersonData.GetPersonByID(personID);

            if (personDTO != null && personDTO.personID > 0)
            {
                return new clsPersonBusiness(personDTO);
            }
            return null;
        }

        public static clsSignInResponseDTO? Login(clsSignInDTO signInDTO)
        {
            clsSignInResponseDTO personAuthDTO = clsPersonData.SignInResponse(signInDTO.email);
            
            if (personAuthDTO == null || personAuthDTO.personID <= 0)
            {
                return null;
            }

            bool isValidPassword = BCrypt.Net.BCrypt.Verify(
                     signInDTO.password,
                     personAuthDTO.passwordHash
                 );

            if (!isValidPassword)
                return null;

            return new clsSignInResponseDTO(
                personAuthDTO.personID,
                personAuthDTO.email,
                personAuthDTO.role
            );
        }


        public static clsSignInResponseDTO? RefreshTokenResponce(string email)
        {
            clsSignInResponseDTO refreshTokenResp = clsPersonData.RefreshTokenResponce(email);


            if (refreshTokenResp == null || refreshTokenResp.personID <= 0)
            {
                return null;
            }

            return refreshTokenResp;
        }

        bool _AddNewPerson()
        {
            this.password = BCrypt.Net.BCrypt.HashPassword(this.password);
            this.personID = clsPersonData.AddNewPerson(personSignUpDTO);
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

        public static bool UpdateRefreshToken(clsUpdateRefreshTokenDto refreshToken)
        {
            return clsPersonData.UpdateRefreshToken(
                refreshToken
            );
        }
    }
}
