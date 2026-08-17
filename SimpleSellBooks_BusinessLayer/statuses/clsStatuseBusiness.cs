using SimpleSellBooks_DataLayer.statuses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.statuses
{
    public class clsStatuseBusiness
    {
        enum enMode { AddNewStatuse = 1, UpdateStatuse = 2 }
        enMode Mode = enMode.AddNewStatuse;

        public int statusID { get; set; }
        public string statusName { get; set; }


        public clsStatuseDTO statuseDTO
        {
            get
            {
                return new clsStatuseDTO(this.statusID, this.statusName);
            }
        }

        public clsStatuseBusiness()
        {
            this.statusID = 0;
            this.statusName = string.Empty;

            Mode = enMode.AddNewStatuse;
        }

        clsStatuseBusiness(clsStatuseDTO statuseDTO)
        {
            this.statusID = statuseDTO.statusID;
            this.statusName = statuseDTO.statusName;

            Mode = enMode.UpdateStatuse;
        }

        public static IEnumerable<clsStatuseDTO> GetAllStatuses()
        {
            return clsStatuseData.GetAllStatuses();
        }

        public static clsStatuseBusiness? FindStatuse(int statusID)
        {
            clsStatuseDTO statuseDTO = clsStatuseData.GetStatuseByID(statusID);

            if (statuseDTO != null && statuseDTO.statusID > 0)
            {
                return new clsStatuseBusiness(statuseDTO);
            }
            return null;
        }


        //bool _AddNewStatuse()
        //{
        //    this.statusID = clsStatuseData.AddNewStatuse(statuseDTO);
        //    return (this.statusID > 0);
        //}


        //bool _UpdateStatuse()
        //{
        //    return clsStatuseData.UpdateStatuse(statuseDTO);
        //}

        //public bool Save()
        //{
        //    switch (Mode)
        //    {
        //        case enMode.AddNewStatuse:
        //            if (_AddNewStatuse())
        //            {
        //                Mode = enMode.UpdateStatuse;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        case enMode.UpdateStatuse:
        //            return _UpdateStatuse();

        //    }

        //    return false;
        //}



        //public static bool DeleteStatuse(int statusID)
        //{
        //    return clsStatuseData.DeleteStatuse(statusID);
        //}
    }
}
