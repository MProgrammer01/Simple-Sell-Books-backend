using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleSellBooks_DataLayer.conditions
{
    public class clsConditionData
    {
        static SqlConnection connectionToDB = new SqlConnection(clsConnectToDB.ConnectionToDB);

        public static IEnumerable<clsConditionDTO> GetAllConditions()
        {
            List<clsConditionDTO> listConditions = new List<clsConditionDTO>();

            string SP_GetAllConditions = "SP_GetAllConditions";

            SqlCommand command = new SqlCommand(SP_GetAllConditions, connectionToDB);

            command.CommandType = CommandType.StoredProcedure;

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int conditionID = (int)reader["conditionID"];
                        string conditionName = (string)reader["conditionName"];

                        listConditions.Add(new clsConditionDTO(conditionID, conditionName));

                    }
                }

                reader.Close();

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error: GetAllConditions Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return listConditions;
        }


        public static clsConditionDTO GetConditionByID(int conditionID)
        {
            clsConditionDTO conditionDTO = new clsConditionDTO();

            string SP_GetConditionByID = "SP_GetConditionByID";

            SqlCommand command = new SqlCommand(SP_GetConditionByID, connectionToDB);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@conditionID", conditionID);

            try
            {
                connectionToDB.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    conditionID = (int)reader["conditionID"];
                    string conditionName = (string)reader["conditionName"];

                    conditionDTO = new clsConditionDTO(conditionID, conditionName);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error GetConditionByID Data " + ex.Message);
            }
            finally
            {
                connectionToDB.Close();
            }

            return conditionDTO;
        }
    }
}
