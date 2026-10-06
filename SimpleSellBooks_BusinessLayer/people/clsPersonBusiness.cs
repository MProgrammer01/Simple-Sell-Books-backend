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
       
        public int personID { get; set; }
        public string fullName { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string? phone { get; set; }
        public string? addressPerson { get; set; }
        public string role { get; set; } = string.Empty;
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }



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

        public static bool UpdateRefreshToken(clsUpdateRefreshTokenDto refreshToken)
        {
            return clsPersonData.UpdateRefreshToken(
                refreshToken
            );
        }

        public static bool IsPersonExistsByEmail(string email)
        {
            return clsPersonData.IsPersonExistsByEmail(email);
        }

        public static string getPasswordHashByPersonID(int personID)
        {
            return clsPersonData.getPasswordHashByPersonID(personID);
        }

        public static bool UpdatePasswordHash(int personID, string newPasswordHash)
        {
            newPasswordHash = BCrypt.Net.BCrypt.HashPassword(newPasswordHash);
            return clsPersonData.UpdatePasswordHash(personID, newPasswordHash);
        }
    }
}
