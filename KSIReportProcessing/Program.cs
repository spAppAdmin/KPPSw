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
using CsvHelper;
using CsvHelper.Configuration;
using System.Net;





namespace ImportListFromCSV
{



    public class Program
    {
        public static int DefaultRetryAfterInMs { get; private set; }
        public static string RetryAfterHeaderName { get; private set; }

        // const string csvPath = "C:\\temp\\ProjectKPI.csv";

        static void Main(string[] args)
        {
            UploadKPICSV();
        }


        public static void UploadKPICSV() {

            Uri siteUri = new Uri("https://kineticsys.sharepoint.com/sites/IntranetPortal/adm/ETL");
            Uri ETLUri = new Uri("https://kineticsys.sharepoint.com/sites/IntranetPortal/adm/ETL");
            Uri ProjUri = new Uri("https://kineticsys.sharepoint.com/sites/projects");
            Uri DevUri = new Uri("https://kineticsys.sharepoint.com/sites/Dev");

            string csvPath = @"\\kineticsys.sharepoint.com\sites\IntranetPortal\adm\ETL\Documents\ProjectKPI.csv";
            string fileName = "ProjectKPI.csv";
            string sourcePath = @"\\eho-erp-ln2\Transfer\Tisoware\151\";
            string targetPath = @"\\kineticsys.sharepoint.com\sites\IntranetPortal\adm\ETL\Documents";
            //CopyFileToSP(fileName, sourcePath, targetPath);

            try {
                ClientContext ctx = getProjectSpCtx(ETLUri);
                if (ctx != null)
                {
                    List<CsvRecord> records = GetRecordsFromCsv(csvPath);
                    List spList = ctx.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["ListName"]);

                    //if(refresh)
                    //DeleteListItem(spList, ctx);

                    foreach (CsvRecord record in records)
                    {
                        CamlQuery query = new CamlQuery();
                        query.ViewXml = String.Format("@<View><Query><Where><Eq><FieldRef Name=\"Title\" /><Value Type=\"Text\">{0}</Value></Eq></Where></Query><RowLimit>10</RowLimit></View>", record.Project);
                        var existingMappings = spList.GetItems(query);
                        ctx.Load(existingMappings);
                        ctx.ExecuteQuery();

                        switch (existingMappings.Count)
                        {
                            case 0:
                                //No records found, needs to be added
                                    AddNewListItem(record, spList, ctx);
                                break;
                            default:
                                //An existing record was found - continue with next item
                                continue;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Trace.TraceError("Failed: " + ex.Message);
                Trace.TraceError("Stack Trace: " + ex.StackTrace);
            }
        }


        public static void CopyFileToSP(string fileName, string sourcePath, string targetPath)
        {
            string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
            string destFile = System.IO.Path.Combine(targetPath, fileName);
            System.IO.File.Copy(sourceFile, destFile, true);
            //Console.WriteLine(Pressanykeytoexit
            //Console.ReadKey();
        }

        public static void DeleteListItem(List spList, ClientContext ctx)
        {

            ListItemCollection listItems = spList.GetItems(CamlQuery.CreateAllItemsQuery());
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

        public static void AddNewListItem(CsvRecord record, List spList, ClientContext clientContext) {
            Int32 recordCount = 0;
            Dictionary<string, object> itemFieldValues = new Dictionary<string, object>();
            //Use reflection to iterate through the record's properties
            PropertyInfo[] properties = typeof(CsvRecord).GetProperties();
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
                            FieldLookupValue lookupFieldValue = GetLookupFieldValue(propValue.ToString(),
                                ConfigurationManager.AppSettings["LookupListName"].ToString(),
                                clientContext);
                            if (lookupFieldValue != null)
                                itemFieldValues.Add(matchingField.InternalName, lookupFieldValue);
                            else
                                throw new Exception("Lookup field value could not be added: " + propValue.ToString());
                            break;
                        case FieldType.Invalid:
                            switch (matchingField.TypeAsString)
                            {
                                default:
                                    //Code for publishing site columns not implemented
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
            ListItem oListItem = spList.AddItem(creationInfo);

            foreach (KeyValuePair<string, object> itemFieldValue in itemFieldValues)
            {
                //Set each field value
                oListItem[itemFieldValue.Key] = itemFieldValue.Value;
            }
            //Persist changes
            oListItem.Update();
            clientContext.ExecuteQuery();
        }

        public static List<CsvRecord> GetRecordsFromCsv(string csvPath)
        {
            try
            {
                List<CsvRecord> records = new List<CsvRecord>();
            using (var sr = new StreamReader(csvPath))
            {
                using (var csvReader = new CsvReader(sr))
                {
                    //csvReader.Configuration.HeaderValidated(false);
                    csvReader.Configuration.TrimOptions = TrimOptions.Trim;
                    csvReader.Configuration.RegisterClassMap<ProjectKPIMap>();

                    records = csvReader.GetRecords<CsvRecord>().ToList();
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

        public static FieldLookupValue GetLookupFieldValue(string lookupName, string lookupListName, ClientContext clientContext)
        {
            //Ref:https://karinebosch.wordpress.com/2015/05/11/setting-the-value-of-a-lookup-field-using-csom/
            var lookupList = clientContext.Web.Lists.GetByTitle(lookupListName);
            CamlQuery query = new CamlQuery();
            string lookupFieldName = ConfigurationManager.AppSettings["LookupFieldName"].ToString();
            string lookupFieldType = ConfigurationManager.AppSettings["LookupFieldType"].ToString();

            query.ViewXml = string.Format(@"<View><Query><Where><Eq><FieldRef Name='{0}'/><Value Type='{1}'>{2}</Value></Eq>" +
                                            "</Where></Query></View>", lookupFieldName, lookupFieldType, lookupName);

            ListItemCollection listItems = lookupList.GetItems(query);
            clientContext.Load(listItems, items => items.Include
                                                (listItem => listItem["ID"],
                                                listItem => listItem[lookupFieldName]));
            clientContext.ExecuteQuery();

            if (listItems != null)
            {
                ListItem item = listItems[0];
                FieldLookupValue lookupValue = new FieldLookupValue();
                lookupValue.LookupId = int.Parse(item["ID"].ToString());
                return lookupValue;
            }

            return null;
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
            }
            catch (Exception exception)
            {
            }
        }
    }
}







