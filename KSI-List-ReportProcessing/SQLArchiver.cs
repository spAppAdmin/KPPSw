using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Microsoft.SharePoint.Client;
using System.Collections.Generic;
//using GB.ListToSQLArchival.Common;


namespace KSI_List_ReportProcessing
{
    /// <summary>
    /// Provide Methods for Archiving and Generating DDL Script.
    /// </summary>
    class SqlArchiver
    {
        #region Private Fields
        private string ViewName = string.Empty;
        private string ListName = string.Empty;
        private SharePointContextWrapper scwrapper;
        #endregion

        #region Public Constructor
        /// <summary>
        /// Initialize the Utility Class
        /// </summary>
        /// <param name="url">SharePoint site URL</param>
        /// <param name="listName">Name of the List to be Archive</param>
        /// <param name="viewName">List View Name</param>
        public SqlArchiver(string url, string listName, string viewName)
        {
            scwrapper = new SharePointContextWrapper(url);
            ViewName = viewName;
            ListName = listName;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Generate create table script and output it to local folder
        /// </summary>
        public string GenerateTableCreateScript(ListItem item)
        {
            StringBuilder createTableScript = scwrapper.ReadListSchema(scwrapper.GetListByName(scwrapper.GetWeb(), ListName), ListName, ViewName, item);
            string DDLoutputLocation = string.Empty;
            if (item["DDL_x0020_Output_x0020_Location"] != null)
            {
                DDLoutputLocation = item["DDL_x0020_Output_x0020_Location"].ToString();
                var p = System.IO.Path.Combine(DDLoutputLocation, ListName);
                System.IO.Directory.CreateDirectory(p);
                Environment.SetEnvironmentVariable("DDLOutputPath", p, EnvironmentVariableTarget.Process);
            }
            var path = string.Format("{0}\\{1}_{2}.sql", Environment.GetEnvironmentVariable("DDLOutputPath"), ListName, ViewName);
            System.IO.File.WriteAllText(path, createTableScript.ToString());

            //generating DDL script for tracking Table.

            //System.IO.File.WriteAllText(string.Format("{0}\\{1}_{2}.sql", Environment.GetEnvironmentVariable("DDLOutputPath"), ListName, "TrackingTable"), string.Format(Constants.ARCHIVALTRACKINGCREATESCRIPT, ListName));
            return path;
        }

        /// <summary>
        /// build the insert command based on the view schema.
        /// </summary>
        /// <returns>Insert Query</returns>
        public string GetInsertCommand(string TableName)
        {
            scwrapper.GenerateInsertQuery(scwrapper.GetListByName(scwrapper.GetWeb(), ListName), ViewName);
            return string.Format(Constants.InsertCMD, TableName, scwrapper.sbInsertTable, scwrapper.sbInsertParm);
        }

        /// <summary>
        /// Archive the ListItems to SQL based on the insert command and then delete ListItems from the List.
        /// </summary>
        /// <param name="insertcmd">insert query to dump items to SQL</param>
        public void ArchiveAndDelete(string insertcmd)
        {
            var list = scwrapper.GetListByName(scwrapper.GetWeb(), ListName);
            var _context = list.Context as ClientContext;
            _context.RequestTimeout = System.Threading.Timeout.Infinite;
            var view = list.Views.GetByTitle(ViewName);
            _context.Load(view);
            _context.ExecuteQuery();
            ListItemCollectionPosition itemCreateInfo = null;
            //Get Batch items based on rowlimit 
            string viewquery = "<View><Query>" + view.ViewQuery + "</Query><RowLimit>" + Constants.ROWLIMIT + "</RowLimit></View>";
            CamlQuery query = new CamlQuery() { ViewXml = viewquery };
            //Get the View Fields from List
            var lc = scwrapper.GetListColumns(list);
            do
            {
                query.ListItemCollectionPosition = itemCreateInfo;
                var items = list.GetItems(query);
                _context.Load(items);
                _context.ExecuteQuery();
                ArchiveItems(items, insertcmd, lc);
                itemCreateInfo = items.ListItemCollectionPosition;

            } while (itemCreateInfo != null);



        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Archive the batch of List items to SQL table and delete it from List.
        /// </summary>
        /// <param name="items">item collection to be archived</param>
        /// <param name="insertcmd">insert command </param>
        /// <param name="vc">list of fields to get item metadata and insert it to DB</param>
        public void ArchiveItems(ListItemCollection items, string insertcmd, List<string> vc)
        {

            DBContextWrapper dbcontext = new DBContextWrapper();
            DBContextWrapper trackingDBContext = new DBContextWrapper(ConfigurationManager.AppSettings["TrackingDBConnection"]);
            ListName = ListName.Replace("'", "");
            ViewName = ViewName.Replace("'", "");
            foreach (var item in items.ToList())
            {
                int itemid = item.Id;
                string itemname = string.Empty;
                if (item["Title"] != null)
                {
                    itemname = item["Title"].ToString();
                }
                itemname = itemname.Replace("'", "");
                try
                {
                    //Build Insert command
                    var insertVal = scwrapper.GetInsertValue(item: item, viewColumns: vc, itemId: itemid, listName: ListName);
                    var insertCommand = new StringBuilder(insertcmd.Substring(0, insertcmd.IndexOf("VALUES")));
                    insertCommand.Append("VALUES (" + insertVal + ")");

                    // send insert command to database.
                    int success = 0;
                    try
                    {
                        success = dbcontext.InsertData(insertCommand.ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    if (success == 1)
                    {
                        // item was inserted successfully. log it to tracking db.

                        Console.WriteLine("Item ID: {0} Name: {1} Archived to SQL Table Successfully.", itemid, itemname);
                        try
                        {
                            item.DeleteObject();
                            items.Context.ExecuteQuery();
                            // item was deleted from List. log it to tracking DB.
                            Console.WriteLine("Item ID: {0} Name: {1} Deleted from List", itemid, itemname);



                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion



    }
}
