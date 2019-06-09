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
                  getProjectList();
                 
        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
                var ctx = spContext.CreateUserClientContextForSPHost();

                var projectURL = txtProjURL.Text;
                var file = csvFile.Value.ToString();
                var listName = ddTargetList.SelectedValue;
                var path = Path.Combine("@", file);
                string fileNameWoExt = Path.GetFileNameWithoutExtension(file);
                string fileNamewExt = Path.GetFileName(file);
                string filepath = Path.GetFullPath(file);
                string fileExt = Path.GetExtension(file);
                string csvPath = file;
                Uri projSiteUrl = new Uri(projectURL);
                var lookup = "CostCodeList";
                 string lookupFieldName = "AreaName_x002b_SubTask";
                string lookupFieldType = "Calculated";
                var action = rbAction.SelectedValue;
                Int32 recordCount = 0;

                switch (action)
                {
                    case "Delete":
                        ActionDeleteAllListItems(listName, projSiteUrl);
                        break;
                    case "DeleteCSV":
                        recordCount = ActionDeleteCSVListItems(listName,projSiteUrl,csvPath);
                        break;
                    case "AddNew":
                        recordCount = ActionAddNewListItems(csvPath, projSiteUrl, action, listName, fileNamewExt, lookup, lookupFieldName, lookupFieldType);
                        break;
                    case "AddAll":
                        recordCount = ActionAddAllListItems(csvPath, projSiteUrl, action, listName, fileNamewExt, lookup, lookupFieldName, lookupFieldType);
                        break;
                    case "Update":
                        recordCount = ActionUpdateListItems(csvPath, projSiteUrl, action, listName, fileNamewExt, lookup, lookupFieldName, lookupFieldType);
                        break;
                }

               ScriptManager.RegisterStartupScript(UpdatePanel, UpdatePanel.GetType()  , "Alert", "alert('Records Loaded'" + recordCount + "')", true);

                ShowError("Records Loaded" + recordCount);


                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append(@"<script language='javascript'>");
                sb.Append(@"var lbl = document.getElementById('msgHtml');");
                sb.Append(@"lbl.color='red';");
                sb.Append(@"</script>");

            } catch (Exception ex) {
                ShowError(ex.Message);

                ScriptManager.RegisterStartupScript(UpdatePanel, UpdatePanel.GetType(), "KK", "alert('Exception')", true);
                //ScriptManager.RegisterStartupScript(UpdatePanel, UpdatePanel.GetType(), "alert", "alert('xxx')", true);
                //ScriptManager.RegisterClientScriptBlock(msg, this.GetType(), "AlertMsg", "<script language='javascript'>var e = document.getElementById('msg');e.text=" + ex.Message + "</script>", false);
                //Console.WriteLine(ex.Message);
                //SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source);
                //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "ex", "alert('" + ex.Message + "');", true);

            }

        }

        private void HideError()
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "HideErrorMessageScript", HideErrorMessageScript(), true);
        }

        private void ShowError(string errorMessage)
        {
            ScriptManager.RegisterStartupScript(Page, GetType(), "ErrorMessageScript", ErrorMessageScript(errorMessage), true);
        }
        
        private static string HideErrorMessageScript()
        {
            return @"
$(document).ready(function() {
    $('#errorMessagePlaceHolder').hide();
});
";
        }

        private static string ErrorMessageScript(string errorMessage)
        {
            return
                string.Format(
                    @"
$(document).ready(function() {{
    $('#errorMessagePlaceHolder').html('{0}');
    $('#errorMessagePlaceHolder').show();
}});
",
                    errorMessage);
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
                if (totalListItems > 0)
                {
                    for (var counter = totalListItems - 1; counter > -1; counter--)
                    {
                        listItems[counter].DeleteObject();
                        ctx.ExecuteQuery();
                        rtnRecord = counter;
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
                    throw ex;
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
                }
            }
            return recordCount;

        } catch (Exception ex)
            {
                throw ex;
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
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        var existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();

                        recordCount = existingMappings.Count;

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
                throw ex;
            }
            
        }

        public static Int32 ActionAddAllListItems(string csvPath, Uri projSiteURI, string action, string listName, string fileName, string lookup, string lookupFieldName, string lookupFieldType)
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
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query></View>", record.Title);
                        var existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();
                        recordCount = existingMappings.Count;
                        AddNewListItem(record, spList, ctx, lookup, lookupFieldName, lookupFieldType);
                    }
                }
                return recordCount;
            }
            catch (Exception ex)
            {
                throw ex;
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
                oListItem.Update();
                clientContext.ExecuteQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

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
                        MissingFieldFound = null
                    });

                    using (parser)
                    {
                        //csvReader.Configuration.HeaderValidated(false);
                        parser.Configuration.TrimOptions = TrimOptions.Trim;
                        parser.Configuration.RegisterClassMap<ProjectITLMap>();
                        records = parser.GetRecords<ITLRecord>().ToList();
                    }
                }
            
                return records;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
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
                msg m = new msg();
                m._msg = "X";
                
            }
            catch (Exception exception)
            {
            }
        }


        public class msg 
        {
            public string _msg { get; set; }
        }




    }

}

