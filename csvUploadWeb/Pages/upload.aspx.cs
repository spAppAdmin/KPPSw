using System;
using System.IO;
using fs = System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using sp = Microsoft.SharePoint.Client;
using System.Web.Hosting;
using System.Configuration;
using System.Security;
using c = csvUploadWeb;
using System.Text;
using Microsoft.SharePoint.Client.Utilities;
using System.Runtime.CompilerServices;
using SPA = csvUploadWeb.QueryAssistants;
using SPL = csvUploadWeb.GeneralLogging;
using CsvHelper;
using CsvHelper.Configuration;
using System.Reflection;
using System.Linq.Expressions;
using System.Net;
using System.Diagnostics.Tracing;                                                                                                                                                                                               
using System.Web.Script.Serialization;
using System.ComponentModel;
using CsvHelper.TypeConversion;
using cvsCnfig = CsvHelper.TypeConversion;
using NLog;
using System.Diagnostics;

namespace csvUploadWeb.Pages
{
    public partial class upload : System.Web.UI.Page
    {
        public static Stopwatch sw;

        protected void Page_PreInit(object sender, EventArgs e)
        {
            
            Uri redirectUrl;
            switch (SharePointContextProvider.CheckRedirectionStatus(Context, out redirectUrl))
            {
                case RedirectionStatus.Ok:
                    return;
                case RedirectionStatus.ShouldRedirect:
                    Response.Redirect(redirectUrl.AbsoluteUri, endResponse: true);
                    break;
                case RedirectionStatus.CanNotRedirect:
                    Response.Write("An error occurred while processing your request.");
                    Response.End();
                    break;
            }
        }

        protected void Page_Load(object sender, EventArgs e) 
        {
            //Get projects catalog list
            getProjectList();

            if (!this.IsPostBack)
            {

            }
            

        }



     

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {

                
                var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
                var ctx = spContext.CreateUserClientContextForSPHost();

                var projectURL = txtProjURL.Text;
                // var file = csvFile.Text.ToString();
                var file = "c:\\temp\\ITL.csv";

                var listName = ddTargetList.SelectedValue;
                var path = Path.Combine("@", file);
                string fileNameWoExt = Path.GetFileNameWithoutExtension(file);
                string fileNamewExt = Path.GetFileName(file);
                string filepath = Path.GetFullPath(file);
                string csvPath = file;
                Uri projSiteUrl = new Uri(projectURL);

                var lookup = "CostCodeList";
                string lookupFieldName = "AreaName_x002b_SubTask";
                string lookupFieldType = "Calculated";

                var action = rbAction.SelectedValue;
                Int32 recordCount = 0;

                switch (action)
                {

                    case "AddNew":// This will add an entry if it doesn't exist
                        recordCount = ActionAddNewListItems(csvPath, projSiteUrl, action, listName, fileNamewExt, lookup, lookupFieldName, lookupFieldType);
                        clientMessage(this.Page, action + " Completed" + recordCount + " Loaded");
                        break;
                    case "AddAll"://This will force the creation of a brand new entry even if it already exists.
                        recordCount = ActionAddAllListItems(csvPath, projSiteUrl, action, listName, fileNamewExt, lookup, lookupFieldName, lookupFieldType);
                        break;
                    case "Update":// This will add an entry if it doesn't exist, but will only update if it does exist.
                        recordCount = ActionUpdateListItems(csvPath, projSiteUrl, action, listName, fileNamewExt, lookup, lookupFieldName, lookupFieldType);
                        break;
                    case "Delete":// // This will delete 1 or MANY entries in the list
                        ActionDeleteCSVListItems(listName, projSiteUrl, csvPath);
                        recordCount = ActionDeleteCSVListItems(listName, projSiteUrl, csvPath);
                        break;
                    case "DeleteAll"://Caution - this will delete all records in list
                        recordCount = ActionDeleteAllListItems(listName, projSiteUrl);

                        break;

                }


                clientMessage(this.Page, action + " Completed" + recordCount + " Loaded");

            }

            catch (Exception ex)
            {
                
                HandleException(this.Page, ex);
                SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source);

                //var n = new NLog.

            }
        }
      

        private static void KillExistingProcesses()
        {
            Process[] p = Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName);
            foreach (var pro in p)
            {
                pro.Kill();
            }
        }


        protected void KillProcess(object sender, EventArgs e)
        {
            KillExistingProcesses();
            


        }


        public static void HandleException(Page page, Exception ex)
        {
            var msg = new JavaScriptSerializer().Serialize(ex.Message.ToString());
            var data =  new JavaScriptSerializer().Serialize(ex.Data.ToString());
            var trace =  new JavaScriptSerializer().Serialize(ex.ToDetailedString());

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(msg);
            sb.AppendLine(data);
            sb.AppendLine(trace);
            var message = sb.ToString();

            var script = string.Format("alert({0});", msg);
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "DisplaysMsg", script, true);


        }

        public static void clientMessage(Page page, string msg)
        {
            var message = new JavaScriptSerializer().Serialize(msg.ToString());
            var script = string.Format("alert({0});", message);
            ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "DisplayMsg", script, true);

        }

        public static Int32 ActionDeleteAllListItems(string listName, Uri projSiteURI) {
            Int32 rtnRecord = 0;
            ClientContext ctx = getProjectSpCtx(projSiteURI);
            if (ctx != null)
            {
                List spList = ctx.Web.Lists.GetByTitle(listName);
                sp.ListItemCollection listItems = spList.GetItems(CamlQuery.CreateAllItemsQuery());
                ctx.Load(listItems, eachItem => eachItem.Include(item => item, item => item["ID"]));
                ctx.ExecuteQuery();
                var totalListItems = listItems.Count;
                rtnRecord = listItems.Count;

                if (totalListItems > 0)
                {
                    for (var counter = totalListItems - 1; counter > -1; counter--)
                    {
                        listItems[counter].DeleteObject();
                        ctx.ExecuteQuery();
                        //rtnRecord = counter;
                    }
                }
            }
            return rtnRecord;
        }

        public static Int32 ActionDeleteCSVListItems(string listName, Uri projSiteURI, string csvPath)
        {
            try
            {
                ClientContext ctx = getProjectSpCtx(projSiteURI);
                Int32 recordCount = 0;
                if (ctx != null)
                {
                    List<ITLRecord> records = GetRecordsFromITLCsv(csvPath);
                    List spList = ctx.Web.Lists.GetByTitle(listName);
                    foreach (ITLRecord record in records)
                    {
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        sp.ListItemCollection existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();

                        var totalListItems = existingMappings.Count;

                        if (totalListItems > 0)
                        {
                            for (var counter = totalListItems - 1; counter > -1; counter--)
                            {
                                //Delete record identified by CSV file so new one can be added
                                existingMappings[counter].DeleteObject();
                                ctx.ExecuteQuery();
                                recordCount = counter;
                            }
                        }
                    }
                }
                return recordCount;
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }
        }

        public static Int32 ActionUpdateListItems(string csvPath, Uri projSiteURI, string action, string listName, string fileName, string lookup, string lookupFieldName, string lookupFieldType)
        {
            try
            {
                ClientContext ctx = getProjectSpCtx(projSiteURI);
                Int32 recordCount = 0;
                if (ctx != null)
                {
                    List<ITLRecord> records = GetRecordsFromITLCsv(csvPath);
                    List spList = ctx.Web.Lists.GetByTitle(listName);

                    foreach (ITLRecord record in records)
                    {
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        sp.ListItemCollection existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();
                        var totalListItems = existingMappings.Count;

                        if (totalListItems > 0)
                        {
                            for (var counter = totalListItems - 1; counter > -1; counter--)
                            {
                                //Delete record identified by CSV file so new one can be added
                                existingMappings[counter].DeleteObject();
                                ctx.ExecuteQuery();
                            }
                            AddNewListItem(record, spList, ctx, lookup, lookupFieldName, lookupFieldType);
                        }
                        recordCount = totalListItems;
                    }
                }
                return recordCount;

            } catch (Exception ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }
        }

        public static Int32 ActionAddNewListItems(string csvPath, Uri projSiteURI, string action, string listName, string fileName, string lookup, string lookupFieldName, string lookupFieldType)
        {
            try
            {
                ClientContext ctx = getProjectSpCtx(projSiteURI);

                Int32 recordCount = 0;

                if (ctx != null)
                {

                    List<ITLRecord> records = GetRecordsFromITLCsv(csvPath);
                    List spList = ctx.Web.Lists.GetByTitle(listName);


                    //Checks to see if an item already exists with the same title and preserves

                    foreach (ITLRecord record in records)
                    {
                        recordCount++;

                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        var existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();

                        //recordCount = existingMappings.Count;

                        switch (existingMappings.Count)
                        {
                            case 0:
                                //No records found, needs to be added
                                AddNewListItem(record, spList, ctx, lookup, lookupFieldName, lookupFieldType);
                                break;
                            default:
                                //Existing record found - ignore and continue with next item
                                continue;
                        }
                    }

                }
                return recordCount;
            }
            catch (Exception ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

        }

        public static Int32 ActionAddAllListItems(string csvPath, Uri projSiteURI, string action, string listName, string fileName, string lookup, string lookupFieldName, string lookupFieldType)
        {
            try
            {
                ClientContext ctx = getProjectSpCtx(projSiteURI);

                Int32 recordCount = 0;
                int i = 0;

                if (ctx != null)
                {
                    List<ITLRecord> records = GetRecordsFromITLCsv(csvPath);
                    List spList = ctx.Web.Lists.GetByTitle(listName);
                    recordCount = records.Count;

                    //Checks to see if an item already exists with the same title and preserves

                    foreach (ITLRecord record in records)
                    {
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        var existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();
                        AddNewListItem(record, spList, ctx, lookup, lookupFieldName, lookupFieldType);
                    }
                }
                return recordCount;
            }

            catch (IndexOutOfRangeException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

            catch (ArgumentOutOfRangeException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;

            }

            catch (InvalidOperationException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }


            catch (ArgumentNullException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               


            catch (Exception ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }
        }

        //public void NullException(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}


       public static void addOrUpdate(Dictionary<int, int> dic, int key, int newValue)
        {
            int val;
            if (dic.TryGetValue(key, out val))
            {
                // yay, value exists!
                dic[key] = val + newValue;
            }
            else
            {
                // darn, lets add the value
                dic.Add(key, newValue);
            }
        }

        public static void AddNewListItem(ITLRecord record, List spList, ClientContext clientContext, string LookupList, string lookupFieldName, string lookupFieldType)
        {
            try
            {
                Int32 recordCount = 0;
                Dictionary<string, object> itemFieldValues = new Dictionary<string, object>();
                PropertyInfo[] properties = typeof(ITLRecord).GetProperties();
                
                foreach (PropertyInfo property in properties)
                {
                    object propValue = property.GetValue(record, null);

                    if (String.IsNullOrEmpty(propValue.ToString()))
                    {
                        //throw new Exception("Error with " + property.Name +"-"+ propValue.ToString());
                    }


                    if (!String.IsNullOrEmpty(propValue.ToString()))
                    {
                        Field matchingField = spList.Fields.GetByInternalNameOrTitle(property.Name);
                        clientContext.Load(matchingField);
                        clientContext.ExecuteQuery();

                        //itemFieldValues.Values.sec

                        switch (matchingField.FieldTypeKind)
                        {
        
                            case FieldType.User:
                                FieldUserValue userFieldValue = GetUserFieldValue(propValue.ToString(), clientContext);
                                if (userFieldValue != null)
                                    itemFieldValues.Add(matchingField.InternalName, userFieldValue);
                                else
                                    throw new Exception("User field value could not be added: " + propValue.ToString());
                                break;

                            case FieldType.Lookup:

                                if (property.Name == "Predecessors")
                                {
                                    LookupList = "ITL";
                                    lookupFieldType = "string";
                                    lookupFieldName = "Title";
                                }

                                
                                FieldLookupValue lookupFieldValue = GetLookupFieldValue(propValue.ToString(), LookupList, lookupFieldName, lookupFieldType, clientContext);
                                if (lookupFieldValue != null)
                                    itemFieldValues.Add(matchingField.InternalName, lookupFieldValue);
                                else
                                    throw new Exception("Lookup field value could not be added: " + propValue.ToString());
                                break;
                            case FieldType.Invalid:
                                switch (matchingField.TypeAsString)
                                {
                                    default:
                                        //Code for publishing site columns
                                        continue;
                                }
                            default:
                                itemFieldValues.Add(matchingField.InternalName, propValue);
                                recordCount = itemFieldValues.Count;
                                
                                break;
                        }
                    }
                }
                //Add new item to list



                ListItemCreationInformation creationInfo = new ListItemCreationInformation();
                sp.ListItem oListItem = spList.AddItem(creationInfo);
                
           
                var d = new DateTime(2000, 1, 1);

                foreach (KeyValuePair<string, object> itemFieldValue in itemFieldValues)
                {

                    if (itemFieldValue.Value.ToString()  ==  "1/1/2000 12:00:00 AM")
    
                    {
                            //Set each field value
                            oListItem[itemFieldValue.Key] = null;
                        }
                    else
                    {
                        oListItem[itemFieldValue.Key] = itemFieldValue.Value;
                    }
                }
                oListItem.Update();
                clientContext.ExecuteQuery();
                
            }

            
            catch (IndexOutOfRangeException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;

            }


            catch (InvalidOperationException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

            catch (ArgumentNullException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }


            catch (NullReferenceException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

            catch (CsvHelperException ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

            catch (Exception ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;

            }


        }

        public static class ForEachHelper
        {
            public sealed class Item<T>
            {
                public int Index { get; set; }
                public T Value { get; set; }
                public bool IsLast { get; set; }
            }

            public static IEnumerable<Item<T>> WithIndex<T>(IEnumerable<T> enumerable)
            {
                Item<T> item = null;
                foreach (T value in enumerable)
                {
                    Item<T> next = new Item<T>();
                    next.Index = 0;
                    next.Value = value;
                    next.IsLast = false;
                    if (item != null)
                    {
                        next.Index = item.Index + 1;
                        yield return item;
                    }
                    item = next;
                }
                if (item != null)
                {
                    item.IsLast = true;
                    yield return item;
                }
            }
        }


        //public IEnumerable<ITLRecord>
        //{

        public static List<ITLRecord> GetRecordsFromITLCsv(string csvPath)
        {
            try
            {
                List<ITLRecord> records = new List<ITLRecord>();
                using (var sr = new StreamReader(csvPath))
                {
                    var parser = new CsvReader(sr, new CsvHelper.Configuration.Configuration
                    {
                        HasHeaderRecord = true,
                        HeaderValidated = null,
                        //MissingFieldFound = " ",
                        //UseNewObjectForNullReferenceMembers = true,
                        //TrimOptions = TrimOptions.Trim,
                        //ShouldSkipRecord = true;
                        //IgnoreBlankLines = false


                        //  ReadingExceptionOccurred = (ex, row) => errors.Add(string.Format("Line[{0}]: {1}", row.Row, ex.Message))
                    });

                    using (parser) {

                        parser.Configuration.RegisterClassMap<ProjectITLMap>();
                        //parser.Configuration.TypeConverterOptionsCache.GetOptions<string>().NullValues.Add("");
                        //parser.Configuration.TypeConverterOptionsCache.GetOptions<DateTime?>().NullValues.Add("null");
                        //parser.Configuration.TypeConverterOptionsCache.GetOptions<DateTime?>().NullValues.Add("");
                        //parser.Configuration.MissingFieldFound = (headerNames, index, context) =>
                        // {
                        //SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source);.WriteLine($"Field with names ['{string.Join("', '", headerNames)}'] at index '{index}' was not found. );
                        //};

                        parser.Configuration.ReadingExceptionOccurred = ReadingExceptionOccurred;
                        //parser.Configuration.MissingFieldFound( = ReadingExceptionOccurred;

                        
                        //NullableConverter(typeof(string)).ConvertFromString(new TypeConverterOptions(), "NA")
                        records = parser.GetRecords<ITLRecord>().ToList();
                    }
                }

                return records;
            }

            catch (CsvHelperException ex)
            {
             ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

            catch (Exception ex)
            {
                ex.Data.Add("UserMessage", ex.InnerException.ToString() + "," + ex.StackTrace.ToString());
                throw;
            }

        }

        private static bool ReadingExceptionOccurred(CsvHelperException arg)
        {
            throw new NotImplementedException();

        }

        public static FieldUserValue GetUserFieldValue(string userName, ClientContext clientContext)
        {
            //Returns first principal match based on user identifier (display name, email, etc.)
            ClientResult<PrincipalInfo> principalInfo = Utility.ResolvePrincipal(
                clientContext, //context
                clientContext.Web, //web
                userName, //input
                PrincipalType.User, //scopes
                PrincipalSource.All, //sources
                null, //usersContainer
                false); //inputIsEmailOnly
            clientContext.ExecuteQuery();
            PrincipalInfo person = principalInfo.Value;

            if (person != null)
            {
                //Get User field from login name
                User validatedUser = clientContext.Web.EnsureUser(person.LoginName);
                clientContext.Load(validatedUser);
                clientContext.ExecuteQuery();

                if (validatedUser != null && validatedUser.Id > 0)
                {
                    //Sets lookup ID for user field to the appropriate user ID
                    FieldUserValue userFieldValue = new FieldUserValue();
                    userFieldValue.LookupId = validatedUser.Id;
                    return userFieldValue;
                }
            }
            return null;
        }

        //   public static string GetDateFieldValue(string dateValue, ClientContext clientContext)
        //   {
        //       if (dateValue == "1/1/2000")
        //       {
        //           return null;
        //}
        //       else
        //       {
        //           return dateValue;
        //}
        //}


        public static FieldLookupValue GetLookupFieldValue(string lookupName, string lookupListName, string lookupFieldName, string lookupFieldType, ClientContext clientContext)
        {
            //Ref:https://karinebosch.wordpress.com/2015/05/11/setting-the-value-of-a-lookup-field-using-csom/
            var lookupList = clientContext.Web.Lists.GetByTitle(lookupListName);
            CamlQuery query = new CamlQuery();


            query.ViewXml = string.Format(@"<View><Query><Where><Eq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Eq>" +
                                            "</Where></Query></View>", lookupFieldName, lookupFieldType, lookupName);

            sp.ListItemCollection listItems = lookupList.GetItems(query);
            clientContext.Load(listItems, items => items.Include
                                                (listItem => listItem["ID"],
                                                listItem => listItem[lookupFieldName]));
            clientContext.ExecuteQuery();

            if (listItems != null)
            {
                sp.ListItem item = listItems[0];
                FieldLookupValue lookupValue = new FieldLookupValue();
                lookupValue.LookupId = int.Parse(item["ID"].ToString());
                return lookupValue;
            }
            return null;
        }

        public static void createCSVLoaderList()
        {

            Uri ProjUri = new Uri("https://kineticsys.sharepoint.com/sites/projects");
            ListCreationInformation listInfo = new ListCreationInformation();
            listInfo.Title = "CSV Uploader";
            ClientContext ctx = getProjectSpCtx(ProjUri);

            ListTemplate listTemplate = ctx.Site.GetCustomListTemplates(ctx.Site.RootWeb).GetByName("CSV Uploader");
            ctx.Load(listTemplate, tL => tL.Name, tL => tL.FeatureId, tL => tL.ListTemplateTypeKind);
            ctx.ExecuteQuery();

            listInfo.TemplateFeatureId = listTemplate.FeatureId;
            listInfo.TemplateType = listTemplate.ListTemplateTypeKind;
            //web.Lists.Add(listInfo);
            ctx.ExecuteQuery();
        }

        public void getProjectList()
        {
            //ADD-IN CONTEXT
            string url = "https://kineticsys.sharepoint.com/sites/projects";
            var uri = new Uri(url);
            var accessToken = TokenHelper.GetAppOnlyAccessToken(TokenHelper.SharePointPrincipal, uri.Authority, TokenHelper.GetRealmFromTargetUrl(uri));
            //var ctx = TokenHelper.GetClientContextWithAccessToken(uri.ToString(), accessToken.AccessToken);

            //var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            //var ctx = spContext.CreateUserClientContextForSPHost();

            var ctx = getProjectSpCtx(uri);
            Web web = ctx.Web;

            List list = ctx.Web.Lists.GetByTitle("KPPS Projects Catalog");
            var q = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Active</Value></Eq></Where></Query><ViewFields><FieldRef Name='Project_x0020_Name' /><FieldRef Name='Proj_x0020_Site_x0020_URL' /></ViewFields><QueryOptions /></View>" };
            sp.ListItemCollection li = list.GetItems(q);
            ctx.Load(li);
            ctx.ExecuteQuery();

            foreach (var item in li)
            {
                var hyp = (FieldUrlValue)item["Proj_x0020_Site_x0020_URL"];
                string URL = string.Format("{0}", item.FieldValues["Proj_x0020_Site_x0020_URL"]);
                string text = string.Format("{0}", item.FieldValues["Project_x0020_Name"]);
                ddlProjName.Items.Add(new System.Web.UI.WebControls.ListItem(hyp.Description, hyp.Url));
            }
            // ddlProjName.SelectedItem.Text= "KPPS Project Template";
            // txtProjURL.Text = "000000000 KPPS Project Template";

        }

        public static ClientContext getProjectSpCtx(Uri UriProject)
        {
            Uri uriProject = UriProject;
            ClientContext ctx = new ClientContext(UriProject);

            string accountName = ConfigurationManager.AppSettings["AccountName"];
            char[] pwdChars = ConfigurationManager.AppSettings["AccountPwd"].ToCharArray();
            SecureString accountPwd = new SecureString();
            for (int i = 0; i < (int)pwdChars.Length; i++)
            {
                accountPwd.AppendChar(pwdChars[i]);
            }

            ctx.Credentials = new SharePointOnlineCredentials(accountName, accountPwd);

            return ctx;
        }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        [Serializable()]
        public class InvalidDepartmentException : System.Exception
        {
            public InvalidDepartmentException() : base() { }
            public InvalidDepartmentException(string message) : base(message) { }
            public InvalidDepartmentException(string message, System.Exception inner) : base(message, inner) { }

            // A constructor is needed for serialization when an
            // exception propagates from a remoting server to the client. 
            protected InvalidDepartmentException(System.Runtime.Serialization.SerializationInfo info,
                System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

        public class LogWriter
        {
            private string m_exePath = string.Empty;

            public LogWriter(string logMessage)
            {
                this.LogWrite(logMessage);
            }

            public void Log(string logMessage, TextWriter txtWriter)
            {
                try
                {
                    txtWriter.Write("\r\nLog Entry : ");
                    txtWriter.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                    txtWriter.WriteLine("  :");
                    txtWriter.WriteLine("  :{0}", logMessage);
                    txtWriter.WriteLine("-------------------------------");
                }
                catch (Exception exception)
                {
                }


            }


            public void LogWrite(string logMessage)
            {
                this.m_exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                try
                {
                    using (StreamWriter streamWriter = fs.File.AppendText(string.Concat(this.m_exePath, "\\log.txt")))
                    {
                        this.Log(logMessage, streamWriter);
                    }

                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}
        