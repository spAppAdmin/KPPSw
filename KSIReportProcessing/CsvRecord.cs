using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;



namespace ImportListFromCSV
{




    public class ITLRecord
    {

        public string Title { get; set; }
   
        public string Priority { get; set; }
        public string Status { get; set; }
        public string Cc { get; set; }
        public string c7z3 { get; set; }
        public string P_x0026_ID_x0020_No_x002e_ { get; set; }
        public string Supervisor { get; set; }
        public string Detailing_x0020_Spool_x0020_Stat { get; set; }
        public string _x0076_h85 { get; set; }
        public string FAB_x002d_SHOP { get; set; }
        public string qfrm { get; set; }
        public string FAB_x002d_Kit_x002f_TO_x0020__x0 { get; set; }
        public string FAB_x002d_Cut_x002f_Tack_x0020__ { get; set; }
        public string FAB_x002d_Weld_x0020__x0028_30_x { get; set; }
        public string Fab_x002d_QC_x0020__x0028_10_x00 { get; set; }
        public string FAB_x002d_Comments_x002f_Mat_x00 { get; set; }
        public string Comments { get; set; }
        public string uz0w { get; set; }
        public string p37l { get; set; }
        public string yvrw { get; set; }
        public string Batch_x0020_No { get; set; }
        public string Project_x0020_Manager { get; set; }
        public string __________________x0020_Kinetics { get; set; }
        public string ISO_x0020_Printed_x0020__x0028_Y { get; set; }
        public string Vessel_or_Module { get; set; }
        public string DET_x0020_to_x0020_Shop_x0020_Tr { get; set; }
        public string Package_x0020_No_x002e_ { get; set; }
        public string Zone { get; set; }
        public string CCOMPANY { get; set; }
        public string IWSAPHistory { get; set; }
        public string VF_x0020_Text { get; set; }

    }

    public sealed class ProjectITLMap : ClassMap<ITLRecord>
    {
        public ProjectITLMap()
        {
            Map(m => m.Title).Name("______________________ISO Number______________________");
            Map(m => m.Priority).Name("Priority");
            Map(m => m.Status).Name("Task Status");
            Map(m => m.Cc).Name("Cc");
            Map(m => m.c7z3).Name("Cell_or_Module");
            Map(m => m.P_x0026_ID_x0020_No_x002e_).Name("P&ID No.");
            Map(m => m.Supervisor).Name("Supervisor");
            Map(m => m.Detailing_x0020_Spool_x0020_Stat).Name("BIM-Detailing Spool Status");
            Map(m => m._x0076_h85).Name("BIM-Detailer");
            Map(m => m.FAB_x002d_SHOP).Name("FAB-SHOP");
            Map(m => m.qfrm).Name("FAB-Shop Foreman");
            Map(m => m.FAB_x002d_Kit_x002f_TO_x0020__x0).Name("FAB-Kit/TO (10%)");
            Map(m => m.FAB_x002d_Cut_x002f_Tack_x0020__).Name("FAB-Cut/Tack (50%)");
            Map(m => m.FAB_x002d_Weld_x0020__x0028_30_x).Name("FAB-Weld (30%)");
            Map(m => m.Fab_x002d_QC_x0020__x0028_10_x00).Name("FAB-QC (10%)");
            Map(m => m.FAB_x002d_Comments_x002f_Mat_x00).Name("FAB-Comments/Mat.Shorts?");
            Map(m => m.Comments).Name("Comments");
            Map(m => m.uz0w).Name("Change Order");
            Map(m => m.p37l).Name("Rev");
            Map(m => m.yvrw).Name("FAB-Transmittal No.");
            Map(m => m.Batch_x0020_No).Name("Floor_or_Location");
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m.__________________x0020_Kinetics).Name("____________________Kinetics PDF Drawing____________________");
            Map(m => m.ISO_x0020_Printed_x0020__x0028_Y).Name("ISO Printed (Y/N)");
            Map(m => m.Vessel_or_Module).Name("Vessel");
            Map(m => m.DET_x0020_to_x0020_Shop_x0020_Tr).Name("DET to Shop Trans No.");
            Map(m => m.Package_x0020_No_x002e_).Name("Package No.");
            Map(m => m.Zone).Name("Zone");
            Map(m => m.CCOMPANY).Name("CCOMPANY");
            Map(m => m.IWSAPHistory).Name("IWSAPHistory");
            Map(m => m.VF_x0020_Text).Name("VF Text");


        }
    }

    public class CsvRecord
    {
        public string Project { get; set; }
        public string NamesColumnData { get; set; }
    public string Description { get; set; }
        public string Status { get; set; }
        public int? Original_x0020_Contract { get; set; }
        public int? BIX_x0020__x002f__x0020_CIX { get; set; }
        public int? Total_x0020_Scope_x0020_Changes { get; set; }
        public int? Contract_x0020_Growth_x0020__x00 { get; set; }
        public int? Proceeding_x0020_Scope_x0020_Cha { get; set; }
        public int? _x0025__x0020_of_x0020_COs_x0020 { get; set; }
        public int? Total_x0020_Contract_x0020_Amoun { get; set; }
        public string Project_x0020_Manager { get; set; }
        public string Operations_x0020_Manager { get; set; }
        public string Client { get; set; }
        public int? Total_x0020_Billings { get; set; }
        public int? Open_x0020_Commitments_x0020_ { get; set; }
        public int? JTD_x0020_Cost_x0020_ { get; set; }
        //public int? Paid_x0020_to_x0020_Supp_x002e__ { get; set; }
        //public int? Received_x0020_from_x0020_Cust_x { get; set; }
        public int? JTD_x0020__x0025__x0020_Complete { get; set; }
        public int? JTD_x002b_OpenCmt_x0020__x0025__ { get; set; }
        public int? Cash_x0020_Position { get; set; }
        public int? Collections_x0020_past_x0020_due { get; set; }
        //public DateTime L_x002e_Invoice_x0020_to_x0020_C { get; set; }
        public int? PM_x0020_Fcst_x0020_Cost { get; set; }
        //public DateTime Start_x0020_Time { get; set; }
        //public DateTime FiniTime { get; set; }
        public int? Original_x0020_Margin { get; set; }
        public int? PM_x0027_s_x0020_Margin { get; set; }
        public int? Budget_x0020_LABOR_x0020_Cost { get; set; }
        public int? ACTUAL_x0020_LABOR_x0020_Cost { get; set; }
        public int? LABOR_x0020__x0025__x0020_Spent { get; set; }
        public int? Budget_x0020_MAT_x0026_EQMNT_x00 { get; set; }
        public int? ACTUAL_x0020_MAT_x0026_EQMNT_x00 { get; set; }
        public int? MAT_x0020__x0025__x0020_Spent { get; set; }
        public int? Budget_x0020_GCs_x0020_Cost { get; set; }
        public int? ACTUAL_x0020_GCs_x0020_Cost { get; set; }
        public int? GC_x0027_s_x0020__x0025__x0020_S { get; set; }
        public int? Budget_x0020_SUBs_x0020_Cost { get; set; }
        public int? ACTUAL_x0020_SUBs_x0020_Cost { get; set; }
        public int? SUBs_x0020__x0025__x0020_Spent { get; set; }
        public string Category { get; set; }
        public int? Collections_x0020_due { get; set; }
        public int? Original_x0020_BUDGET_x0020_Cost { get; set; }
        public int? Total_x0020_BUDGET_x0020_Cost { get; set; }

    }

    public sealed class ProjectKPIMap : ClassMap<CsvRecord>
    {
        public ProjectKPIMap()
        {

            Map(m => m.Project).Name("Project");
            Map(m => m.Description).Name("Description");
            Map(m => m.Status).Name("Status");
            Map(m => m.Original_x0020_Contract).Name("Original Contract");
            Map(m => m.BIX_x0020__x002f__x0020_CIX).Name("BIX / CIX");
            Map(m => m.Total_x0020_Scope_x0020_Changes).Name("Total Scope Changes");
            Map(m => m.Contract_x0020_Growth_x0020__x00).Name("Contract Growth (%)");
            Map(m => m.Proceeding_x0020_Scope_x0020_Cha).Name("Proceeding Scope Changes");
            Map(m => m._x0025__x0020_of_x0020_COs_x0020).Name("% of COs in Proceeding");
            Map(m => m.Total_x0020_Contract_x0020_Amoun).Name("Total Contract Amount");
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m.Operations_x0020_Manager).Name("Operations Manager");
            Map(m => m.Client).Name("Client");
            Map(m => m.Total_x0020_Billings).Name("Total Billings");
            Map(m => m.Open_x0020_Commitments_x0020_).Name("Open Commitments");
            Map(m => m.JTD_x0020_Cost_x0020_).Name("JTD Cost");
            //Map(m => m.Paid_x0020_to_x0020_Supp_x002e__).Name("Paid to Supp");
           // Map(m => m.Received_x0020_from_x0020_Cust_x).Name("Received from Cust");
            Map(m => m.JTD_x0020__x0025__x0020_Complete).Name("JTD % Complete");
            Map(m => m.JTD_x002b_OpenCmt_x0020__x0025__).Name("JTD+OpenCmt % Complete");
            Map(m => m.Cash_x0020_Position).Name("Cash Position");
            Map(m => m.Collections_x0020_past_x0020_due).Name("Collections past due");
            //Map(m => m.L_x002e_Invoice_x0020_to_x0020_C).Name("L.Invoice to Cust").TypeConverterOption.Format("dd-MM-yyyy");
            Map(m => m.PM_x0020_Fcst_x0020_Cost).Name("PM Fcst Cost");
            //Map(m => m.Start_x0020_Time).Name("Start Time").TypeConverterOption.Format("dd-MM-yyyy");
            //Map(m => m.FiniTime).Name("FiniTime").TypeConverterOption.Format("dd-MM-yyyy");
            Map(m => m.Original_x0020_Margin).Name("Original Margin");
            Map(m => m.PM_x0027_s_x0020_Margin).Name("PMs Margin");
            Map(m => m.Budget_x0020_LABOR_x0020_Cost).Name("Budget LABOR Cost");
            Map(m => m.ACTUAL_x0020_LABOR_x0020_Cost).Name("ACTUAL LABOR Cost");
            Map(m => m.LABOR_x0020__x0025__x0020_Spent).Name("LABOR % Spent");
            Map(m => m.Budget_x0020_MAT_x0026_EQMNT_x00).Name("Budget MAT&EQMNT Cost");
            Map(m => m.ACTUAL_x0020_MAT_x0026_EQMNT_x00).Name("ACTUAL MAT&EQMNT Cost");
            Map(m => m.MAT_x0020__x0025__x0020_Spent).Name("MAT % Spent");
            Map(m => m.Budget_x0020_GCs_x0020_Cost).Name("Budget GCs Cost");
            Map(m => m.ACTUAL_x0020_GCs_x0020_Cost).Name("ACTUAL GCs Cost");
            Map(m => m.GC_x0027_s_x0020__x0025__x0020_S).Name("GCs % Spent");
            Map(m => m.Budget_x0020_SUBs_x0020_Cost).Name("Budget SUBs Cost");
            Map(m => m.ACTUAL_x0020_SUBs_x0020_Cost).Name("ACTUAL SUBs Cost");
            Map(m => m.SUBs_x0020__x0025__x0020_Spent).Name("SUBs % Spent");
            Map(m => m.Category).Name("Category");
            Map(m => m.Collections_x0020_due).Name("Collections due");
            Map(m => m.Original_x0020_BUDGET_x0020_Cost).Name("Original BUDGET Cost");
            Map(m => m.Total_x0020_BUDGET_x0020_Cost).Name("Total BUDGET Cost");

        }
    }
}