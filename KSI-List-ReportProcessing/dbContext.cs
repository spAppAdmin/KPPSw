using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.SharePoint.Client;

namespace KSI_List_ReportProcessing
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

        public void truncateTable(string Tablename)
        {
           
            var connStr = ConfigurationManager.AppSettings["DBConnection"];
            SqlConnection con = new SqlConnection(connStr);
            con.Open();

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Truncate Table ITL";
            int res = cmd.ExecuteNonQuery();
            con.Close();

            //return res;
        }
          
        

        public int InsertData(int ID, string prjNo, string prjName, string ISO, string BIMDetailingSpoolStatus, DateTime? DateRlstoFAB, DateTime? DateFABComplete, DateTime? DateFABReceived, DateTime? DateSUPPORTSInst, DateTime? DateInRackErected, DateTime? DateFieldWelded, DateTime? DateSlopeCheckQC, DateTime? DateTested, DateTime? DateMCWalk, decimal PlanQtySUPPORTS, decimal FIELDActualQtySUPPs, decimal PlanFt, decimal FIELDActualLF, decimal PlanSWs, decimal FABShopACTUALWelds, decimal PlanFWs, decimal AddedFWs, decimal TotalFWs, decimal FIELDActWeldsCompl, decimal TotalSWsFWs)
        {


            var connStr = ConfigurationManager.AppSettings["DBConnection"];     
            SqlConnection con = new SqlConnection(connStr);
            con.Open();

            SqlCommand cmdTr = new SqlCommand();
            cmdTr.Connection = con;
            cmdTr.CommandType = CommandType.Text;
            cmdTr.CommandText = "Truncate Table ITL";

            int resx = cmdTr.ExecuteNonQuery();
            //con.Close();


            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateITL";

            try
            {
                //pass the parameter to stored procedure

                cmd.Parameters.Add(new SqlParameter("@LID", SqlDbType.Int)).Value = ID;
                cmd.Parameters.Add(new SqlParameter("@prjNo", SqlDbType.VarChar)).Value = prjNo;
                cmd.Parameters.Add(new SqlParameter("@prjName", SqlDbType.VarChar)).Value = prjName;
                cmd.Parameters.Add(new SqlParameter("@ISO", SqlDbType.VarChar)).Value = ISO;
                //cmd.Parameters.Add(new SqlParameter("@PctComplete", SqlDbType.VarChar)).Value = PctComplete;
                cmd.Parameters.Add(new SqlParameter("@BIMDetailingSpoolStatus", SqlDbType.VarChar)).Value = BIMDetailingSpoolStatus;

                cmd.Parameters.Add(new SqlParameter("@DateRlstoFAB", SqlDbType.DateTime)).Value = DateRlstoFAB;
                cmd.Parameters.Add(new SqlParameter("@DateFABComplete", SqlDbType.DateTime)).Value = DateFABComplete;
                cmd.Parameters.Add(new SqlParameter("@DateFABReceived", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateSUPPORTSInst", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateInRack_Erected", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateFieldWelded", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateSlopeCheck_QC", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateTested", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateMCWalk", SqlDbType.DateTime)).Value = null;

                cmd.Parameters.Add(new SqlParameter("@PlanQtySUPPORTS", SqlDbType.Decimal)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@FIELDActualQtySUPPs", SqlDbType.Decimal)).Value = FIELDActualQtySUPPs;
                cmd.Parameters.Add(new SqlParameter("@PlanFt", SqlDbType.Decimal)).Value = PlanFt;
                cmd.Parameters.Add(new SqlParameter("@FIELDActualLF", SqlDbType.Decimal)).Value = FIELDActualLF;

                cmd.Parameters.Add(new SqlParameter("@PlanSWs", SqlDbType.Decimal)).Value = PlanSWs;
                cmd.Parameters.Add(new SqlParameter("@PlanFWs", SqlDbType.Decimal)).Value = PlanFWs;
                cmd.Parameters.Add(new SqlParameter("@AddedFWs", SqlDbType.Decimal)).Value = AddedFWs;
                cmd.Parameters.Add(new SqlParameter("@TotalFWs", SqlDbType.Decimal)).Value = TotalFWs;
                cmd.Parameters.Add(new SqlParameter("@TotalSWsFWs", SqlDbType.Decimal)).Value = TotalSWsFWs;

                cmd.Parameters.Add(new SqlParameter("@FABShopACTUALWelds", SqlDbType.Decimal)).Value = FABShopACTUALWelds;
                cmd.Parameters.Add(new SqlParameter("@FIELDActWeldsCompl", SqlDbType.Decimal)).Value = FIELDActWeldsCompl;

                int res = cmd.ExecuteNonQuery();
                con.Close();

                return res;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
        }



        public int InsertProjectKSI(int ID, string prjNo, string prjName, string ISO, string BIMDetailingSpoolStatus, DateTime? DateRlstoFAB, DateTime? DateFABComplete, DateTime? DateFABReceived, DateTime? DateSUPPORTSInst, DateTime? DateInRackErected, DateTime? DateFieldWelded, DateTime? DateSlopeCheckQC, DateTime? DateTested, DateTime? DateMCWalk, decimal PlanQtySUPPORTS, decimal FIELDActualQtySUPPs, decimal PlanFt, decimal FIELDActualLF, decimal PlanSWs, decimal FABShopACTUALWelds, decimal PlanFWs, decimal AddedFWs, decimal TotalFWs, decimal FIELDActWeldsCompl, decimal TotalSWsFWs)
        {


            var connStr = ConfigurationManager.AppSettings["DBConnection"];
            SqlConnection con = new SqlConnection(connStr);
            con.Open();

            SqlCommand cmdT = new SqlCommand();
            cmdT.Connection = con;
            cmdT.CommandType = CommandType.Text;
            cmdT.CommandText = "Truncate Table ITL";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "spUpdateITL";

            try
            {
                //pass the parameter to stored procedure

                cmd.Parameters.Add(new SqlParameter("@LID", SqlDbType.Int)).Value = ID;
                cmd.Parameters.Add(new SqlParameter("@prjNo", SqlDbType.VarChar)).Value = prjNo;
                cmd.Parameters.Add(new SqlParameter("@prjName", SqlDbType.VarChar)).Value = prjName;
                cmd.Parameters.Add(new SqlParameter("@ISO", SqlDbType.VarChar)).Value = ISO;
                //cmd.Parameters.Add(new SqlParameter("@PctComplete", SqlDbType.VarChar)).Value = PctComplete;
                cmd.Parameters.Add(new SqlParameter("@BIMDetailingSpoolStatus", SqlDbType.VarChar)).Value = BIMDetailingSpoolStatus;

                cmd.Parameters.Add(new SqlParameter("@DateRlstoFAB", SqlDbType.DateTime)).Value = DateRlstoFAB;
                cmd.Parameters.Add(new SqlParameter("@DateFABComplete", SqlDbType.DateTime)).Value = DateFABComplete;
                cmd.Parameters.Add(new SqlParameter("@DateFABReceived", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateSUPPORTSInst", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateInRack_Erected", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateFieldWelded", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateSlopeCheck_QC", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateTested", SqlDbType.DateTime)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@DateMCWalk", SqlDbType.DateTime)).Value = null;

                cmd.Parameters.Add(new SqlParameter("@PlanQtySUPPORTS", SqlDbType.Decimal)).Value = null;
                cmd.Parameters.Add(new SqlParameter("@FIELDActualQtySUPPs", SqlDbType.Decimal)).Value = FIELDActualQtySUPPs;
                cmd.Parameters.Add(new SqlParameter("@PlanFt", SqlDbType.Decimal)).Value = PlanFt;
                cmd.Parameters.Add(new SqlParameter("@FIELDActualLF", SqlDbType.Decimal)).Value = FIELDActualLF;

                cmd.Parameters.Add(new SqlParameter("@PlanSWs", SqlDbType.Decimal)).Value = PlanSWs;
                cmd.Parameters.Add(new SqlParameter("@PlanFWs", SqlDbType.Decimal)).Value = PlanFWs;
                cmd.Parameters.Add(new SqlParameter("@AddedFWs", SqlDbType.Decimal)).Value = AddedFWs;
                cmd.Parameters.Add(new SqlParameter("@TotalFWs", SqlDbType.Decimal)).Value = TotalFWs;
                cmd.Parameters.Add(new SqlParameter("@TotalSWsFWs", SqlDbType.Decimal)).Value = TotalSWsFWs;

                cmd.Parameters.Add(new SqlParameter("@FABShopACTUALWelds", SqlDbType.Decimal)).Value = FABShopACTUALWelds;
                cmd.Parameters.Add(new SqlParameter("@FIELDActWeldsCompl", SqlDbType.Decimal)).Value = FIELDActWeldsCompl;





                int res = cmd.ExecuteNonQuery();
                con.Close();

                return res;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                con.Close();
                con.Dispose();

            }
        }

    }
}





