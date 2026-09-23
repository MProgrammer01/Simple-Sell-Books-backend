using Microsoft.Data.SqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SimpleSellBooks_DataLayer.books
{
    public class clsBookData
    {
        static SqlConnection connectionToDB = new SqlConnection(clsConnectToDB.ConnectionToDB);

        public static IEnumerable<clsBookDTO> GetAllBooks()
        {
            List<clsBookDTO> booksList = new List<clsBookDTO>();

            string SP_GetAllBooks = "SP_GetAllBooks";

            SqlCommand command = new SqlCommand(SP_GetAllBooks, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connectionToDB.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    int BookId = (int)reader["bookID"];
                    int SellerId = (int)reader["sellerID"];
                    int CategoryId = (int)reader["categoryID"];
                    string Title = (string)reader["title"];
                    string Author = (string)reader["author"];
                    string? BookDescription = reader.IsDBNull(reader.GetOrdinal("bookDescription")) ? null :
                                           (string)reader["bookDescription"];
                    decimal Price = (decimal)reader["price"];
                    int Stock = (int)reader["stock"];
                    int ConditionId = (int)reader["conditionID"];
                    string? CoverImg = reader.IsDBNull(reader.GetOrdinal("coverImg")) ? null :
                                            (string)reader["coverImg"];
                    int StatusId = (int)reader["statusID"];
                    DateTime CreatedAt = (DateTime)reader["createdAt"];
                    DateTime UpdatedAt = (DateTime)reader["updatedAt"];

                    booksList.Add(new clsBookDTO(BookId, SellerId, CategoryId, Title, Author,
                        BookDescription, Price, Stock, ConditionId, CoverImg, StatusId, CreatedAt, UpdatedAt));
                
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllBooks Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return booksList;
        }

        public static clsPaginatedBooksDTO GetAllBooksByPersonID(int personID, 
            int pageNumber,
            int pageSize)
        {
            List<clsBookDTO> booksList = new List<clsBookDTO>();

            int totalCount = 0;

            string SP_GetAllBooksByPersonID = "SP_GetAllBooksByPersonID";

            SqlCommand command = new SqlCommand(SP_GetAllBooksByPersonID, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@pageNumber", pageNumber);
            command.Parameters.AddWithValue("@pageSize", pageSize);


            try
            {
                connectionToDB.Open();

                SqlDataReader reader = command.ExecuteReader();

                // Result Set 1: totalCount
                if (reader.Read())
                {
                    totalCount = (int)reader["totalCount"];
                }

                // Move to Result Set 2: books
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        int BookId = (int)reader["bookID"];
                        int SellerId = (int)reader["sellerID"];
                        int CategoryId = (int)reader["categoryID"];
                        string Title = (string)reader["title"];
                        string Author = (string)reader["author"];
                        string? BookDescription = reader.IsDBNull(reader.GetOrdinal("bookDescription")) ? null :
                                               (string)reader["bookDescription"];
                        decimal Price = (decimal)reader["price"];
                        int Stock = (int)reader["stock"];
                        int ConditionId = (int)reader["conditionID"];
                        string? CoverImg = reader.IsDBNull(reader.GetOrdinal("coverImg")) ? null :
                                                (string)reader["coverImg"];
                        int StatusId = (int)reader["statusID"];
                        DateTime CreatedAt = (DateTime)reader["createdAt"];
                        DateTime UpdatedAt = (DateTime)reader["updatedAt"];

                        booksList.Add(new clsBookDTO(BookId, SellerId, CategoryId, Title, Author,
                            BookDescription, Price, Stock, ConditionId, CoverImg, StatusId, CreatedAt, UpdatedAt));

                    }
                }
                reader.Close();
            }

            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllBooksByPersonID Data " + personID + " " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return new clsPaginatedBooksDTO(booksList, totalCount);
        }


        public static int AddNewBook(clsBookDTO bookDTO)
        {
            int InsertedID = 0;

            string SP_AddNewBook = "SP_AddNewBook";

            SqlCommand command = new SqlCommand(SP_AddNewBook, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sellerID", bookDTO.sellerId);
            command.Parameters.AddWithValue("@categoryID", bookDTO.categoryId);
            command.Parameters.AddWithValue("@title", bookDTO.title);
            command.Parameters.AddWithValue("@author", bookDTO.author);
            command.Parameters.AddWithValue("@bookDescription", bookDTO.bookDescription ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@price", bookDTO.price);
            command.Parameters.AddWithValue("@stock", bookDTO.stock);
            command.Parameters.AddWithValue("@conditionID", bookDTO.conditionId);
            command.Parameters.AddWithValue("@coverImg", bookDTO.coverImg ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@statusID", bookDTO.statusId);
            var outputIdParam = new SqlParameter("@NewBookID", SqlDbType.Int)
            {
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputIdParam);

            try
            {
                connectionToDB.Open();

                int rows = command.ExecuteNonQuery();


                if (rows > 0)
                {
                    InsertedID = (int)outputIdParam.Value;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: AddNewBook Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return InsertedID;
        }

        public static bool UpdateBook(clsBookDTO bookDTO)
        {
            bool isUpdated = false;

            string SP_UpdateBook = "SP_UpdateBook";


            SqlCommand command = new SqlCommand(SP_UpdateBook, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@bookID", bookDTO.bookId);
            command.Parameters.AddWithValue("@categoryID", bookDTO.categoryId);
            command.Parameters.AddWithValue("@title", bookDTO.title);
            command.Parameters.AddWithValue("@author", bookDTO.author);
            command.Parameters.AddWithValue("@bookDescription", bookDTO.bookDescription ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@price", bookDTO.price);
            command.Parameters.AddWithValue("@stock", bookDTO.stock);
            command.Parameters.AddWithValue("@conditionID", bookDTO.conditionId);
            command.Parameters.AddWithValue("@coverImg", bookDTO.coverImg ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@statusID", bookDTO.statusId);

            try
            {
                connectionToDB.Open();
                int rowsAffected = command.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine($"rowsAffected : {rowsAffected}");
                if (rowsAffected > 0)
                {
                    isUpdated = true;
                    System.Diagnostics.Debug.WriteLine($"isUpdated : {isUpdated}");
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: UpdateBook Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            System.Diagnostics.Debug.WriteLine($"isUpdated : {isUpdated}");
            return isUpdated;
        }

        public static bool DeleteBook(int bookID)
        {
            bool isDeleted = false;

            string SP_DeleteBook = "SP_DeleteBook";

            SqlCommand command = new SqlCommand(SP_DeleteBook, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@bookID", bookID);

            try
            {
                connectionToDB.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isDeleted = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : DeleteBook Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return isDeleted;
        }

        public static clsBookDTO GetBookById(int bookId)
        {
            clsBookDTO bookDTO = new clsBookDTO();

            string SP_GetBookById = "SP_GetBookById";

            SqlCommand command = new SqlCommand(SP_GetBookById, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@bookId", bookId);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int BookId = (int)reader["bookID"];
                    int SellerId = (int)reader["sellerID"];
                    int CategoryId = (int)reader["categoryID"];
                    string Title = (string)reader["title"];
                    string Author = (string)reader["author"];
                    string? BookDescription = reader.IsDBNull(reader.GetOrdinal("bookDescription")) ? null :
                                           (string)reader["bookDescription"];
                    decimal Price = (decimal)reader["price"];
                    int Stock = (int)reader["stock"];
                    int ConditionId = (int)reader["conditionID"];
                    string? CoverImg = reader.IsDBNull(reader.GetOrdinal("coverImg")) ? null :
                                            (string)reader["coverImg"];
                    int StatusId = (int)reader["statusID"];
                    DateTime CreatedAt = (DateTime)reader["createdAt"];
                    DateTime UpdatedAt = (DateTime)reader["updatedAt"];

                    bookDTO = new clsBookDTO(BookId, SellerId, CategoryId, Title, Author,
                        BookDescription, Price, Stock, ConditionId, CoverImg, StatusId, CreatedAt, UpdatedAt);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetBookById Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return bookDTO;
        }

    }
}
