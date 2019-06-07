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

namespace csvUploadWeb.Pages
{
    public partial class upload : System.Web.UI.Page
    {


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
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Default), "BasePageScript", script, true);
            getProjectList();

        }

        protected void Button1_Click1(object sender, EventArgs e)
        {                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
            // Deploy to library

            if (csvFile.Value == "")
            {
                msg.Text = "File Required";
                return;
            }

            var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            var ctx = spContext.CreateUserClientContextForSPHost();

            var projectURL = txtProjURL.Text;
            var file = csvFile.Value.ToString();
            var listName = ddTargetList.SelectedValue;
             var lib = "CSV Uploads";

            var path = Path.Combine("@", file);
            string fileNameWoExt = Path.GetFileNameWithoutExtension(file);
            string fileNamewExt = Path.GetFileName(file);
            string filepath = Path.GetFullPath(file);
            string fileExt = Path.GetExtension(file);

            //List list = ctx.Web.Lists.GetByTitle(lib);

            //var q = new CamlQuery();
            //q.ViewXml = string.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query><ViewFields><FieldRef FileRef/></ViewFields><QueryOptions /></View>", fileNameWoExt);
            //sp.ListItemCollection listItems = list.GetItems(q);
            //ctx.Load(listItems, items => items.Include (listItem => listItem["ID"], listItem => listItem["FileRef"]));
            //ctx.ExecuteQuery();
            //if (listItems != null){
            //sp.ListItem item = listItems[0];
            //}

            //string absoluteUrl ="";

            //foreach (sp.ListItem item in listItems) {
            //var serverRelativeUrl = item["FileRef"];
            //absoluteUrl = new Uri(ctx.Url).GetLeftPart(UriPartial.Authority) + serverRelativeUrl;
            //}

            //  var q = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title' /><Value Type='Text'>ITL</Value></Eq></Where></Query><ViewFields><FieldRef Name='Project' /><FieldRef Name='Project' /><FieldRef FileRef/><FieldRef Name='Title' /></ViewFields><QueryOptions /></View>" };
            //string csvPath = @"\\kineticsys.sharepoint.com\sites\projects\construction\SPP\000000000\CSV Uploads\ITL.csv";
            try {
                string csvPath = @"C:\temp\ITL.csv";
                Uri projSiteUrl = new Uri(projectURL);
                var tList = listName;
                var lookup = "CostCodeList";
                var append = "no";
                string lookupFieldName = "AreaName_x002b_SubTask";
                string lookupFieldType = "Calculated";

                UploadITLCSV(csvPath, projSiteUrl, append, tList, fileNamewExt, lookup, lookupFieldName, lookupFieldType);

                //string[] hdr = getCSVHeaders(csvPath);
                //ClientContext ct = getProjectSpCtx(projSiteUrl);
                //var dic = getListData(ct);


                //var dic = hdr.ToDictionary(Item=>Item, GetByInternalNameOrTitle())
                //Dictionary<string, object> itemFieldValues = new Dictionary<string, object>();
                // Field matchingField = spList.Fields.GetByInternalNameOrTitle(property.Name);
                //        clientContext.Load(matchingField);
                //        clientContext.ExecuteQuery();

                //Field matchingField = spList.Fields.GetByInternalNameOrTitle(property.Name);


                //System.Threading.Thread.Sleep(9000);
            } catch (Exception ex) { 
            SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source);
            }



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

        public static void UploadITLCSV(string csvPath, Uri projSiteURI, string append, string listName, string fileName, string lookup, string lookupFieldName, string lookupFieldType)
        {
            try
            {
                ClientContext ctx = getProjectSpCtx(projSiteURI);
                if (ctx != null)
                {
                    List<ITLRecord> records = GetRecordsFromITLCsv(csvPath);
                    List spList = ctx.Web.Lists.GetByTitle(listName);

                    //if (append == "no")
                    //{
                    DeleteListItem(spList, ctx);
                    //}

                    foreach (ITLRecord record in records)
                    {
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        var existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();

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
            }
            catch (Exception ex)
            {
                throw ex;
                
           }
            finally
            {
                Console.WriteLine("Control flow reaches finally");
                Console.ReadLine();



            }
        }
   

        public static void DeleteListItem(List spList, ClientContext ctx)
        {

            sp.ListItemCollection listItems = spList.GetItems(CamlQuery.CreateAllItemsQuery());
            ctx.Load(listItems, eachItem => eachItem.Include(item => item, item => item["ID"]));
            ctx.ExecuteQuery();
            var totalListItems = listItems.Count;
            if (totalListItems > 0)
            {
                for (var counter = totalListItems - 1; counter > -1; counter--)
                {
                    listItems[counter].DeleteObject();
                    ctx.ExecuteQuery();
                    Console.WriteLine("Row: " + counter + " Item Deleted");
                }
            }
        }

        public static void AddNewListItem(ITLRecord record, List spList, ClientContext clientContext, string LookupList, string lookupFieldName, string lookupFieldType)
        {
            try
            {
                Dictionary<string, object> itemFieldValues = new Dictionary<string, object>();
                PropertyInfo[] properties = typeof(ITLRecord).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    object propValue = property.GetValue(record, null);
                    if (!String.IsNullOrEmpty(propValue.ToString()))
                    {
                        Field matchingField = spList.Fields.GetByInternalNameOrTitle(property.Name);
                        clientContext.Load(matchingField);
                        clientContext.ExecuteQuery();

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
                                break;
                        }
                    }
                }

                //Add new item to list
                ListItemCreationInformation creationInfo = new ListItemCreationInformation();
                sp.ListItem oListItem = spList.AddItem(creationInfo);

                foreach (KeyValuePair<string, object> itemFieldValue in itemFieldValues)
                {
                    //Set each field value
                    oListItem[itemFieldValue.Key] = itemFieldValue.Value;
                }
                //Persist changes
                oListItem.Update();
                clientContext.ExecuteQuery();

            }
            catch (Exception ex)
            {
                throw ex;
                //Trace.TraceError("Failed: " + ex.Message);
                //Trace.TraceError("Stack Trace: " + ex.StackTrace);
                SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source);
            }
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
            }
            return headerRow;
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


        public static List<ITLRecord> GetRecordsFromITLCsv(string csvPath)
        {
            List<ITLRecord> records = new List<ITLRecord>();
            using (var sr = new StreamReader(csvPath))
            {
                using (var csvReader = new CsvReader(sr))
                {
                    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                    csvReader.Configuration.RegisterClassMap<ProjectITLMap>();
                    records = csvReader.GetRecords<ITLRecord>().ToList();
                }
            }
            return records;
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
            catch (Exception exception)
            {
            }
        }
    }

}

