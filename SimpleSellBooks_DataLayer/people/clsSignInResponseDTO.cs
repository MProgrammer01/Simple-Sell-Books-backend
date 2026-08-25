using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsSignInResponseDTO
    {
        public int personID { get; set; }
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string passwordHash { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;

        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime? RefreshTokenExpirationDate { get; set; }
        public DateTime? RefreshTokenRevokedAt { get; set; }

        public clsSignInResponseDTO() { }

        public clsSignInResponseDTO(int personID,string email, string role)
        { 
            this.personID = personID;
            this.email = email;
            this.role = role;
        }

        public clsSignInResponseDTO(int personID, string email, string role, 
            string passwordHash) 
        {
            this.personID = personID;
            this.email = email;
            this.role = role;
            this.passwordHash = passwordHash;
        }

        public clsSignInResponseDTO(int personID, string email, string role,
            string refreshTokenHash, 
            DateTime? refreshTokenExpirationDate, DateTime? refreshTokenRevokedAt)
        {
            this.personID = personID;
            this.email = email;
            this.role = role;
            this.RefreshTokenHash = refreshTokenHash;
            this.RefreshTokenExpirationDate = refreshTokenExpirationDate;
            this.RefreshTokenRevokedAt = refreshTokenRevokedAt;
        }

    }
}
