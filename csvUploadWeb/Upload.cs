using System;
using System.IO;
using fs = System.IO;
using System.Configuration;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Utilities;
using System.Runtime.CompilerServices;
using System.Security;
using System.Web;
using SPA = csvUploadWeb.QueryAssistants;
using SPL = csvUploadWeb.GeneralLogging;
using CsvHelper;
using CsvHelper.Configuration;
using System.Linq.Expressions;
using System.Web.UI.WebControls;
using sp = Microsoft.SharePoint.Client;
using c = csvUploadWeb;
using System.Net;
using System.Dynamic;

namespace csvUploadWeb
{
    public class Program
    {
        static void Main(string[] args)
        {
            //UploadKPICSV();
        }

        private Dictionary<string, Dictionary<string, object>> getListData(ClientContext ctx)
        {
            //Log.LogMessage("Fetching {0}{1}", ctx.Url, ListName);

            var list = ctx.Web.Lists.GetByTitle("ITL");


            // fetch the fields from this list
            FieldCollection fields = list.Fields;
            ctx.Load(fields);
            ctx.ExecuteQuery();

            // dynamically build a list of fields to get from this list
            var columns = new List<string> { "ID" }; // always include the ID field
            foreach (var f in fields)
            {
                // Log.LogMessage( "\t\t{0}: {1} of type {2}", f.Title, f.InternalName, f.FieldTypeKind );
                if (f.InternalName.StartsWith("_") || f.InternalName.StartsWith("ows")) continue;  // skip these
                if (f.FieldTypeKind == FieldType.Text) // get Text fields only... but you can get other types too by uncommenting below
                                                       // || f.FieldTypeKind == FieldType.Counter
                                                       // || f.FieldTypeKind == FieldType.User
                                                       // || f.FieldTypeKind == FieldType.Integer
                                                       // || f.FieldTypeKind == FieldType.Number
                                                       // || f.FieldTypeKind == FieldType.DateTime
                                                       // || f.FieldTypeKind == FieldType.Lookup
                                                       // || f.FieldTypeKind == FieldType.Computed
                                                       // || f.FieldTypeKind == FieldType.Boolean )
                {
                    columns.Add(f.InternalName);
                }
            }

            // build the include expression of which fields to fetch
            List<Expression<Func<sp.ListItemCollection, object>>> allIncludes = new List<Expression<Func<sp.ListItemCollection, object>>>();
            foreach (var c in columns)
            {
                // Log.LogMessage( "Fetching column {0}", c );
                allIncludes.Add(items => items.Include(item => item[c]));
            }

            // get all the items in the list with the fields
            sp.ListItemCollection listItems = list.GetItems(CamlQuery.CreateAllItemsQuery());
            ctx.Load(listItems, allIncludes.ToArray());

            ctx.ExecuteQuery();

            var sd = listItems.ToDictionary(k => k["Title"] as string, v => v.FieldValues);   // FieldValues is a Dictionary<string,object>

            // show the fields


            return sd;
        }

        public static void uploadFixle(string siteUrl, string filePath, string fileName, string docLibName)
        {
            siteUrl = siteUrl.EndsWith("/") ? siteUrl.Substring(0, siteUrl.Length - 1) : siteUrl;
            ClientContext context = new ClientContext(siteUrl);
            List docLib = context.Web.Lists.GetByTitle(docLibName);
            context.Load(docLib);
            context.ExecuteQuery();

            Byte[] bytes = System.IO.File.ReadAllBytes(filePath + fileName);

            FileCreationInformation createFile = new FileCreationInformation();

            createFile.Content = bytes;
            createFile.Url = siteUrl + "/" + docLibName + "/" + fileName;
            createFile.Overwrite = true;
            Microsoft.SharePoint.Client.File newFile = docLib.RootFolder.Files.Add(createFile);
            newFile.ListItemAllFields.Update();
            context.ExecuteQuery();
        }

        public static string[] getCSVHeaders(string csvPath)
        {
            string[] headerRow;

            using (var reader = new StreamReader(csvPath))
            {
                var csv = new CsvReader(reader);
                csv.Read();
                csv.ReadHeader();
                headerRow = csv.Context.HeaderRecord;

                dynamic expando = new ExpandoObject();
                expando.Name = "Brian";
                expando.Country = "USA";


            }
            return headerRow;
        }







    }
}


