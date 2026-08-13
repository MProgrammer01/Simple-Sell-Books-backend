using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace SimpleSellBooks_DataLayer.books
{
    internal class clsBookData
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
                    clsBookDTO book = new clsBookDTO
                    {
                        BookId = Convert.ToInt32(reader["bookID"]),
                        SellerId = Convert.ToInt32(reader["sellerID"]),
                        CategoryId = Convert.ToInt32(reader["categoryID"]),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        Author = reader.GetString(reader.GetOrdinal("author")),
                        BookDescription = reader.IsDBNull(reader.GetOrdinal("bookDescription")) ? null :
                                            reader["bookDescription"].ToString(),
                        Price = Convert.ToDecimal(reader["price"]),
                        Stock = Convert.ToInt32(reader["stock"]),
                        ConditionId = Convert.ToInt32(reader["conditionID"]),
                        CoverImg = reader.IsDBNull(reader.GetOrdinal("coverImg")) ? null :
                                            reader["coverImg"].ToString(),
                        StatusId = Convert.ToInt32(reader["statusID"]),
                        CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["updatedAt"]),
                    };

                    booksList.Add(book);
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

        public static int AddNewBook(clsBookDTO bookDTO)
        {
            int InsertedID = 0;

            string SP_AddNewBook = "SP_AddNewBook";

            SqlCommand command = new SqlCommand(SP_AddNewBook, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@sellerID", bookDTO.SellerId);
            command.Parameters.AddWithValue("@categoryID", bookDTO.CategoryId);
            command.Parameters.AddWithValue("@title", bookDTO.Title);
            command.Parameters.AddWithValue("@author", bookDTO.Author);
            command.Parameters.AddWithValue("@bookDescription", bookDTO.BookDescription ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@price", bookDTO.Price);
            command.Parameters.AddWithValue("@stock", bookDTO.Stock);
            command.Parameters.AddWithValue("@conditionID", bookDTO.ConditionId);
            command.Parameters.AddWithValue("@coverImg", bookDTO.CoverImg ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@statusID", bookDTO.StatusId);
            command.Parameters.AddWithValue("@createdAt", bookDTO.CreatedAt);
            command.Parameters.AddWithValue("@updatedAt", bookDTO.UpdatedAt);
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

            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@bookID", bookDTO.BookId);
            command.Parameters.AddWithValue("@sellerID", bookDTO.SellerId);
            command.Parameters.AddWithValue("@categoryID", bookDTO.CategoryId);
            command.Parameters.AddWithValue("@title", bookDTO.Title);
            command.Parameters.AddWithValue("@author", bookDTO.Author);
            command.Parameters.AddWithValue("@bookDescription", bookDTO.BookDescription ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@price", bookDTO.Price);
            command.Parameters.AddWithValue("@stock", bookDTO.Stock);
            command.Parameters.AddWithValue("@conditionID", bookDTO.ConditionId);
            command.Parameters.AddWithValue("@coverImg", bookDTO.CoverImg ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@statusID", bookDTO.StatusId);
            command.Parameters.AddWithValue("@createdAt", bookDTO.CreatedAt);
            command.Parameters.AddWithValue("@updatedAt", bookDTO.UpdatedAt);

            try
            {
                connectionToDB.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    isUpdated = true;
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
                    bookDTO = new clsBookDTO
                    {
                        BookId = Convert.ToInt32(reader["bookID"]),
                        SellerId = Convert.ToInt32(reader["sellerID"]),
                        CategoryId = Convert.ToInt32(reader["categoryID"]),
                        Title = reader.GetString(reader.GetOrdinal("title")),
                        Author = reader.GetString(reader.GetOrdinal("author")),
                        BookDescription = reader.IsDBNull(reader.GetOrdinal("bookDescription")) ? null :
                                            reader["bookDescription"].ToString(),
                        Price = Convert.ToDecimal(reader["price"]),
                        Stock = Convert.ToInt32(reader["stock"]),
                        ConditionId = Convert.ToInt32(reader["conditionID"]),
                        CoverImg = reader.IsDBNull(reader.GetOrdinal("coverImg")) ? null :
                                            reader["coverImg"].ToString(),
                        StatusId = Convert.ToInt32(reader["statusID"]),
                        CreatedAt = Convert.ToDateTime(reader["createdAt"]),
                        UpdatedAt = Convert.ToDateTime(reader["updatedAt"]),
                    };
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
