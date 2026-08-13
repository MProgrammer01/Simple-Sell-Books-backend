using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.books
{
    internal class clsBookDTO
    {
        public int BookId { get; set; }
        public int SellerId { get; set; }
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? BookDescription { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int ConditionId { get; set; }
        public string? CoverImg { get; set; }
        public int StatusId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }


        public clsBookDTO() {
            Title = string.Empty;
            Author = string.Empty;
        }

        //DTO Retrive Data
        public clsBookDTO(int bookID, int sellerId,
            int categoryId, string title, string author,
            string? description, decimal price, int stock, int conditionId,
            string? coverImg, int statusId, DateTime createdAt, 
            DateTime updatedAt)
        {
            BookId = bookID;
            SellerId = sellerId;
            CategoryId = categoryId;
            Title = title;
            Author = author;
            BookDescription = description;
            Price = price;
            Stock = stock;
            ConditionId = conditionId;
            CoverImg = coverImg;
            StatusId = statusId;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
        }

        //DTO Add New
        public clsBookDTO(int sellerId,
            int categoryId, string title, string author,
            string? description, decimal price, int stock, int conditionId,
            string? coverImg, int statusId)
        {
            SellerId = sellerId;
            CategoryId = categoryId;
            Title = title;
            Author = author;
            BookDescription = description;
            Price = price;
            Stock = stock;
            ConditionId = conditionId;
            CoverImg = coverImg;
            StatusId = statusId;
        }

        //DTO Update
        public clsBookDTO(int bookID, int sellerId,
            int categoryId, string title, string author,
            string? description, decimal price, int stock, int conditionId,
            string? coverImg, int statusId, DateTime updatedAt)
        {
            BookId = bookID;
            SellerId = sellerId;
            CategoryId = categoryId;
            Title = title;
            Author = author;
            BookDescription = description;
            Price = price;
            Stock = stock;
            ConditionId = conditionId;
            CoverImg = coverImg;
            StatusId = statusId;
            UpdatedAt = updatedAt;
        }
    }
}
