using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client.Taxonomy;
using Microsoft.SharePoint.Client;
//using GB.ListToSQLArchival.Common;
using System.IO;


namespace KSI_List_ReportProcessing
{
    class Program
    {
        static void Main(string[] args)
        {

            //string SiteUrl = args[0];

            string SiteUrl = "https://kineticsys.sharepoint.com/sites/projects/construction/SPP/000000000";

            Environment.SetEnvironmentVariable("siteUrl", SiteUrl, EnvironmentVariableTarget.Process);
            Dictionary<string, string> SuccessEmailDic = new Dictionary<string, string>();
            Dictionary<string, string> FailureEmailDic = new Dictionary<string, string>();
            bool successflag = true;
            try
            {
                SharePointContextWrapper scwrapper = new SharePointContextWrapper(siteUrl: SiteUrl);
                //Get all the items from Config List
                var configitems = scwrapper.GetListItems(listname: Constants.CONFIGLIST, query: new Microsoft.SharePoint.Client.CamlQuery());
                // get send mail information from EmailConfig List
                var emailItems = scwrapper.GetListItems(listname: Constants.EMAILSETTINGLIST, query: new Microsoft.SharePoint.Client.CamlQuery());

                //BuildEmailDictionary(SuccessEmailDic, FailureEmailDic, emailItems);

                foreach (var item in configitems)
                {
                    try
                    {
                        if (item[Constants.MODE].ToString() == Mode.Offline.ToString())
                        {
                            Console.WriteLine("Skkiping Item ID {0} . This is Selected as Offline.", item["ID"]);
                            continue;
                        }
                        FailureEmailDic[Constants.TO] = item[Constants.SUPPORTTEAMEMAIL].ToString();
                        var surl = item[Constants.SITEURL].ToString().Trim();
                        var vname = item[Constants.VIEWNAME].ToString().Trim();
                        var lname = item[Constants.LISTNAME].ToString().Trim();
                        var dbname = item[Constants.DBNAME].ToString().Trim();
                        var dbuserID = item[Constants.DBUSERID].ToString().Trim();
                        var dbpsw = item[Constants.DBUSERPSW].ToString().Trim();
                        Console.WriteLine("Operation Started on :" + surl);
                        //Build Database connection string by fetching information from list item.
                        Constants.sqlconstr = string.Format(Constants.sqlconstr, dbname, dbuserID, dbpsw);

                        string DBInstance = item["Database_x0020_Name"].ToString();
                        string TrackingDBInstance = item["Tracking_x0020_Database_x0020_Na"].ToString();
                        var tablecreatedflag = item[Constants.TABLECREATEDFLAG].ToString() == "No";

                        SqlArchiver sqlArchiver = new SqlArchiver(url: surl, listName: lname, viewName: vname);
                        if (tablecreatedflag)
                        {
                            Console.WriteLine("Generating Create Table Script.");
                            var path = sqlArchiver.GenerateTableCreateScript(item);


                        }
                        else
                        {
                            var insertQuery = item[Constants.INSERTQUERY];
                            string attachmentLocation = string.Empty;
                            if (item["Attachment_x0020_Location"] != null)
                            {
                                attachmentLocation = item["Attachment_x0020_Location"].ToString();
                                string path = Path.Combine(attachmentLocation, lname);
                                System.IO.Directory.CreateDirectory(path);
                                Environment.SetEnvironmentVariable("AttachmentPath", path, EnvironmentVariableTarget.Process);
                            }

                            //Check if insertquery is already generated in previous run.
                            if (insertQuery == null)
                            {
                                var tablename = item[Constants.TABLENAME];
                                Console.WriteLine("Generating Insert Query for the first time.");
                                var insertquery = sqlArchiver.GetInsertCommand(TableName: tablename.ToString().Trim());
                                item[Constants.INSERTQUERY] = insertquery;
                                item.Update();
                                item.Context.ExecuteQuery();
                                Console.WriteLine("Insert Query Generated");
                                insertQuery = insertquery;
                            }
                            Console.WriteLine("Starting Archival Job on Item ID: " + item.Id);
                            //Call the archive method to Dump data to SQL and Delete Listitems from View.
                            sqlArchiver.ArchiveAndDelete(insertcmd: insertQuery.ToString());
                            Console.WriteLine("Archival Job Completed for ItemID : " + item.Id);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            string msg = successflag == true ? "Operation Completed Successfully.!!!!" : "Operation Completed with Exception..!!!!";
            Console.WriteLine(msg);
            Environment.SetEnvironmentVariable("siteUrl", null, EnvironmentVariableTarget.Process);
        }


        public void LoadListItems()
        {

            /*      var projSiteURL = "https://kineticsys.sharepoint.com/sites/projects";
                  //ClientContext ctx = P.QueryAssistants.getProjectSpCtx(new Uri(projSiteURL));

                  _context = new ClientContext(projSiteURL);


                  List oList = _context.Web.Lists.GetByTitle("KPPS Projects Catalog");
                  CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Active</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL' /></ViewFields><QueryOptions /></View>" };
                  ListItemCollection items = oList.GetItems(camlQuery);
                  _context.Load(items);
                  _context.ExecuteQuery();
                  var n = items.Count;

                  foreach (ListItem item in items)
                  {
                      //get each projects SP context

                      var hyperLink = (FieldUrlValue)item["Proj_x0020_Site_x0020_URL"];
                      string url = hyperLink.Url;
                      //ClientContext ctxProj = P.QueryAssistants.getProjectSpCtx(new Uri(url));
                      _context = new ClientContext(url);

                      //get list fields

                      List list = _context.Web.Lists.GetByTitle("ITL");
                      StringBuilder script = new StringBuilder();
                      var listFieldCollection = list.Fields;
                      _context.Load(list);
                      _context.Load(listFieldCollection);
                      _context.ExecuteQuery();

                      //Create insert SQL

                      StringBuilder sb = GenerateInsertQuery(list, "default");
                      //INSERT INTO table_name(column1, column2, column3, ...) VALUES(value1, value2, value3, ...);

                  }
                  */
        }

    }


}

