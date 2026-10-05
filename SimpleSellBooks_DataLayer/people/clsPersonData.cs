using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    public class clsPersonData
    {
        static SqlConnection connectionToDB = new SqlConnection(clsConnectToDB.ConnectionToDB);

        public static IEnumerable<clsPersonDTO> GetAllPeople()
        {
            List<clsPersonDTO> listPerson = new List<clsPersonDTO>();

            string SP_GetAllPeople = "SP_GetAllPeople";

            SqlCommand command = new SqlCommand(SP_GetAllPeople, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int personID = (int)reader["personID"];
                        string fullName = (string)reader["fullName"];
                        string email = (string)reader["email"];
                        string? phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null :
                                            (string)reader["phone"];
                        string? addressPerson = reader.IsDBNull(reader.GetOrdinal("addressPerson")) ? null :
                                            (string)reader["addressPerson"];
                        string role = (string)reader["role"];
                        DateTime createdAt = (DateTime)reader["createdAt"];
                        DateTime updatedAt = (DateTime)reader["updatedAt"];

                        listPerson.Add(new clsPersonDTO(personID, fullName, email, 
                            phone, addressPerson, role, createdAt, updatedAt));

                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllPeople Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return listPerson;
        }

        public static int AddNewPerson(clsSignUpDTO personDTO)
        {
            int InsertedID = 0;

            string SP_AddNewPerson = "SP_AddNewPerson";

            SqlCommand command = new SqlCommand(SP_AddNewPerson, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@fullName", personDTO.fullName);
            command.Parameters.AddWithValue("@email", personDTO.email);
            command.Parameters.AddWithValue("@passwordHash", personDTO.password);
            command.Parameters.AddWithValue("@phone", personDTO.phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@addressPerson", personDTO.addressPerson ?? (object)DBNull.Value);
            //command.Parameters.AddWithValue("@role", personDTO.role ?? (object)DBNull.Value);

            var outputIdParam = new SqlParameter("@NewPerson_ID", SqlDbType.Int)
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
                System.Diagnostics.Debug.WriteLine("Error: AddNewPerson Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return InsertedID;

        }

        public static bool UpdatePerson(clsUpdatePersonDTO personDTO)
        {
            bool isUpdated = false;

            string SP_UpdatePerson = "SP_UpdatePerson";

            SqlCommand command = new SqlCommand(SP_UpdatePerson, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@personID", personDTO.personID);
            command.Parameters.AddWithValue("@fullName", personDTO.fullName);
            command.Parameters.AddWithValue("@email", personDTO.email);
            command.Parameters.AddWithValue("@phone", personDTO.phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@addressPerson", personDTO.addressPerson ?? (object)DBNull.Value);
            //command.Parameters.AddWithValue("@role", personDTO.role ?? (object)DBNull.Value);


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
                System.Diagnostics.Debug.WriteLine("Error: UpdatePerson Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return isUpdated;
        }

        public static bool DeletePerson(int personID)
        {
            bool isDeleted = false;

            string SP_DeletePerson = "SP_DeletePerson";

            SqlCommand command = new SqlCommand(SP_DeletePerson, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@personID", personID);

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
                Console.WriteLine("Error : DeletePerson Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return isDeleted;
        }

        public static clsPersonDTO GetPersonByID(int personID)
        {
            clsPersonDTO personDTO = new clsPersonDTO();

            string SP_GetPersonByID = "SP_GetPersonByID";

            SqlCommand command = new SqlCommand(SP_GetPersonByID, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    personID = (int)reader["personID"];
                    string fullName = (string)reader["fullName"];
                    string email = (string)reader["email"];
                    string? phone = reader.IsDBNull(reader.GetOrdinal("phone")) ? null :
                                        (string)reader["phone"];
                    string? addressPerson = reader.IsDBNull(reader.GetOrdinal("addressPerson")) ? null :
                                        (string)reader["addressPerson"];
                    string role = (string)reader["role"];
                    DateTime createdAt = (DateTime)reader["createdAt"];
                    DateTime updatedAt = (DateTime)reader["updatedAt"];


                    personDTO = new clsPersonDTO(personID, fullName, email,
                        phone, addressPerson, role, createdAt, updatedAt);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetPersonByID Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return personDTO;
        }

        public static clsSignInResponseDTO SignInResponse(string email)
        {
            clsSignInResponseDTO personDTO = new clsSignInResponseDTO();

            string SP_SignInResponse = "SP_SignInResponse";

            SqlCommand command = new SqlCommand(SP_SignInResponse, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@email", email);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int personID = (int)reader["personID"];
                    email = (string)reader["email"];
                    string passwordHash = (string)reader["passwordHash"];
                    string role = (string)reader["role"];
                    personDTO = new clsSignInResponseDTO(personID, email, role, passwordHash);
                    
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error SignInResponse Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return personDTO;
        }


        public static clsSignInResponseDTO RefreshTokenResponce(string email)
        {
            clsSignInResponseDTO personDTO = new clsSignInResponseDTO();

            string SP_RefreshTokenResponce = "SP_RefreshTokenResponce";

            SqlCommand command = new SqlCommand(SP_RefreshTokenResponce, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@email", email);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    int personID = (int)reader["personID"];

                    email = (string)reader["email"];

                    string role = (string)reader["role"];

                    string refreshTokenHash = (string)reader["RefreshTokenHash"];

                    DateTime? refreshTokenExpirationDate = reader.IsDBNull(reader.GetOrdinal("RefreshTokenExpiresAt")) ? null :
                                            (DateTime)reader["RefreshTokenExpiresAt"];

                    DateTime? refreshTokenRevokedAt = reader.IsDBNull(reader.GetOrdinal("RefreshTokenRevokedAt")) ? null :
                                            (DateTime)reader["RefreshTokenRevokedAt"];
                    
                    personDTO = new clsSignInResponseDTO(personID, email, role, refreshTokenHash,
                        refreshTokenExpirationDate, refreshTokenRevokedAt);

                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetPersonByEmail Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return personDTO;
        }


        public static bool UpdateRefreshToken(clsUpdateRefreshTokenDto refreshToken)
        {
            bool refreshTokenIsUpdated = false;

            string SP_UpdateRefreshToken = "SP_UpdateRefreshToken";

            SqlCommand command = new SqlCommand(SP_UpdateRefreshToken, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@personID", refreshToken.personId);
            command.Parameters.AddWithValue("@RefreshTokenHash", refreshToken.RefreshTokenHash);
            command.Parameters.AddWithValue("@RefreshTokenExpiresAt", refreshToken.RefreshTokenExpirationDate);

            try
            {
                connectionToDB.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    refreshTokenIsUpdated = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error : UpdateRefreshToken Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return refreshTokenIsUpdated;
        }

        public static bool IsPersonExistsByEmail(string email)
        {
            bool isPersonExist = false;

            string SP_GetSellerByID = "SP_ExistingPersonByEmail";

            SqlCommand command = new SqlCommand(SP_GetSellerByID, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@email", email);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isPersonExist = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error IsPersonExistsByEmail Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return isPersonExist;
        }

        public static string getPasswordHashByPersonID(int personID)
        {
            string passwordHash = string.Empty;
            string SP_GetPasswordHashByPersonID = "SP_GetPasswordByPersonID";
            SqlCommand command = new SqlCommand(SP_GetPasswordHashByPersonID, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@personID", personID);
            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    passwordHash = (string)reader["passwordHash"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error getPasswordHashByPersonID Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return passwordHash;
        }

        public static bool UpdatePasswordHash(int personID, 
            string newPasswordHash)
        {
            bool isUpdated = false;
            string SP_UpdatePasswordHash = "SP_UpdatePasswordByPersonID";
            SqlCommand command = new SqlCommand(SP_UpdatePasswordHash, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@personID", personID);
            command.Parameters.AddWithValue("@newPasswordHash", newPasswordHash);
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
                System.Diagnostics.Debug.WriteLine("Error: UpdatePasswordHash Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }
            return isUpdated;
        }
    }
}
