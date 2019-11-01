using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.Configuration.Attributes;
using CsvHelper.Expressions;
using sp = Microsoft.SharePoint.Client;




namespace KSI_List_ReportProcessing
{
    public class ITLRecord
    {
        public string Insul_x0020_Spec { get; set; }
        public string Line { get; set; }
        public string Sht_x002e_ { get; set; }
        public string P_x0026_ID_x0020_No_x002e_ { get; set; }
        public string Title { get; set; }
        public string Area_x002d_Task_x002d_Name_x0028 { get; set; }
        public string Batch_x0020_No { get; set; }
        public DateTime c7z3 { get; set; }
        public DateTime p37l { get; set; }
        public DateTime h7r6 { get; set; }
        public DateTime cuci { get; set; }
        public DateTime nigq { get; set; }
        public DateTime awcz { get; set; }
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
        public string Comments { get; set; }
        public string Cc { get; set; }
        public string uz0w { get; set; }
        public string _x0061_y74 { get; set; }
        public string _x0073_rp1 { get; set; }
        public decimal _x0070_j39 { get; set; }
        public decimal h35h { get; set; }
        public decimal avnf { get; set; }
        public decimal ydxs { get; set; }
        public decimal j2a7 { get; set; }
        public decimal bcav { get; set; }
        public decimal Body { get; set; }
        public decimal FAB_x002d_Comments_x002f_Mat_x00 { get; set; }
        public decimal DueDate { get; set; }
        public decimal FAB_x002d_Cut_x002f_Tack_x0020__ { get; set; }
        public decimal FAB_x002d_Kit_x002f_TO_x0020__x0 { get; set; }
        public decimal _x0071_gx7 { get; set; }
        public decimal Fab_x002d_QC_x0020__x0028_10_x00 { get; set; }
        public decimal FAB_x002d_SHOP { get; set; }
        public string nb0x { get; set; }
        public string qfrm { get; set; }
        public string lhfu { get; set; }
        public string yvrw { get; set; }
        public string FAB_x002d_Weld_x0020__x0028_30_x { get; set; }
        public string w11d { get; set; }
        public string czjw { get; set; }
        public string xpin { get; set; }
        public string ip0d { get; set; }
        public string Field_x002d_MHs { get; set; }
        public string Predecessors { get; set; }
        public string Priority { get; set; }
        public string Project_x0020_Manager { get; set; }
        public string Supervisor { get; set; }
        public string Weight_x0020__x0028_lbs_x0029_ { get; set; }


    }


    public sealed class ProjectKPIMap : ClassMap<ITLRecord>
    {
        public ProjectITLMap()
        {
            Map(m => m.Insul_x0020_Spec).Name("Insul Spec").ConvertUsing(NullDecimalParser);
            Map(m => m.Line).Name("Line").ConvertUsing(NullDecimalParser);
            Map(m => m.Sht_x002e_).Name("Sht.").ConvertUsing(NullDecimalParser);
            Map(m => m.P_x0026_ID_x0020_No_x002e_).Name("P&ID No.");
            Map(m => m.Title).Name("______________________ISO Number______________________");
            Map(m => m.Area_x002d_Task_x002d_Name_x0028).Name("Area-Task-Name(from C.C. List)");
            Map(m => m.Batch_x0020_No).Name("Batch No");
            Map(m => m.c7z3).Name("Vessel_or_Location");
            Map(m => m.p37l).Name("Rev");
            Map(m => m.h7r6).Name("Plan Ft.").ConvertUsing(NullDecimalParser);
            Map(m => m.cuci).Name("Plan FWs").ConvertUsing(NullDecimalParser);
            Map(m => m.nigq).Name("Plan SWs").ConvertUsing(NullDecimalParser);
            Map(m => m.awcz).Name("Date-IFR Sub").ConvertUsing(NullDateTimeParser);
            Map(m => m._x0069_kt1).Name("Date-Approved").ConvertUsing(NullDateTimeParser);
            Map(m => m.rz4k).Name("Date-IFC").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x002d_OwnerMat_x002e_ETA).Name("Date-OwnerMat.ETA").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x0020_FAB_x0020_Complete).Name("Date-FAB Complete").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x0020_FAB_x0020_Received).Name("Date-FAB Received").ConvertUsing(NullDateTimeParser);
            Map(m => m.x24v).Name("Date-SUPPORTSInst").ConvertUsing(NullDateTimeParser);
            Map(m => m.oiqs).Name("Date-InRack_Erected").ConvertUsing(NullDateTimeParser);
            Map(m => m.i67c).Name("Date-FieldWelded").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x002d_SlopeCheck_x0028_QC_x).Name("Date-SlopeCheck_QC").ConvertUsing(NullDateTimeParser);
            Map(m => m._x006b_o58).Name("Date-Tested").ConvertUsing(NullDateTimeParser);
            Map(m => m.ah1c).Name("Date-MCWalk").ConvertUsing(NullDateTimeParser);
            Map(m => m.qh8m).Name("Added FWs").ConvertUsing(NullDecimalParser);
            Map(m => m._x0076_h85).Name("BIM-Detailer");
            Map(m => m.Detailing_x0020_Spool_x0020_Stat).Name("BIM-Detailing Spool Status");
            Map(m => m.PercentComplete).Name("% Complete").ConvertUsing(NullDecimalParser);
            Map(m => m.Comments).Name("Comments");
            Map(m => m.Cc).Name("Cc");
            Map(m => m.uz0w).Name("Change Order");
            Map(m => m._x0061_y74).Name("Date-DEMO/or/Eqmnt. Inst.").ConvertUsing(NullDateTimeParser);
            Map(m => m._x0073_rp1).Name("Date-Expected onsite").ConvertUsing(NullDateTimeParser);
            Map(m => m._x0070_j39).Name("Date-FAB Shipped").ConvertUsing(NullDateTimeParser);
            Map(m => m.h35h).Name("Date-Install Needed by").ConvertUsing(NullDateTimeParser);
            Map(m => m.avnf).Name("Date-Insulated").ConvertUsing(NullDateTimeParser);
            Map(m => m.ydxs).Name("Date-ISO Needed").ConvertUsing(NullDateTimeParser);
            Map(m => m.j2a7).Name("Date-Passivated").ConvertUsing(NullDateTimeParser);
            Map(m => m.bcav).Name("Date-Rls to FAB").ConvertUsing(NullDateTimeParser);
            Map(m => m.Body).Name("Description");
            Map(m => m.FAB_x002d_Comments_x002f_Mat_x00).Name("FAB-Comments/Mat.Shorts?");
            Map(m => m.DueDate).Name("Due Date").ConvertUsing(NullDateTimeParser);
            Map(m => m.FAB_x002d_Cut_x002f_Tack_x0020__).Name("FAB-Cut/Tack (50%)");
            Map(m => m.FAB_x002d_Kit_x002f_TO_x0020__x0).Name("FAB-Kit/TO (10%)");
            Map(m => m._x0071_gx7).Name("FAB-MHs").ConvertUsing(NullDecimalParser);
            Map(m => m.Fab_x002d_QC_x0020__x0028_10_x00).Name("FAB-QC (10%)");
            Map(m => m.FAB_x002d_SHOP).Name("FAB-SHOP");
            Map(m => m.nb0x).Name("FAB-Shop ACTUAL Welds").ConvertUsing(NullDecimalParser);
            Map(m => m.qfrm).Name("FAB-Shop Foreman");
            Map(m => m.lhfu).Name("FAB-Spool Pieces").ConvertUsing(NullDecimalParser);
            Map(m => m.yvrw).Name("FAB-Transmittal No.");
            Map(m => m.FAB_x002d_Weld_x0020__x0028_30_x).Name("FAB-Weld (30%)");
            Map(m => m.w11d).Name("FIELD-Act.WeldsCompl.").ConvertUsing(NullDecimalParser);
            Map(m => m.czjw).Name("FIELD-Actual LF").ConvertUsing(NullDecimalParser);
            Map(m => m.xpin).Name("FIELD-Actual QtySUPPs").ConvertUsing(NullDecimalParser);
            Map(m => m.ip0d).Name("Plan QtySUPPORTS").ConvertUsing(NullDecimalParser);
            Map(m => m.Field_x002d_MHs).Name("FIELD-MHs").ConvertUsing(NullDecimalParser);
            Map(m => m.Predecessors).Name("Predecessors");
            Map(m => m.Priority).Name("Priority");
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m.Supervisor).Name("Supervisor");
            Map(m => m.Weight_x0020__x0028_lbs_x0029_).Name("Weight (lbs)").ConvertUsing(NullDecimalParser);



            Decimal? NullDecimalParser(IReaderRow row)
            {
                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "")
                    return 0;
                else
                if (rawValue == null)
                    return 0;
                else
                    return Decimal.Parse(rawValue);
            }


            DateTime? NullDateTimeParser(IReaderRow row)
            {
                var d = new DateTime(2000, 1, 1);

                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "")
                    return d;

                //return null;
                else
                    //  return rawValueTypeConverterOption.Format("M/d/yyyy");
                    return DateTime.Parse(rawValue);
            }

            string NullStringParser(IReaderRow row)
            {

                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "null")
                    return "";
                else
                    //  return rawValueTypeConverterOption.Format("M/d/yyyy");
                    return rawValue.ToString();
            }

        }
    }


}

