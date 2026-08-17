using SimpleSellBooks_DataLayer.conditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.conditions
{
    public class clsConditionBusiness
    {
        enum enMode { AddNewCondition = 1, UpdateCondition = 2 }
        enMode Mode = enMode.AddNewCondition;

        public int conditionID { get; set; }
        public string conditionName { get; set; }


        public clsConditionDTO conditionDTO
        {
            get
            {
                return new clsConditionDTO(this.conditionID, this.conditionName);
            }
        }

        public clsConditionBusiness()
        {
            this.conditionID = 0;
            this.conditionName = string.Empty;

            Mode = enMode.AddNewCondition;
        }

        clsConditionBusiness(clsConditionDTO conditionDTO)
        {
            this.conditionID = conditionDTO.conditionID;
            this.conditionName = conditionDTO.conditionName;

            Mode = enMode.UpdateCondition;
        }

        public static IEnumerable<clsConditionDTO> GetAllConditions()
        {
            return clsConditionData.GetAllConditions();
        }


        public static clsConditionBusiness? FindCondition(int conditionID)
        {
            clsConditionDTO conditionDTO = clsConditionData.GetConditionByID(conditionID);

            if (conditionDTO != null && conditionDTO.conditionID > 0)
            {
                return new clsConditionBusiness(conditionDTO);
            }
            return null;
        }


        //bool _AddNewCondition()
        //{
        //    this.conditionID = clsConditionData.AddNewCondition(conditionDTO);
        //    return (this.conditionID > 0);
        //}


        //bool _UpdateCondition()
        //{
        //    return clsConditionData.UpdateCondition(conditionDTO);
        //}

        //public bool Save()
        //{
        //    switch (Mode)
        //    {
        //        case enMode.AddNewCondition:
        //            if (_AddNewCondition())
        //            {
        //                Mode = enMode.UpdateCondition;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        case enMode.UpdateCondition:
        //            return _UpdateCondition();

        //    }

        //    return false;
        //}



        //public static bool DeleteCondition(int conditionID)
        //{
        //    return clsConditionData.DeleteCondition(conditionID);
        //}
    }
}
