using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.statuses
{
    internal class clsStatuseData
    {
        static SqlConnection connectionToDB = new SqlConnection(clsConnectToDB.ConnectionToDB);

        public static IEnumerable<clsStatuseDTO> GetAllStatuses()
        {
            List<clsStatuseDTO> listStatuses = new List<clsStatuseDTO>();

            string SP_GetAllStatuses = "SP_GetAllStatuses";

            SqlCommand command = new SqlCommand(SP_GetAllStatuses, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int statusID = (int)reader["statusID"];
                        string statusName = (string)reader["statusName"];

                        listStatuses.Add(new clsStatuseDTO(statusID, statusName));

                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllStatuses Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return listStatuses;
        }

        public static clsStatuseDTO GetStatuseByID(int statusID)
        {
            clsStatuseDTO statuseDTO = new clsStatuseDTO();

            string SP_GetStatuseByID = "SP_GetStatuseByID";

            SqlCommand command = new SqlCommand(SP_GetStatuseByID, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@statusID", statusID);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    statusID = (int)reader["statusID"];
                    string statusName = (string)reader["statusName"];

                    statuseDTO = new clsStatuseDTO(statusID, statusName);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetStatuseByID Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return statuseDTO;
        }
    }
}
