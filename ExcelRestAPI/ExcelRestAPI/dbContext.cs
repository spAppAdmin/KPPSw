using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SharePoint.Client;
using Microsoft.Azure.Management.DataFactories.Runtime;
using Microsoft.Azure.Management.DataFactories.Models;

namespace ExcelRestAPI
{
    class DBContextWrapper
    {
        private SqlConnection connection;


        public DBContextWrapper()
        {
            string sqlconnstr = ConfigurationManager.AppSettings["DBConnection"];
            connection = new SqlConnection(sqlconnstr);
        }

        public DBContextWrapper(string constring)
        {
            string sqlconntr = ConfigurationManager.AppSettings["DBConnection"];
            connection = new SqlConnection(constring);
        }


        public int truncateTable()
        {
            try
            {
                connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "Truncate table [dbo].[ProjectKPIKFactor]";
                int retval = cmd.ExecuteNonQuery();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
                connection.Close();
                return retval;
            }
            catch (Exception)
            {
                connection.Close();
                throw;
            }
        }

        public int InsertData(string projNo, string projName, Decimal oKFactor, Decimal dKFactor, Decimal sKFactor, Decimal fKFactor, Decimal pctCmpte, DateTime dtLstUptd, Decimal currBudMh, Decimal burnedMHs, Decimal remainMHs, Decimal earnedMHs, Decimal fpKFactor, decimal cProjhrs, decimal fProjHrs, string comments, FieldUrlValue site)
        {
             try
             {
            connection.Open();

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "spUpdateKPIFactor";

                cmd.Parameters.Add(new SqlParameter("@ProjectNo", SqlDbType.VarChar)).Value = projNo;
                cmd.Parameters.Add(new SqlParameter("@Project_Name", SqlDbType.VarChar)).Value = projName;
                cmd.Parameters.Add(new SqlParameter("@Overall_KFactor", SqlDbType.Decimal)).Value = oKFactor;
                cmd.Parameters.Add(new SqlParameter("@Detailing_KFactor", SqlDbType.Decimal)).Value = dKFactor;                                                                                                                                                                                                                                                                                                             
                cmd.Parameters.Add(new SqlParameter("@Shop_KFactor", SqlDbType.Decimal)).Value = sKFactor;
                cmd.Parameters.Add(new SqlParameter("@Field_KFactor", SqlDbType.Decimal)).Value = fKFactor;
                cmd.Parameters.Add(new SqlParameter("@pctComplete", SqlDbType.Decimal)).Value = pctCmpte;
                cmd.Parameters.Add(new SqlParameter("@Date_Last_Updated", SqlDbType.DateTime)).Value = dtLstUptd;
                cmd.Parameters.Add(new SqlParameter("@Current_Budget", SqlDbType.Decimal)).Value = currBudMh;
                cmd.Parameters.Add(new SqlParameter("@Burned_MHS", SqlDbType.Decimal)).Value = burnedMHs;
                cmd.Parameters.Add(new SqlParameter("@Remaining_MHS", SqlDbType.Decimal)).Value = remainMHs;
                cmd.Parameters.Add(new SqlParameter("@Earned_MHS", SqlDbType.Decimal)).Value = earnedMHs;
                cmd.Parameters.Add(new SqlParameter("@Field_Projected_KFactor", SqlDbType.Decimal)).Value = fpKFactor;
                cmd.Parameters.Add(new SqlParameter("@Computer_Projected_MHs", SqlDbType.Decimal)).Value = cProjhrs;
                cmd.Parameters.Add(new SqlParameter("@Field_Projected_MHs", SqlDbType.Decimal)).Value = fProjHrs;
                cmd.Parameters.Add(new SqlParameter("@Comments", SqlDbType.VarChar)).Value = comments;
                cmd.Parameters.Add(new SqlParameter("@Site", SqlDbType.NVarChar)).Value = site.Url;


                int retval = cmd.ExecuteNonQuery();
                connection.Close();
                return retval;
        }
          catch (Exception)
          {
                connection.Close();
                throw;
        }
    }
}

}





