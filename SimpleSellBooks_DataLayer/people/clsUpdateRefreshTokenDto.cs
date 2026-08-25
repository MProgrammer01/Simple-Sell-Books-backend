using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsUpdateRefreshTokenDto
    {
        public int personId { get; set; }
        public string RefreshTokenHash { get; set; } = string.Empty;
        public DateTime RefreshTokenExpirationDate { get; set; }
    }
}
