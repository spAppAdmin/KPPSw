using CsvHelper;
using CsvHelper.Configuration;
using System;//



namespace csvUploadWeb
{
    public class ITLRecord
    {
        public string P_x0026_ID_x0020_No_x002e_ { get; set; }
        public string Title { get; set; }
        public string Area_x002d_Task_x002d_Name_x0028 { get; set; }
        public string c7z3 { get; set; }
        public string Batch_x0020_No { get; set; }
        public string p37l { get; set; }
        /*
        public decimal h7r6 { get; set; }
        public decimal nigq { get; set; }
        public decimal cuci { get; set; }
        public decimal qh8m { get; set; }
        public decimal Added_x0020_SWs { get; set; }
        public decimal ip0d { get; set; }
        public string uz0w { get; set; }
        public string _x0076_h85 { get; set; }
        public string Detailing_x0020_Spool_x0020_Stat { get; set; }
        public string Cc { get; set; }
        public string Supervisor { get; set; }
        public string Project_x0020_Manager { get; set; }

        public DateTime _x0069_kt1 { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime _x0061_y74 { get; set; }
        public DateTime _x0073_rp1 { get; set; }
        public DateTime Date_x0020_FAB_x0020_Complete { get; set; }
        public DateTime Date_x0020_FAB_x0020_Received { get; set; }
        public DateTime _x0070_j39 { get; set; }
        public DateTime i67c { get; set; }
        public DateTime rz4k { get; set; }
        public DateTime awcz { get; set; }
        public DateTime oiqs { get; set; }
        public DateTime h35h { get; set; }
        public DateTime avnf { get; set; }
        public DateTime ydxs { get; set; }
        public DateTime ah1c { get; set; }
        public DateTime Date_x002d_OwnerMat_x002e_ETA { get; set; }
        public DateTime j2a7 { get; set; }
        public DateTime bcav { get; set; }
        public DateTime Date_x002d_SlopeCheck_x0028_QC_x { get; set; }
        public DateTime x24v { get; set; }
        public DateTime _x006b_o58 { get; set; }
        public DateTime DueDate { get; set; }

        public string Body { get; set; }
        public string FAB_x002d_Comments_x002f_Mat_x00 { get; set; }
        public string FAB_x002d_Cut_x002f_Tack_x0020__ { get; set; }
        public string FAB_x002d_Kit_x002f_TO_x0020__x0 { get; set; }
        public decimal _x0071_gx7 { get; set; }
        public string Fab_x002d_QC_x0020__x0028_10_x00 { get; set; }
        public string FAB_x002d_SHOP { get; set; }
        public decimal nb0x { get; set; }
        public string qfrm { get; set; }
        public decimal lhfu { get; set; }
        public string yvrw { get; set; }
        public string FAB_x002d_Weld_x0020__x0028_30_x { get; set; }
        public decimal w11d { get; set; }
        public decimal czjw { get; set; }
        public decimal xpin { get; set; }
        public decimal Field_x002d_MHs { get; set; }
        public string Predecessors { get; set; }
        public string Priority { get; set; }
        public decimal PercentComplete { get; set; }
        public decimal Weight_x0020__x0028_lbs_x0029_ { get; set; }
        public string Comments { get; set; }
        */
    }

    public class ToBeIgnoredAttribute : Attribute
    {

    }

    public sealed class ProjectITLMap : ClassMap<ITLRecord>
    {
        public ProjectITLMap()
        {
            AutoMap();        


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
                    return null;
                else
                  //  return rawValueTypeConverterOption.Format("M/d/yyyy");
                return DateTime.Parse(rawValue);


            }

            }
    }
}

