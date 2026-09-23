using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.books
{
    public class clsPaginatedBooksDTO
    {
        public IEnumerable<clsBookDTO> books { get; set;  } = Enumerable.Empty<clsBookDTO>();
        public int totalCount { get; set; }
        //public int pageNumber { get; set; }
        //public int pageSize { get; set; }

        public clsPaginatedBooksDTO() { }

        public clsPaginatedBooksDTO(IEnumerable<clsBookDTO> books, int totalCount
            //int pageNumber, int pageSize
            )
        {
            this.books = books;
            this.totalCount = totalCount;
            //this.pageNumber = pageNumber;
            //this.pageSize = pageSize;
        }
    }
}
