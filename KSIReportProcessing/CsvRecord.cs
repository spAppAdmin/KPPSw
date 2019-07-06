using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using CsvHelper.Expressions;
using CsvHelper;
using sp = Microsoft.SharePoint.Client;
using CsvHelper.Configuration.Attributes;



namespace ImportListFromCSV
{

    public class CsvRecord
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public decimal? Original_x0020_Contract { get; set; }
        public decimal? BIX_x0020__x002f__x0020_CIX { get; set; }
        public decimal? Total_x0020_Scope_x0020_Changes { get; set; }
        public decimal? Contract_x0020_Growth_x0020__x00 { get; set; }
        public decimal? Proceeding_x0020_Scope_x0020_Cha { get; set; }
        public decimal? _x0025__x0020_of_x0020_COs_x0020 { get; set; }
        public decimal? Total_x0020_Contract_x0020_Amoun { get; set; }
        public string Project_x0020_Manager { get; set; }
        public string Operations_x0020_Manager { get; set; }
        public string Client { get; set; }
        public decimal? Total_x0020_Billings { get; set; }
        public decimal? Open_x0020_Commitments_x0020_ { get; set; }
        public decimal? JTD_x0020_Cost_x0020_ { get; set; }
        public decimal? Paid_x0020_to_x0020_Supp_x002e__ { get; set; }
        public decimal? Received_x0020_from_x0020_Cust_x { get; set; }
        public decimal? JTD_x0020__x0025__x0020_Complete { get; set; }
        public decimal? JTD_x002b_OpenCmt_x0020__x0025__ { get; set; }
        public decimal? Cash_x0020_Position { get; set; }
        public decimal? Collections_x0020_past_x0020_due { get; set; }
        public DateTime? L_x002e_Invoice_x0020_to_x0020_C { get; set; }
        public decimal? PM_x0020_Fcst_x0020_Cost { get; set; }
        public DateTime? Start_x0020_Time { get; set; }
        public DateTime? FiniTime { get; set; }
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
        public string Category { get; set; }
        public decimal? Collections_x0020_due { get; set; }
        public decimal? Original_x0020_BUDGET_x0020_Cost { get; set; }
        public decimal? Total_x0020_BUDGET_x0020_Cost { get; set; }
        public decimal? OriginalDetailingBudget { get; set; }
        public decimal? ChangeOrdersDetailingBudget { get; set; }
        public decimal? CurrentDetailingBudget { get; set; }
        public decimal? ActualDetailingCost { get; set; }
        public decimal? DetailingBurnedRatio { get; set; }

    }


    public sealed class ProjectKPIMap : ClassMap<CsvRecord>
    {
        public ProjectKPIMap()
        {

            Map(m => m.Title).Name("Project");
            Map(m => m.Description).Name("Description");
            Map(m => m.Status).Name("Status");
            Map(m => m.Original_x0020_Contract).Name("Original Contract").ConvertUsing(NullDecimalParser);
            Map(m => m.BIX_x0020__x002f__x0020_CIX).Name("BIX / CIX").ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_Scope_x0020_Changes).Name("Total Scope Changes").ConvertUsing(NullDecimalParser);
            Map(m => m.Contract_x0020_Growth_x0020__x00).Name("Contract Growth (%)").ConvertUsing(NullDecimalParser);
            Map(m => m.Proceeding_x0020_Scope_x0020_Cha).Name("Proceeding Scope Changes").ConvertUsing(NullDecimalParser);
            Map(m => m._x0025__x0020_of_x0020_COs_x0020).Name("% of COs in Proceeding").ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_Contract_x0020_Amoun).Name("Total Contract Amount").ConvertUsing(NullDecimalParser);
            Map(m => m.Project_x0020_Manager).Name("Project Manager");
            Map(m => m.Operations_x0020_Manager).Name("Operations Manager");
            Map(m => m.Client).Name("Client");
            Map(m => m.Total_x0020_Billings).Name("Total Billings").ConvertUsing(NullDecimalParser);
            Map(m => m.Open_x0020_Commitments_x0020_).Name("Open Commitments").ConvertUsing(NullDecimalParser);
            Map(m => m.JTD_x0020_Cost_x0020_).Name("JTD Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.Paid_x0020_to_x0020_Supp_x002e__).Name("Paid to Supp").ConvertUsing(NullDecimalParser);
            Map(m => m.Received_x0020_from_x0020_Cust_x).Name("Received from Cust").ConvertUsing(NullDecimalParser);
            Map(m => m.JTD_x0020__x0025__x0020_Complete).Name("JTD % Complete").ConvertUsing(NullDecimalParser);
            Map(m => m.JTD_x002b_OpenCmt_x0020__x0025__).Name("JTD+OpenCmt % Complete").ConvertUsing(NullDecimalParser);
            Map(m => m.Cash_x0020_Position).Name("Cash Position").ConvertUsing(NullDecimalParser);
            Map(m => m.Collections_x0020_past_x0020_due).Name("Collections past due").ConvertUsing(NullDecimalParser);
            Map(m => m.L_x002e_Invoice_x0020_to_x0020_C).Name("LastInvoice to Cust").ConvertUsing(NullDateTimeParser);
            Map(m => m.PM_x0020_Fcst_x0020_Cost).Name("PM Fcst Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.Start_x0020_Time).Name("Start Time").ConvertUsing(NullDateTimeParser);
            Map(m => m.FiniTime).Name("FinishTime").ConvertUsing(NullDateTimeParser);
            Map(m => m.Original_x0020_Margin).Name("Original Margin").ConvertUsing(NullDecimalParser);
            Map(m => m.PM_x0027_s_x0020_Margin).Name("PMs Margin").ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_LABOR_x0020_Cost).Name("Budget LABOR Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_LABOR_x0020_Cost).Name("ACTUAL LABOR Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.LABOR_x0020__x0025__x0020_Spent).Name("LABOR % Spent").ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_MAT_x0026_EQMNT_x00).Name("Budget MAT&EQMNT Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_MAT_x0026_EQMNT_x00).Name("ACTUAL MAT&EQMNT Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.MAT_x0020__x0025__x0020_Spent).Name("MAT % Spent").ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_GCs_x0020_Cost).Name("Budget GCs Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_GCs_x0020_Cost).Name("ACTUAL GCs Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.GC_x0027_s_x0020__x0025__x0020_S).Name("GCs % Spent").ConvertUsing(NullDecimalParser);
            Map(m => m.Budget_x0020_SUBs_x0020_Cost).Name("Budget SUBs Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.ACTUAL_x0020_SUBs_x0020_Cost).Name("ACTUAL SUBs Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.SUBs_x0020__x0025__x0020_Spent).Name("SUBs % Spent").ConvertUsing(NullDecimalParser);
            Map(m => m.Category).Name("Category");
            Map(m => m.Collections_x0020_due).Name("Collections due").ConvertUsing(NullDecimalParser);
            Map(m => m.Original_x0020_BUDGET_x0020_Cost).Name("Original BUDGET Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.Total_x0020_BUDGET_x0020_Cost).Name("Total BUDGET Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.OriginalDetailingBudget).Name("Original Detailing Budget").ConvertUsing(NullDecimalParser);
            Map(m => m.ChangeOrdersDetailingBudget).Name("Change Orders Detailing Budget").ConvertUsing(NullDecimalParser);
            Map(m => m.CurrentDetailingBudget).Name("Current Detailing Budget").ConvertUsing(NullDecimalParser);
            Map(m => m.ActualDetailingCost).Name("Actual Detailing Cost").ConvertUsing(NullDecimalParser);
            Map(m => m.DetailingBurnedRatio).Name("Detailing Burned Ratio").ConvertUsing(NullDecimalParser);




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

