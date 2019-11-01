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
            

            CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='K_x002d_FactorInclude'/><Value Type='Boolean'>1</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title'/><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };
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
                    var PID = string.Empty;
                    //var AreaTaskName = string.Empty;
                    //var VesLoc = string.Empty;
                    //var BatchNo = string.Empty;
                    //var rev = string.Empty;

                    string ISO = string.Empty;
                    string BIMDetailingSpoolStatus = string.Empty;

                    //string DateRlstoFAB = string.Empty;
                    //string DateFABComplete = string.Empty;
                    //string DateFABReceived = string.Empty;
                    //string DateSUPPORTSInst = string.Empty;
                    //string DateInRackErected = string.Empty;
                    //string DateFieldWelded = string.Empty;
                    //string DateSlopeCheckQC = string.Empty;
                    //string DateTested = string.Empty;
                    //string DateMCWalk = string.Empty;

                    DateTime? DateRlstoFAB = null;
                    DateTime? DateFABComplete = null;
                    DateTime? DateFABReceived = null;
                    DateTime? DateSUPPORTSInst = null;
                    DateTime? DateInRackErected = null;
                    DateTime? DateFieldWelded = null;
                    DateTime? DateSlopeCheckQC = null;
                    DateTime? DateTested = null;
                    DateTime? DateMCWalk = null;

                    Decimal PercentComplete = 0;
                    Decimal PlanQtySUPPORTS = 0;
                    Decimal FIELDActualQtySUPPs = 0;
                    Decimal PlanFt = 0;
                    Decimal FIELDActualLF = 0;
                    Decimal FIELDActWeldsCompl = 0;
                    Decimal FABShopACTUALWelds = 0;
                    Decimal PlanSWs = 0;
                    Decimal PlanFWs = 0;
                    Decimal AddedFWs = 0;
                    Decimal AddedSWs = 0;
                    Decimal TotalFWs = 0;
                    Decimal TotalSWsFWs = 0;

                    //DateTime dt;
                    //if (DateTime.TryParse(projItem["bcav"].ToString(), out dt))   
                    //{
                    //DateRlstoFAB = dt;   
                    //}
                    //else
                    //{
                    //DateRlstoFAB = null;
                    //}


                    if (projItem.FieldValues.ContainsKey("Title")) { ISO = projItem["Title"].ToString(); }
                    if (projItem.FieldValues.ContainsKey("Detailing_x0020_Spool_x0020_Stat") && projItem["Detailing_x0020_Spool_x0020_Stat"] != null) { BIMDetailingSpoolStatus = projItem["Detailing_x0020_Spool_x0020_Stat"].ToString(); }
                    if (projItem.FieldValues.ContainsKey("bcav") && projItem["bcav"] != null) { DateRlstoFAB = DateTime.Parse(projItem["bcav"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("Date_x0020_FAB_x0020_Complete") && projItem["Date_x0020_FAB_x0020_Complete"] != null) { DateFABComplete = DateTime.Parse(projItem["Date_x0020_FAB_x0020_Complete"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("Date_x0020_FAB_x0020_Received") && projItem["Date_x0020_FAB_x0020_Received"] != null) { DateFABReceived = DateTime.Parse(projItem["Date_x0020_FAB_x0020_Received"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("x24v") && projItem["x24v"] != null) { DateSUPPORTSInst = DateTime.Parse(projItem["x24v"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("oiqs") && projItem["oiqs"] != null) { DateInRackErected = DateTime.Parse(projItem["oiqs"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("i67c") && projItem["i67c"] != null) { DateFieldWelded = DateTime.Parse(projItem["i67c"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("Date_x002d_SlopeCheck_x0028_QC_x") && projItem["Date_x002d_SlopeCheck_x0028_QC_x"] != null) { DateSlopeCheckQC = DateTime.Parse(projItem["Date_x002d_SlopeCheck_x0028_QC_x"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("_x006b_o58") && projItem["_x006b_o58"] != null) { DateTested = DateTime.Parse(projItem["_x006b_o58"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("ah1c") && projItem["ah1c"] != null) { DateMCWalk = DateTime.Parse(projItem["ah1c"].ToString()); }
                    if (projItem.FieldValues.ContainsKey("% Complete")) { PercentComplete = Convert.ToDecimal(projItem["% Complete"]); }
                    if (projItem.FieldValues.ContainsKey("ip0d")) { PlanQtySUPPORTS = Convert.ToDecimal(projItem["ip0d"]); }
                    if (projItem.FieldValues.ContainsKey("xpin")) { FIELDActualQtySUPPs = Convert.ToDecimal(projItem["xpin"]); }
                    if (projItem.FieldValues.ContainsKey("h7r6")) { PlanFt = Convert.ToDecimal(projItem["h7r6"]); }
                    if (projItem.FieldValues.ContainsKey("czjw")) { FIELDActualLF = Convert.ToDecimal(projItem["czjw"]); }
                    if (projItem.FieldValues.ContainsKey("w11d")) { FIELDActWeldsCompl = Convert.ToDecimal(projItem["w11d"]); }
                    if (projItem.FieldValues.ContainsKey("nb0x")) { FABShopACTUALWelds = Convert.ToDecimal(projItem["nb0x"]); }
                    if (projItem.FieldValues.ContainsKey("nigq")) { PlanSWs = Convert.ToDecimal(projItem["nigq"]); }
                    if (projItem.FieldValues.ContainsKey("cuci")) { PlanFWs = Convert.ToDecimal(projItem["cuci"]); }
                    if (projItem.FieldValues.ContainsKey("qh8m")) { AddedFWs = Convert.ToDecimal(projItem["qh8m"]); }
                    if (projItem.FieldValues.ContainsKey("Added_x0020_SWs")) { AddedSWs = Convert.ToDecimal(projItem["Added_x0020_SWs"]); }
                    if (projItem.FieldValues.ContainsKey("Total_x0020_FWs")) { TotalFWs = Convert.ToDecimal(projItem["Total_x0020_FWs"]); }
                    if (projItem.FieldValues.ContainsKey("Total_x0020_SWs_x002b_FWs")) { TotalSWsFWs = Convert.ToDecimal(projItem["Total_x0020_SWs_x002b_FWs"]); }
                    if (projItem.FieldValues.ContainsKey("cuci")) { PlanFWs = Convert.ToDecimal(projItem["cuci"]); }


                   
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
