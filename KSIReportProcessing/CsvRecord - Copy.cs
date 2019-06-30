using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Expressions;
using CsvHelper;
using sp = Microsoft.SharePoint.Client;

namespace ImportListFromCSV
{

    public class CsvRecord
    {
        public string Project { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Project_x0020_Manager { get; set; }
        public string Operations_x0020_Manager { get; set; }
        public string Client { get; set; }
        public string Category { get; set; }
        /*
        public decimal? Original_x0020_Contract { get; set; }
        public decimal? BIX_x0020__x002f__x0020_CIX { get; set; }
        public decimal? Total_x0020_Scope_x0020_Changes { get; set; }
        public decimal? Contract_x0020_Growth_x0020__x00 { get; set; }
        public decimal? Proceeding_x0020_Scope_x0020_Cha { get; set; }
        public decimal? _x0025__x0020_of_x0020_COs_x0020 { get; set; }
        public decimal? Total_x0020_Contract_x0020_Amoun { get; set; }
        public decimal? Total_x0020_Billings { get; set; }
        public decimal? Open_x0020_Commitments_x0020_ { get; set; }
        public decimal? JTD_x0020_Cost_x0020_ { get; set; }
        public decimal? Paid_x0020_to_x0020_Supp_x002e__ { get; set; }
        public decimal? Received_x0020_from_x0020_Cust_x { get; set; }
        public decimal? JTD_x0020__x0025__x0020_Complete { get; set; }
        public decimal? JTD_x002b_OpenCmt_x0020__x0025__ { get; set; }
        public decimal? Cash_x0020_Position { get; set; }
        public decimal? Collections_x0020_past_x0020_due { get; set; }
        public decimal? PM_x0020_Fcst_x0020_Cost { get; set; }
        public decimal? Original_x0020_Margin { get; set; }
        public decimal? PM_x0027_s_x0020_Margin { get; set; }
        public decimal? Budget_x0020_LABOR_x0020_Cost { get; set; }
        public decimal? ACTUAL_x0020_LABOR_x0020_Cost { get; set; }
        public decimal? LABOR_x0020__x0025__x0020_Spent { get; set; }
        public decimal? Budget_x0020_MAT_x0026_EQMNT_x00 { get; set; }
        public decimal? ACTUAL_x0020_MAT_x0026_EQMNT_x00 { get; set; }
        public decimal? MAT_x0020__x0025__x0020_Spent { get; set; }
        public decimal? Budget_x0020_GCs_x0020_Cost { get; set; }
        public decimal? ACTUAL_x0020_GCs_x0020_Cost { get; set; }
        public decimal? GC_x0027_s_x0020__x0025__x0020_S { get; set; }
        public decimal? Budget_x0020_SUBs_x0020_Cost { get; set; }
        public decimal? ACTUAL_x0020_SUBs_x0020_Cost { get; set; }
        public decimal? SUBs_x0020__x0025__x0020_Spent { get; set; }
        public decimal? Collections_x0020_due { get; set; }
        public decimal? Original_x0020_BUDGET_x0020_Cost { get; set; }
        public decimal? Total_x0020_BUDGET_x0020_Cost { get; set; }


        public DateTime L_x002e_Invoice_x0020_to_x0020_C { get; set; }
        public DateTime Start_x0020_Time { get; set; }
        public DateTime FiniTime { get; set; }
        */
    }

    public sealed class ProjectKPIMap : ClassMap<CsvRecord>
    {
        public ProjectKPIMap()
        {

            Map(m => m.Project).Name("Project");
            Map(m => m.Description).Name("Description");
            Map(m => m.Status).Name("Status");
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m.Operations_x0020_Manager).Name("Operations Manager");
            Map(m => m.Client).Name("Client");
            Map(m => m.Category).Name("Category");
            /*
            Map(m => m.Original_x0020_Contract).ConvertUsing(NullDecimalParser); 
            Map(m => m.BIX_x0020__x002f__x0020_CIX).ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_Scope_x0020_Changes).ConvertUsing(NullDecimalParser);
            Map(m => m.Contract_x0020_Growth_x0020__x00).ConvertUsing(NullDecimalParser);
            Map(m => m.Proceeding_x0020_Scope_x0020_Cha).ConvertUsing(NullDecimalParser);
            Map(m => m._x0025__x0020_of_x0020_COs_x0020).ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_Contract_x0020_Amoun).ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_Billings).ConvertUsing(NullDecimalParser);                                                                                                                                                                                                                                                                                                                                                                       
            Map(m => m.Open_x0020_Commitments_x0020_).ConvertUsing(NullDecimalParser);
            Map(m => m.JTD_x0020_Cost_x0020_).ConvertUsing(NullDecimalParser);
            Map(m => m.Paid_x0020_to_x0020_Supp_x002e__).ConvertUsing(NullDecimalParser);
            Map(m => m.Received_x0020_from_x0020_Cust_x).ConvertUsing(NullDecimalParser);
            Map(m => m.JTD_x0020__x0025__x0020_Complete).ConvertUsing(NullDecimalParser);
            Map(m => m.JTD_x002b_OpenCmt_x0020__x0025__).ConvertUsing(NullDecimalParser);
            Map(m => m.Cash_x0020_Position).ConvertUsing(NullDecimalParser);
            Map(m => m.Collections_x0020_past_x0020_due).ConvertUsing(NullDecimalParser);
            Map(m => m.PM_x0020_Fcst_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.Original_x0020_Margin).ConvertUsing(NullDecimalParser);
            Map(m => m.PM_x0027_s_x0020_Margin).ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_LABOR_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_LABOR_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.LABOR_x0020__x0025__x0020_Spent).ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_MAT_x0026_EQMNT_x00).ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_MAT_x0026_EQMNT_x00).ConvertUsing(NullDecimalParser);
            Map(m => m.MAT_x0020__x0025__x0020_Spent).ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_GCs_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_GCs_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.GC_x0027_s_x0020__x0025__x0020_S).ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_SUBs_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_SUBs_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.SUBs_x0020__x0025__x0020_Spent).ConvertUsing(NullDecimalParser);
            Map(m => m.Collections_x0020_due).ConvertUsing(NullDecimalParser);
            Map(m => m.Original_x0020_BUDGET_x0020_Cost).ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_BUDGET_x0020_Cost).ConvertUsing(NullDecimalParser);


            Map(m => m.L_x002e_Invoice_x0020_to_x0020_C).Name("LastInvoice to Cust").TypeConverterOption.Format("dd-MM-yyyy");
            Map(m => m.Start_x0020_Time).Name("Start Time").TypeConverterOption.Format("dd-MM-yyyy");
            Map(m => m.FiniTime).Name("FinishTime").TypeConverterOption.Format("dd-MM-yyyy");

    */



            Decimal? NullDecimalParser(IReaderRow row)
            {
                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "")
                    return 0;
                else
                    return Decimal.Parse(rawValue);
            }


            DateTime? NullDateTimeParser(IReaderRow row)
            {
                var rawValue = row.GetField(row.Context.CurrentIndex + 1);
                if (rawValue == "")
                    return null;
                else
                    return DateTime.Parse(rawValue);
            }



        }
    }
}