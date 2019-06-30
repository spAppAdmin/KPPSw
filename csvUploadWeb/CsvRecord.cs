using CsvHelper;
using CsvHelper.Configuration;
using System;



namespace csvUploadWeb
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




            Decimal? NullDecimalParser(IReaderRow row)
            {
                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "")
                    return 0;
                else
                if(rawValue == null)
                    return 0;
                else
                    return Decimal.Parse(rawValue);
            }


            DateTime? NullDateTimeParser (IReaderRow row)
            {
                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "")
                    return DateTime.MinValue;
                else
                    //  return rawValueTypeConverterOption.Format("M/d/yyyy");
                    return DateTime.Parse(rawValue);


            }

            }
    }
}

