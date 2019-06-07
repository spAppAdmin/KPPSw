using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using sp = Microsoft.SharePoint.Client;



namespace csvUploadWeb
{



    public class ITLRecord
    {
        public string P_x0026_ID_x0020_No_x002e_ { get; set; }
        public string Title { get; set; }
        public string Area_x002d_Task_x002d_Name_x0028 { get; set; }
        public string Batch_x0020_No { get; set; }
        public string p37l { get; set; }
        public string c7z3 { get; set; }
        public string h7r6 { get; set; }
        public string cuci { get; set; }
        public string nigq { get; set; }
        public string awcz { get; set; }
        public DateTime _x0069_kt1 { get; set; }
        public DateTime rz4k { get; set; }
        public DateTime Date_x002d_OwnerMat_x002e_ETA { get; set; }
        public DateTime Date_x0020_FAB_x0020_Complete { get; set; }
        public DateTime Date_x0020_FAB_x0020_Received { get; set; }
        public DateTime x24v { get; set; }
        public DateTime oiqs { get; set; }
        public DateTime i67c { get; set; }
        public DateTime Date_x002d_SlopeCheck_x0028_QC_x { get; set; }
        public DateTime _x006b_o58 { get; set; }
        public DateTime ah1c { get; set; }
        public DateTime qh8m { get; set; }
        public DateTime _x0076_h85 { get; set; }
        public DateTime Detailing_x0020_Spool_x0020_Stat { get; set; }
        public DateTime PercentComplete { get; set; }
        public DateTime Cc { get; set; }
        public DateTime Comments { get; set; }
        public DateTime uz0w { get; set; }
        public DateTime _x0061_y74 { get; set; }
        public DateTime _x0073_rp1 { get; set; }
        public DateTime _x0070_j39 { get; set; }
        public DateTime h35h { get; set; }
        public DateTime avnf { get; set; }
        public DateTime ydxs { get; set; }
        public DateTime j2a7 { get; set; }
        public DateTime bcav { get; set; }
        public DateTime Body { get; set; }
        public DateTime FAB_x002d_Comments_x002f_Mat_x00 { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime FAB_x002d_Cut_x002f_Tack_x0020__ { get; set; }
        public DateTime FAB_x002d_Kit_x002f_TO_x0020__x0 { get; set; }
        public string _x0071_gx7 { get; set; }
        public string Fab_x002d_QC_x0020__x0028_10_x00 { get; set; }
        public string FAB_x002d_SHOP { get; set; }
        public string nb0x { get; set; }
        public string qfrm { get; set; }
        public string lhfu { get; set; }
        public string yvrw { get; set; }
        public string FAB_x002d_Weld_x0020__x0028_30_x { get; set; }
        public string w11d { get; set; }
        public string czjw { get; set; }
        public string xpin { get; set; }
        public decimal ip0d { get; set; }
        public decimal Field_x002d_MHs { get; set; }
        public decimal Predecessors { get; set; }
        public decimal Priority { get; set; }
        public decimal Project_x0020_Manager { get; set; }
        public string RelatedItems { get; set; }
        public string Supervisor { get; set; }
        public string Weight_x0020__x0028_lbs_x0029_ { get; set; }
    }

    public sealed class ProjectITLMap : ClassMap<ITLRecord>
    {
        public ProjectITLMap()
        {
            Map(m => m.P_x0026_ID_x0020_No_x002e_).Name("P&ID No.");
            Map(m => m.Title).Name("______________________ISO Number______________________");
            Map(m => m.Area_x002d_Task_x002d_Name_x0028).Name("Area-Task-Name(from C.C. List)");
            Map(m => m.Batch_x0020_No).Name("Batch No");
            Map(m => m.p37l).Name("Rev");
            Map(m => m.c7z3).Name("Vessel_or_Location");
            Map(m => m.h7r6).Name("Plan Ft.");
            Map(m => m.cuci).Name("Plan FWs");
            Map(m => m.nigq).Name("Plan SWs");
            Map(m => m.awcz).Name("Date-IFR Sub");
            Map(m => m._x0069_kt1).Name("Date-Approved");
            Map(m => m.rz4k).Name("Date-IFC");
            Map(m => m.Date_x002d_OwnerMat_x002e_ETA).Name("Date-OwnerMat.ETA");
            Map(m => m.Date_x0020_FAB_x0020_Complete).Name("Date-FAB Complete");
            Map(m => m.Date_x0020_FAB_x0020_Received).Name("Date-FAB Received");
            Map(m => m.x24v).Name("Date-SUPPORTSInst");
            Map(m => m.oiqs).Name("Date-InRack_Erected");
            Map(m => m.i67c).Name("Date-FieldWelded");
            Map(m => m.Date_x002d_SlopeCheck_x0028_QC_x).Name("Date-SlopeCheck_QC");
            Map(m => m._x006b_o58).Name("Date-Tested");
            Map(m => m.ah1c).Name("Date-MCWalk");
            Map(m => m.qh8m).Name("Added FWs");
            Map(m => m._x0076_h85).Name("BIM-Detailer");
            Map(m => m.Detailing_x0020_Spool_x0020_Stat).Name("BIM-Detailing Spool Status");
            Map(m => m.PercentComplete).Name("% Complete");
            Map(m => m.Cc).Name("Cc");
            Map(m => m.Comments).Name("Comments");
            Map(m => m.uz0w).Name("Change Order");
            Map(m => m._x0061_y74).Name("Date-DEMO/or/Eqmnt. Inst.");
            Map(m => m._x0073_rp1).Name("Date-Expected onsite");
            Map(m => m._x0070_j39).Name("Date-FAB Shipped");
            Map(m => m.h35h).Name("Date-Install Needed by");
            Map(m => m.avnf).Name("Date-Insulated");
            Map(m => m.ydxs).Name("Date-ISO Needed");
            Map(m => m.j2a7).Name("Date-Passivated");
            Map(m => m.bcav).Name("Date-Rls to FAB");
            Map(m => m.Body).Name("Description");
            Map(m => m.FAB_x002d_Comments_x002f_Mat_x00).Name("FAB-Comments/Mat.Shorts?");
            Map(m => m.DueDate).Name("Due Date");
            Map(m => m.FAB_x002d_Cut_x002f_Tack_x0020__).Name("FAB-Cut/Tack (50%)");
            Map(m => m.FAB_x002d_Kit_x002f_TO_x0020__x0).Name("FAB-Kit/TO (10%)");
            Map(m => m._x0071_gx7).Name("FAB-MHs");
            Map(m => m.Fab_x002d_QC_x0020__x0028_10_x00).Name("FAB-QC (10%)");
            Map(m => m.FAB_x002d_SHOP).Name("FAB-SHOP");
            Map(m => m.nb0x).Name("FAB-Shop ACTUAL Welds");
            Map(m => m.qfrm).Name("FAB-Shop Foreman");
            Map(m => m.lhfu).Name("FAB-Spool Pieces");
            Map(m => m.yvrw).Name("FAB-Transmittal No.");
            Map(m => m.FAB_x002d_Weld_x0020__x0028_30_x).Name("FAB-Weld (30%)");
            Map(m => m.w11d).Name("FIELD-Act.WeldsCompl.");
            Map(m => m.czjw).Name("FIELD-Actual LF");
            Map(m => m.xpin).Name("FIELD-Actual QtySUPPs");
            Map(m => m.ip0d).Name("Plan QtySUPPORTS");
            Map(m => m.Field_x002d_MHs).Name("FIELD-MHs");
            Map(m => m.Predecessors).Name("Predecessors");
            Map(m => m.Priority).Name("Priority");
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m.RelatedItems).Name("Related Items");
            Map(m => m.Supervisor).Name("Supervisor");
            Map(m => m.Weight_x0020__x0028_lbs_x0029_).Name("Weight (lbs)");

        }
    }
}

