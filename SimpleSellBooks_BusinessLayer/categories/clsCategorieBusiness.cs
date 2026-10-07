using SimpleSellBooks_DataLayer.categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.categories
{
    public class clsCategorieBusiness
    {
        enum enMode { AddNewCategorie = 1, UpdateCategorie = 2 }
        enMode Mode = enMode.AddNewCategorie;

        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string? categoryDescription { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }


        public clsCategorieDTO categorieDTO
        {
            get
            {
                return new clsCategorieDTO(this.categoryID, this.categoryName, 
                    this.categoryDescription
                    //, this.createdAt, this.updatedAt
                    );
            }
        }

        public clsCategorieBusiness()
        {
            this.categoryID = 0;
            this.categoryName = string.Empty;
            this.categoryDescription = string.Empty;
            this.createdAt = DateTime.Now;
            this.updatedAt = DateTime.Now;

            Mode = enMode.AddNewCategorie;
        }

        clsCategorieBusiness(clsCategorieDTO categorieDTO)
        {
            this.categoryID = categorieDTO.categoryID;
            this.categoryName = categorieDTO.categoryName;
            this.categoryDescription = categorieDTO.categoryDescription;
            //this.createdAt = categorieDTO.createdAt;
            //this.updatedAt = categorieDTO.updatedAt;

            Mode = enMode.UpdateCategorie;
        }

        public static IEnumerable<clsCategorieDTO> GetAllCategories()
        {
            return clsCategorieData.GetAllCategories();
        }


        public static clsCategorieBusiness? FindCategorie(int categoryID)
        {
            clsCategorieDTO categorieDTO = clsCategorieData.GetCategorieByID(categoryID);

            if (categorieDTO != null && categorieDTO.categoryID > 0)
            {
                return new clsCategorieBusiness(categorieDTO);
            }
            return null;
        }


        //bool _AddNewCategorie()
        //{
        //    this.categoryID = clsCategorieData.AddNewCategorie(categorieDTO);
        //    return (this.categoryID > 0);
        //}


        //bool _UpdateCategorie()
        //{
        //    return clsCategorieData.UpdateCategorie(categorieDTO);
        //}

        //public bool Save()
        //{
        //    switch (Mode)
        //    {
        //        case enMode.AddNewCategorie:
        //            if (_AddNewCategorie())
        //            {
        //                Mode = enMode.UpdateCategorie;
        //                return true;
        //            }
        //            else
        //            {
        //                return false;
        //            }

        //        case enMode.UpdateCategorie:
        //            return _UpdateCategorie();

        //    }

        //    return false;
        //}



        //public static bool DeleteCategorie(int categoryID)
        //{
        //    return clsCategorieData.DeleteCategorie(categoryID);
        //}
    }
}
