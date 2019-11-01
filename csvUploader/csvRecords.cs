using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace CSVUploader
{
    public class ITLRecord
    {
        public string P_x0026_ID_x0020_No_x002e_ { get; set; }//P&ID No.
        public string Title { get; set; }//______________________ISO Number______________________
        public string Area_x002d_Task_x002d_Name_x0028 { get; set; }//Area-Task-Name(from C.C. List)
        public string c7z3 { get; set; }//Vessel_or_Location
        public string Batch_x0020_No { get; set; }//Batch No
        public string p37l { get; set; }//Rev
        public decimal? h7r6 { get; set; }//Plan Ft.
        public decimal? nigq { get; set; }//Plan SWs
        public decimal? cuci { get; set; }//Plan FWs
        public decimal? qh8m { get; set; }//Added FWs
        public decimal? Added_x0020_SWs { get; set; }//Added SWs
        public decimal? ip0d { get; set; }//Plan QtySUPPORTS
        public string uz0w { get; set; }//Change Order
        public string _x0076_h85 { get; set; }//BIM-Detailer
        public string Detailing_x0020_Spool_x0020_Stat { get; set; }//BIM-Detailing Spool Status
        public string Cc { get; set; }//Cc
        public string Supervisor { get; set; }//Supervisor
        public string Project_x0020_Manager { get; set; }//Project Manager
        public DateTime? _x0069_kt1 { get; set; }//Date-Approved
        public DateTime? StartDate { get; set; }//Start Date
        public DateTime? _x0061_y74 { get; set; }//Date-DEMO/or/Eqmnt. Inst.
        public DateTime? _x0073_rp1 { get; set; }//Date-Expected onsite
        public DateTime? Date_x0020_FAB_x0020_Complete { get; set; }//Date-FAB Complete
        public DateTime? Date_x0020_FAB_x0020_Received { get; set; }//Date-FAB Received
        public DateTime? _x0070_j39 { get; set; }//Date-FAB Shipped
        public DateTime? i67c { get; set; }//Date-FieldWelded
        public DateTime? rz4k { get; set; }//Date-IFC
        public DateTime? awcz { get; set; }//Date-IFR Sub
        public DateTime? oiqs { get; set; }//Date-InRack_Erected
        public DateTime? h35h { get; set; }//Date-Install Needed by
        public DateTime? avnf { get; set; }//Date-Insulated
        public DateTime? ydxs { get; set; }//Date-ISO Needed
        public DateTime? ah1c { get; set; }//Date-MCWalk
        public DateTime? Date_x002d_OwnerMat_x002e_ETA { get; set; }//Date-OwnerMat.ETA
        public DateTime? j2a7 { get; set; }//Date-Passivated
        public DateTime? bcav { get; set; }//Date-Rls to FAB
        public DateTime? Date_x002d_SlopeCheck_x0028_QC_x { get; set; }//Date-SlopeCheck_QC
        public DateTime? x24v { get; set; }//Date-SUPPORTSInst
        public DateTime? _x006b_o58 { get; set; }//Date-Tested
        public DateTime? DueDate { get; set; }//Due Date
        public string Body { get; set; }//Description
        public string FAB_x002d_Comments_x002f_Mat_x00 { get; set; }//FAB-Comments/Mat.Shorts
        public string FAB_x002d_Cut_x002f_Tack_x0020__ { get; set; }//FAB-Cut/Tack (50%)
        public string FAB_x002d_Kit_x002f_TO_x0020__x0 { get; set; }//FAB-Kit/TO (10%)
        public string Fab_x002d_QC_x0020__x0028_10_x00 { get; set; }//FAB-QC (10%)
        public string FAB_x002d_Weld_x0020__x0028_30_x { get; set; }//FAB-Weld (30%)
        public decimal? _x0071_gx7 { get; set; }//FAB-MHs
        public decimal? lhfu { get; set; }//FAB-Spool Pieces
        public decimal? nb0x { get; set; }//FAB-Shop ACTUAL Welds
        public string FAB_x002d_SHOP { get; set; }//FAB-SHOP
        public string yvrw { get; set; }//FAB-Transmittal No.
        public string qfrm { get; set; }//FAB-Shop Foreman
        public decimal? w11d { get; set; }//FIELD-Act.WeldsCompl.
        public decimal? czjw { get; set; }//FIELD-Actual LF
        public decimal? xpin { get; set; }//FIELD-Actual QtySUPPs
        public decimal? Field_x002d_MHs { get; set; }//FIELD-MHs
        public decimal? PercentComplete { get; set; }//% Complete
        //public string Predecessors { get; set; }//Predecessors
        public string Priority { get; set; }//Priority
        public string Comments { get; set; }//Comments
        public decimal? Weight_x0020__x0028_lbs_x0029_ { get; set; }//Weight (lbs)


    }

    public class ToBeIgnoredAttribute : Attribute
    {
    }

    public sealed class ProjectITLMap : ClassMap<ITLRecord>
    {
        public ProjectITLMap()
        {
            Map(m => m.P_x0026_ID_x0020_No_x002e_).Name("P&ID No.");
            Map(m => m.Title).Name("______________________ISO Number______________________");
            Map(m => m.Area_x002d_Task_x002d_Name_x0028).Name("Area-Task-Name(from C.C. List)");

            Map(m => m.c7z3).Name("Vessel_or_Location");
            Map(m => m.Batch_x0020_No).Name("Batch No");
            Map(m => m.p37l).Name("Rev");
            Map(m => m.h7r6).Name("Plan Ft.").ConvertUsing(NullDecimalParser);
            Map(m => m.nigq).Name("Plan SWs").ConvertUsing(NullDecimalParser);
            Map(m => m.cuci).Name("Plan FWs").ConvertUsing(NullDecimalParser);
            Map(m => m.qh8m).Name("Added FWs").ConvertUsing(NullDecimalParser);
            Map(m => m.Added_x0020_SWs).Name("Added SWs").ConvertUsing(NullDecimalParser);
            Map(m => m.ip0d).Name("Plan QtySUPPORTS").ConvertUsing(NullDecimalParser);
            Map(m => m.uz0w).Name("Change Order");
            Map(m => m._x0076_h85).Name("BIM-Detailer");
            Map(m => m.Detailing_x0020_Spool_x0020_Stat).Name("BIM-Detailing Spool Status");
            Map(m => m.Cc).Name("Cc");
            Map(m => m.Supervisor).Name("Supervisor");
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m._x0069_kt1).Name("Date-Approved").ConvertUsing(NullDateTimeParser);
            Map(m => m.StartDate).Name("Start Date").ConvertUsing(NullDateTimeParser);
            Map(m => m._x0061_y74).Name("Date-DEMO/or/Eqmnt. Inst.").ConvertUsing(NullDateTimeParser);
            Map(m => m._x0073_rp1).Name("Date-Expected onsite").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x0020_FAB_x0020_Complete).Name("Date-FAB Complete").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x0020_FAB_x0020_Received).Name("Date-FAB Received").ConvertUsing(NullDateTimeParser);
            Map(m => m._x0070_j39).Name("Date-FAB Shipped").ConvertUsing(NullDateTimeParser);
            Map(m => m.i67c).Name("Date-FieldWelded").ConvertUsing(NullDateTimeParser);
            Map(m => m.rz4k).Name("Date-IFC").ConvertUsing(NullDateTimeParser);
            Map(m => m.awcz).Name("Date-IFR Sub").ConvertUsing(NullDateTimeParser);
            Map(m => m.oiqs).Name("Date-InRack_Erected").ConvertUsing(NullDateTimeParser);
            Map(m => m.h35h).Name("Date-Install Needed by").ConvertUsing(NullDateTimeParser);
            Map(m => m.avnf).Name("Date-Insulated").ConvertUsing(NullDateTimeParser);
            Map(m => m.ydxs).Name("Date-ISO Needed").ConvertUsing(NullDateTimeParser);
            Map(m => m.ah1c).Name("Date-MCWalk").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x002d_OwnerMat_x002e_ETA).Name("Date-OwnerMat.ETA").ConvertUsing(NullDateTimeParser);
            Map(m => m.j2a7).Name("Date-Passivated").ConvertUsing(NullDateTimeParser);
            Map(m => m.bcav).Name("Date-Rls to FAB").ConvertUsing(NullDateTimeParser);
            Map(m => m.Date_x002d_SlopeCheck_x0028_QC_x).Name("Date-SlopeCheck_QC").ConvertUsing(NullDateTimeParser);
            Map(m => m.x24v).Name("Date-SUPPORTSInst").ConvertUsing(NullDateTimeParser);
            Map(m => m._x006b_o58).Name("Date-Tested").ConvertUsing(NullDateTimeParser);
            Map(m => m.DueDate).Name("Due Date").ConvertUsing(NullDateTimeParser);
            Map(m => m.Body).Name("Description");
            Map(m => m.FAB_x002d_Comments_x002f_Mat_x00).Name("FAB-Comments/Mat.Shorts?");
            Map(m => m.FAB_x002d_Cut_x002f_Tack_x0020__).Name("FAB-Cut/Tack (50%)");
            Map(m => m.FAB_x002d_Kit_x002f_TO_x0020__x0).Name("FAB-Kit/TO (10%)");
            Map(m => m.Fab_x002d_QC_x0020__x0028_10_x00).Name("FAB-QC (10%)");
            Map(m => m.FAB_x002d_Weld_x0020__x0028_30_x).Name("FAB-Weld (30%)");
            Map(m => m._x0071_gx7).Name("FAB-MHs").ConvertUsing(NullDecimalParser);
            Map(m => m.lhfu).Name("FAB-Spool Pieces").ConvertUsing(NullDecimalParser);
            Map(m => m.nb0x).Name("FAB-Shop ACTUAL Welds").ConvertUsing(NullDecimalParser);
            Map(m => m.FAB_x002d_SHOP).Name("FAB-SHOP");
            Map(m => m.yvrw).Name("FAB-Transmittal No.");
            Map(m => m.qfrm).Name("FAB-Shop Foreman");
            Map(m => m.w11d).Name("FIELD-Act.WeldsCompl.").ConvertUsing(NullDecimalParser);
            Map(m => m.czjw).Name("FIELD-Actual LF").ConvertUsing(NullDecimalParser);
            Map(m => m.xpin).Name("FIELD-Actual QtySUPPs").ConvertUsing(NullDecimalParser);
            Map(m => m.Field_x002d_MHs).Name("FIELD-MHs").ConvertUsing(NullDecimalParser);
            Map(m => m.PercentComplete).Name("% Complete").ConvertUsing(NullDecimalParser);
            //Map(m => m.Predecessors).Name("Predecessors");
            Map(m => m.Priority).Name("Priority");
            Map(m => m.Comments).Name("Comments");
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
                try
                {
                    var d = new DateTime(2000, 1, 1);

                    var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                    if (rawValue == "")
                        return d;
                    else
                        return DateTime.Parse(rawValue);
                }
                catch (Exception ex)
                {
                    ex.Data.Add("Info", row.GetField(row.GetField(row.Context.CurrentIndex + 1)));
                    throw ex;
                }
            }

            string NullStringParser(IReaderRow row)
            {

                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "null")
                    return "";
                else
                    return rawValue.ToString();
            }

        }
    }

    public class StaticStringConverter : DefaultTypeConverter
    {
        public string ConvertToString(TypeConverterOptions options, object value)
        {
            return "my static string";
        }
    }
}