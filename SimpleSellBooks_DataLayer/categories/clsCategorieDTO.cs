using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.categories
{
    internal class clsCategorieDTO
    {
        public int categoryID { get; set; }
        public string categoryName { get; set; }
        public string? categoryDescription { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public clsCategorieDTO()
        {
            categoryName = string.Empty;

        }

        public clsCategorieDTO(int categoryID, string categoryName, string? categoryDescription, DateTime createdAt, DateTime updatedAt)
        {
            this.categoryID = categoryID;
            this.categoryName = categoryName;
            this.categoryDescription = categoryDescription;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;

        }

    }
}
