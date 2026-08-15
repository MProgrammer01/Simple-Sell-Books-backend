using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.categories
{
    internal class clsCategorieData
    {
        static SqlConnection connectionToDB = new SqlConnection(clsConnectToDB.ConnectionToDB);

        public static IEnumerable<clsCategorieDTO> GetAllCategories()
        {
            List<clsCategorieDTO> listCategories = new List<clsCategorieDTO>();

            string SP_GetAllCategories = "SP_GetAllCategories";

            SqlCommand command = new SqlCommand(SP_GetAllCategories, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int categoryID = (int)reader["categoryID"];
                        string categoryName = (string)reader["categoryName"];
                        string? categoryDescription = reader.IsDBNull(reader.GetOrdinal("categoryDescription")) ? null :
                                            (string)reader["categoryDescription"];
                        DateTime createdAt = (DateTime)reader["createdAt"];
                        DateTime updatedAt = (DateTime)reader["updatedAt"];

                        listCategories.Add(new clsCategorieDTO(categoryID, categoryName, categoryDescription, createdAt, updatedAt));

                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllCategories Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return listCategories;
        }

        public static clsCategorieDTO GetCategorieByID(int categoryID)
        {
            clsCategorieDTO categorieDTO = new clsCategorieDTO();

            string SP_GetCategorieByID = "SP_GetCategorieByID";

            SqlCommand command = new SqlCommand(SP_GetCategorieByID, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@categoryID", categoryID);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    categoryID = (int)reader["categoryID"];
                    string categoryName = (string)reader["categoryName"];
                    string? categoryDescription = reader.IsDBNull(reader.GetOrdinal("categoryDescription")) ? null :
                                            (string)reader["categoryDescription"];
                    DateTime createdAt = (DateTime)reader["createdAt"];
                    DateTime updatedAt = (DateTime)reader["updatedAt"];

                    categorieDTO = new clsCategorieDTO(categoryID, categoryName, categoryDescription, createdAt, updatedAt);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetCategorieByID Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return categorieDTO;
        }
    }
}
