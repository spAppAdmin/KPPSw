using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client;
//using GB.ListToSQLArchival.Common;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using CsvHelper;
using CsvHelper.Configuration;
//using P = ProcessHelpers;


namespace KSI_List_ReportProcessing
{
    class Program
    {

        public StringBuilder sbInsertTable = new StringBuilder();
        public StringBuilder sbInsertParm = new StringBuilder();

        static void Main(string[] args)
        {
            ProjectKPI.LoadData();
            //ITL.LoadData();
            //RFI.LoadData();


        }


    }

    }







