using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.statuses
{
    public class clsStatuseDTO
    {
        public int statusID { get; set; }
        public string statusName { get; set; }


        public clsStatuseDTO()
        {
            statusName = string.Empty;
        }

        public clsStatuseDTO(int statusID, string statusName)
        {
            this.statusID = statusID;
            this.statusName = statusName;

        }
    }
}
