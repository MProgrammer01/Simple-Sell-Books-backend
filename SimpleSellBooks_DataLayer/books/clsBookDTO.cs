using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.books
{
    public class clsBookDTO
    {
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


        public clsBookDTO() {
            title = string.Empty;
            author = string.Empty;
        }

        //DTO Retrive Data
        public clsBookDTO(int bookID, int sellerId,
            int categoryId, string title, string author,
            string? description, decimal price, int stock, int conditionId,
            string? coverImg, int statusId, DateTime createdAt, 
            DateTime updatedAt)
        {
            bookId = bookID;
            this.sellerId = sellerId;
            this.categoryId = categoryId;
            this.title = title;
            this.author = author;
            bookDescription = description;
            this.price = price;
            this.stock = stock;
            this.conditionId = conditionId;
            this.coverImg = coverImg;
            this.statusId = statusId;
            this.createdAt = createdAt;
            UpdatedAt = updatedAt;
        }

        //DTO Add New
        public clsBookDTO(int sellerId,
            int categoryId, string title, string author,
            string? description, decimal price, int stock, int conditionId,
            string? coverImg, int statusId)
        {
            this.sellerId = sellerId;
            this.categoryId = categoryId;
            this.title = title;
            this.author = author;
            this.bookDescription = description;
            this.price = price;
            this.stock = stock;
            this.conditionId = conditionId;
            this.coverImg = coverImg;
            this.statusId = statusId;
        }

        //DTO Update
        public clsBookDTO(int bookID, int sellerId,
            int categoryId, string title, string author,
            string? description, decimal price, int stock, int conditionId,
            string? coverImg, int statusId, DateTime updatedAt)
        {
            bookId = bookID;
            this.sellerId = sellerId;
            this.categoryId = categoryId;
            this.title = title;
            this.author = author;
            this.bookDescription = description;
            this.price = price;
            this.stock = stock;
            this.conditionId = conditionId;
            this.coverImg = coverImg;
            this.statusId = statusId;
            this.UpdatedAt = updatedAt;
        }
    }
}
