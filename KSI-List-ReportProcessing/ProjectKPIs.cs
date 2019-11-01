using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using System.IO;

namespace KSI_List_ReportProcessing
{
    class ProjectKPI
    {

        public static void LoadData()
        {
            

            string targetPath = @"\\eho-erp-ln2\Transfer\Tisoware\151\ProjectKPI.csv";

            readdt(targetPath);


            //GetCSVData2(targetPath);

            //var dt =  new DataTable = GetCSVData(targetPath);

        }


        public static void readdt(string target)
        {
            var dt = new DataTable();
            dt = GetCSVData(target);

            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    Console.WriteLine(row[column]);
                }
            }
        }



        private static void GetCSVData2(string csv_file_path)
        {
            using (var reader = new StreamReader(csv_file_path))
            using (var csv = new CsvReader(reader))
            {
                csv.Configuration.PrepareHeaderForMatch = (string header, int index) => header.ToLower();
                //var records = csv.GetRecords(T);
            }
        }



      private static DataTable GetCSVData(string csv_file_path)
        {
            var dt = new DataTable();
            using (var reader = new StreamReader(csv_file_path))
            using (var csv = new CsvReader(reader))
            {
                using (var dr = new CsvDataReader(csv))
                {
                    dt.Columns.Add("Project", typeof(string));
                    dt.Columns.Add("Description", typeof(string));
                    dt.Load(dr);
                }
                return dt;
            }
        }





        public static int LoadSQLData(int ID, string prjNo, string prjName, string ISO, string BIMDetailingSpoolStatus, DateTime? DateRlstoFAB, DateTime? DateFABComplete, DateTime? DateFABReceived, DateTime? DateSUPPORTSInst, DateTime? DateInRackErected, DateTime? DateFieldWelded, DateTime? DateSlopeCheckQC, DateTime? DateTested, DateTime? DateMCWalk, decimal PlanQtySUPPORTS, decimal FIELDActualQtySUPPs, decimal PlanFt, decimal FIELDActualLF, decimal PlanSWs, decimal FABShopACTUALWelds, decimal PlanFWs, decimal AddedFWs, decimal TotalFWs, decimal FIELDActWeldsCompl, decimal TotalSWsFWs)
        {
            DBContextWrapper dbcontext = new DBContextWrapper();
            DBContextWrapper trackingDBContext = new DBContextWrapper(ConfigurationManager.AppSettings["DBConnection"]);
            int success = 0;
            try
            {
                success = dbcontext.InsertData(ID, prjNo, prjName, ISO, BIMDetailingSpoolStatus, DateRlstoFAB, DateFABComplete, DateFABReceived, DateSUPPORTSInst, DateInRackErected, DateFieldWelded, DateSlopeCheckQC, DateTested, DateMCWalk, PlanQtySUPPORTS, FIELDActualQtySUPPs, PlanFt, FIELDActualLF, PlanSWs, FABShopACTUALWelds, PlanFWs, AddedFWs, TotalFWs, FIELDActWeldsCompl, TotalSWsFWs);
                return success;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
