using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSI_List_ReportProcessing
{
    class ITL
    {
        public static void LoadData()
        {

            ClientContext ctxCatalog = QueryAssistants.getProjectSpCtx(new Uri("https://kineticsys.sharepoint.com/sites/projects"));
            List oList = ctxCatalog.Web.Lists.GetByTitle("KPPS Projects Catalog");
            CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Active</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };
            //CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title' /><Value Type='Text'>622000336</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };

            ListItemCollection sites = oList.GetItems(camlQuery);
            ctxCatalog.Load(sites);
            ctxCatalog.ExecuteQuery();

            foreach (ListItem site in sites)
            {
                var hyperLink = (FieldUrlValue)site["Proj_x0020_Site_x0020_URL"];
                string projSiteURL = hyperLink.Url.Replace("/SitePages/Home.aspx", "");
                var projCatalogNum = site["Title"].ToString();
                var prjCatalogName = site["Project_x0020_Name"].ToString();
                Console.WriteLine(hyperLink.Url + "\r\n");


                ClientContext ctxProjSite = QueryAssistants.getProjectSpCtx(new Uri(projSiteURL));
                List pList = ctxProjSite.Web.Lists.GetByTitle("ITL");
                CamlQuery pCamlQuery = new CamlQuery() { ViewXml = "<View Scope='RecursiveAll'><RowLimit>5000</RowLimit></View>" };
                /// list.GetItems(CamlQuery.CreateAllItemsQuery());
                ListItemCollection projItems = pList.GetItems(pCamlQuery);
                FieldCollection lfc = pList.Fields;

                ctxProjSite.Load(pList);
                ctxProjSite.Load(projItems);
                ctxProjSite.Load(lfc);
                ctxProjSite.ExecuteQuery();

                foreach (ListItem projItem in projItems)
                {
                    //Console.WriteLine("ID: {0} \nTitle: {1} \nBody: {2}", proj.Id, proj["Title"], proj["P_x0026_ID_x0020_No_x002e_"]);

                    Int32 ID = Convert.ToInt32(projItem["ID"]);
                    string prjNo = projCatalogNum;
                    string prjName = prjCatalogName;


                    ////////Inconsistent Fields/////////////////////////////////////
                    //var PID = string.Empty;
                    //var AreaTaskName = string.Empty;
                    //var VesLoc = string.Empty;
                    //var BatchNo = string.Empty;
                    //var rev = string.Empty;

                    Decimal KSIRFINumber = 0;
                    string RFIStatus = string.Empty;
                    Decimal ResponseTime = 0;
                    int DaysOutstanding = 0;
                    string RFITitle = string.Empty;


                    //RFI_x0020_Number
                    //    RFIStatus
                    //Response_x0020_Time
                    //Days_x0020_Outstanding2
                    //Title






                    if (projItem.FieldValues.ContainsKey("Title")) { RFITitle = projItem["Title"].ToString(); }
                    if (projItem.FieldValues.ContainsKey("RFIStatus") && projItem["Detailing_x0020_Spool_x0020_Stat"] != null) { BIMDetailingSpoolStatus = projItem["Detailing_x0020_Spool_x0020_Stat"].ToString(); }
                    if (projItem.FieldValues.ContainsKey("Response_x0020_Time") && projItem["bcav"] != null) { DateRlstoFAB = DateTime.Parse(projItem["bcav"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("Days_x0020_Outstanding2") && projItem["Date_x0020_FAB_x0020_Complete"] != null) { DateFABComplete = DateTime.Parse(projItem["Date_x0020_FAB_x0020_Complete"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("RFI_x0020_Number") && projItem["Date_x0020_FAB_x0020_Received"] != null) { DateFABReceived = DateTime.Parse(projItem["Date_x0020_FAB_x0020_Received"].ToString()); }
                    
                    int res = LoadSQLData(ID, prjNo, prjName, ISO, BIMDetailingSpoolStatus, DateRlstoFAB, DateFABComplete, DateFABReceived, DateSUPPORTSInst, DateInRackErected, DateFieldWelded, DateSlopeCheckQC, DateTested, DateMCWalk, PlanQtySUPPORTS, FIELDActualQtySUPPs, PlanFt, FIELDActualLF, PlanSWs, FABShopACTUALWelds, PlanFWs, AddedFWs, TotalFWs, FIELDActWeldsCompl, TotalSWsFWs);

                    if (res > 0)
                    {
                        Console.Write(prjNo + " Data Inserted Successfully" + "\r\n");
                    }
                    else
                    {
                        Console.Write("Data Not Inserted"  + "\r\n");
                    }
                }
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
