using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.conditions
{
    public class clsConditionDTO
    {
        public int conditionID { get; set; }
        public string conditionName { get; set; }


        public clsConditionDTO()
        {
            conditionName = string.Empty;
        }

        public clsConditionDTO(int conditionID, string conditionName)
        {
            this.conditionID = conditionID;
            this.conditionName = conditionName;

        }
    }
}
