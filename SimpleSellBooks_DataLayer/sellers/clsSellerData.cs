using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.sellers
{
    public class clsSellerData
    {
        static SqlConnection connectionToDB = new SqlConnection(clsConnectToDB.ConnectionToDB);

        public static IEnumerable<clsSellerDTO> GetAllSellers()
        {
            List<clsSellerDTO> listSeller = new List<clsSellerDTO>();

            string SP_GetAllSellers = "SP_GetAllSellers";

            SqlCommand command = new SqlCommand(SP_GetAllSellers, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int sellerID = (int)reader["sellerID"];
                        int personID = (int)reader["personID"];
                        string storeName = (string)reader["storeName"];
                        string? logoStore = reader.IsDBNull(reader.GetOrdinal("logoStore")) ? null :
                                            (string)reader["logoStore"];

                        listSeller.Add(new clsSellerDTO(sellerID, personID, storeName, logoStore));

                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllSellers Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return listSeller;
        }

        public static int AddNewSeller(clsSellerDTO sellerDTO)
        {
            int InsertedID = 0;
            string SP_AddNewSeller = "SP_AddNewSeller";
            SqlCommand command = new SqlCommand(SP_AddNewSeller, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@personID", sellerDTO.personID);
            command.Parameters.AddWithValue("@storeName", sellerDTO.storeName);
            command.Parameters.AddWithValue("@logoStore", sellerDTO.logoStore ?? (object)DBNull.Value);

            var outputIdParam = new SqlParameter("@NewSeller_ID", SqlDbType.Int)
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
                System.Diagnostics.Debug.WriteLine("Error: AddNewSeller Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return InsertedID;

        }

        public static bool UpdateSeller(clsSellerDTO sellerDTO)
        {
            bool isUpdated = false;

            string SP_UpdateSeller = "SP_UpdateSeller";

            SqlCommand command = new SqlCommand(SP_UpdateSeller, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@sellerID", sellerDTO.sellerID);
            //command.Parameters.AddWithValue("@personID", sellerDTO.personID);
            command.Parameters.AddWithValue("@storeName", sellerDTO.storeName);
            command.Parameters.AddWithValue("@logoStore", sellerDTO.logoStore ?? (object)DBNull.Value);


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
                System.Diagnostics.Debug.WriteLine("Error: UpdateSeller Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return isUpdated;
        }

        public static bool DeleteSeller(int sellerID)
        {
            bool isDeleted = false;

            string SP_DeleteSeller = "SP_DeleteSeller";

            SqlCommand command = new SqlCommand(SP_DeleteSeller, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@sellerID", sellerID);

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
                Console.WriteLine("Error : DeleteSeller Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return isDeleted;
        }

        public static clsSellerDTO GetSellerByID(int sellerID)
        {
            clsSellerDTO sellerDTO = new clsSellerDTO();

            string SP_GetSellerByID = "SP_GetSellerByID";

            SqlCommand command = new SqlCommand(SP_GetSellerByID, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@sellerID", sellerID);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    sellerID = (int)reader["sellerID"];
                    int personID = (int)reader["personID"];
                    string storeName = (string)reader["storeName"];
                    string? logoStore = reader.IsDBNull(reader.GetOrdinal("logoStore")) ? null :
                                        (string)reader["logoStore"];

                    sellerDTO = new clsSellerDTO(sellerID, personID, storeName, logoStore);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetSellerBysellerID Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return sellerDTO;
        }

        public static bool SellerSignUp(clsSignUPSellerDTO sellerDTO)
        {
            bool isSignedIN = false;

            string SP_SellerSignUp = "SP_SellerSignUp";

            SqlCommand command = new SqlCommand(SP_SellerSignUp, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            //person data
            command.Parameters.AddWithValue("@fullName", sellerDTO.fullName);
            command.Parameters.AddWithValue("@email", sellerDTO.email);
            command.Parameters.AddWithValue("@passwordHash", sellerDTO.password);
            command.Parameters.AddWithValue("@phone", sellerDTO.phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@addressPerson", sellerDTO.addressPerson ?? (object)DBNull.Value);

            //seller data
            command.Parameters.AddWithValue("@storeName", sellerDTO.storeName);
            command.Parameters.AddWithValue("@logoStore", sellerDTO.logoStore ?? (object)DBNull.Value);

            SqlParameter returnParameter = new SqlParameter
            {
                ParameterName = "@ReturnValue",
                Direction = ParameterDirection.ReturnValue
            };

            command.Parameters.Add(returnParameter);

            try
            {
                connectionToDB.Open();

                command.ExecuteNonQuery();

                isSignedIN = Convert.ToInt32(returnParameter.Value) == 1;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: SellerSignUp Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return isSignedIN;

        }

    }
}
