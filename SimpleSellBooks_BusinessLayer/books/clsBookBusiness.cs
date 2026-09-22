using SimpleSellBooks_BusinessLayer.categories;
using SimpleSellBooks_BusinessLayer.conditions;
using SimpleSellBooks_BusinessLayer.sellers;
using SimpleSellBooks_BusinessLayer.statuses;
using SimpleSellBooks_DataLayer.books;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_BusinessLayer.books
{
    public class clsBookBusiness
    {
        enum enMode { AddNewBook = 1, UpdateBook = 2 }
        enMode Mode = enMode.AddNewBook;

        public int bookId { get; set; }
        public int sellerId { get; set; }
        public int categoryId { get; set; }
        public string title { get; set; }
        public string author { get; set; }
        public string? bookDescription { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public int conditionId { get; set; }
        public string? coverImg { get; set; }
        public int statusId { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public clsSellerBusiness sellerInfo;
        public clsCategorieBusiness categorieInfo;
        public clsConditionBusiness conditionInfo;
        public clsStatuseBusiness statuseInfo;


        public clsBookDTO bookDTO
        {
            get
            {
                return new clsBookDTO(this.bookId, 
                    this.sellerId, this.categoryId, 
                    this.title, this.author, this.bookDescription,
                    this.price, this.stock, this.conditionId, this.coverImg, 
                    this.statusId, this.createdAt, this.UpdatedAt);
            }
        }


        private clsBookDTO bookAddDTO
        {
            get
            {
                return new clsBookDTO(
                    this.sellerId,
                    this.categoryId,
                    this.title,
                    this.author,
                    this.bookDescription,
                    this.price,
                    this.stock,
                    this.conditionId,
                    this.coverImg,
                    this.statusId);
            }
        }


        private clsBookDTO bookUpdateDTO
        {
            get
            {
                return new clsBookDTO(
                    this.bookId,
                    this.categoryId,
                    this.title,
                    this.author,
                    this.bookDescription,
                    this.price,
                    this.stock,
                    this.conditionId,
                    this.coverImg,
                    this.statusId, 
                    0);
            }
        }

        public clsBookBusiness()
        {
            this.bookId = 0;
            this.sellerId = 0;
            this.categoryId = 0;
            this.title = string.Empty;
            this.author = string.Empty;
            this.bookDescription = string.Empty;
            this.price = 0.0m;
            this.stock = 0;
            this.conditionId = 0;
            this.coverImg = string.Empty;
            this.statusId = 0;
            this.createdAt = DateTime.Now;
            this.UpdatedAt = DateTime.Now;

            this.sellerInfo = new clsSellerBusiness();
            this.categorieInfo = new clsCategorieBusiness();
            this.conditionInfo = new clsConditionBusiness();
            this.statuseInfo = new clsStatuseBusiness();

            Mode = enMode.AddNewBook;
        }

        clsBookBusiness(clsBookDTO bookDTO)
        {
            
            this.bookId = bookDTO.bookId;
            this.sellerId = bookDTO.sellerId;
            this.categoryId = bookDTO.categoryId;
            this.title = bookDTO.title;
            this.author = bookDTO.author;
            this.bookDescription = bookDTO.bookDescription;
            this.price = bookDTO.price;
            this.stock = bookDTO.stock;
            this.conditionId = bookDTO.conditionId;
            this.coverImg = bookDTO.coverImg;
            this.statusId = bookDTO.statusId;
            this.createdAt = bookDTO.createdAt;
            this.UpdatedAt = bookDTO.UpdatedAt;

            this.sellerInfo = clsSellerBusiness.FindSeller(bookDTO.sellerId) ?? new clsSellerBusiness();
            this.categorieInfo = clsCategorieBusiness.FindCategorie(bookDTO.categoryId) ?? new clsCategorieBusiness();
            this.conditionInfo = clsConditionBusiness.FindCondition(bookDTO.conditionId) ?? new clsConditionBusiness();
            this.statuseInfo = clsStatuseBusiness.FindStatuse(bookDTO.statusId) ?? new clsStatuseBusiness();

            Mode = enMode.UpdateBook;
        }

        public static IEnumerable<clsBookDTO> GetAllBooks()
        {
            return clsBookData.GetAllBooks();
        }

        public static IEnumerable<clsBookDTO> GetAllBooksByPersonID(int personID)
        {
            return clsBookData.GetAllBooksByPersonID(personID);
        }


        public static clsBookBusiness? FindBook(int bookId)
        {
            clsBookDTO bookDTO = clsBookData.GetBookById(bookId);

            if (bookDTO != null && bookDTO.bookId > 0)
            {
                
                return new clsBookBusiness(bookDTO);
            }
            return null;
        }


        bool _AddNewBook()
        {
            this.bookId = clsBookData.AddNewBook(bookAddDTO);
            return (this.bookId > 0);
        }


        bool _UpdateBook()
        {
            return clsBookData.UpdateBook(bookUpdateDTO);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNewBook:
                    if (_AddNewBook())
                    {
                        Mode = enMode.UpdateBook;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.UpdateBook:
                    return _UpdateBook();

            }

            return false;
        }



        public static bool DeleteBook(int bookId)
        {
            return clsBookData.DeleteBook(bookId);
        }
    }
}
