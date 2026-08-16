using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.people
{
    internal class clsPersonData
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
                        DateTime createdAt = (DateTime)reader["createdAt"];
                        DateTime updatedAt = (DateTime)reader["updatedAt"];

                        listPerson.Add(new clsPersonDTO(personID, fullName, email, 
                            phone, addressPerson, createdAt, updatedAt));

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

        public static int AddNewPerson(clsPersonDTO personDTO)
        {
            int InsertedID = 0;

            string SP_AddNewPerson = "SP_AddNewPerson";

            SqlCommand command = new SqlCommand(SP_AddNewPerson, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@fullName", personDTO.fullName);
            command.Parameters.AddWithValue("@email", personDTO.email);
            command.Parameters.AddWithValue("@passwordHash", personDTO.passwordHash);
            command.Parameters.AddWithValue("@phone", personDTO.phone ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@addressPerson", personDTO.addressPerson ?? (object)DBNull.Value);

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

        public static bool UpdatePerson(clsPersonDTO personDTO)
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
            command.Parameters.AddWithValue("@updatedAt", personDTO.updatedAt);


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
                    DateTime createdAt = (DateTime)reader["createdAt"];
                    DateTime updatedAt = (DateTime)reader["updatedAt"];


                    personDTO = new clsPersonDTO(personID, fullName, email,
                        phone, addressPerson, createdAt, updatedAt);
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
    }
}
