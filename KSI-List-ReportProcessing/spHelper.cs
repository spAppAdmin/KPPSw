using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using sp = Microsoft.SharePoint.Client;



namespace KSI_List_ReportProcessing
{
    public static class Extensions
    {
        public static bool IsNegative(this int n)
        { return Math.Abs(n) > n; }
    }


    public static class QueryAssistants
    {

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


        internal static string getInternalFieldName(string strList, string field, ClientContext ctx)
        {
            field = field.Trim();
            var cList = ctx.Web.Lists.GetByTitle(strList);
            var fc = cList.Fields.GetByInternalNameOrTitle(field);
            ctx.Load(cList);
            ctx.Load(fc);
            ctx.ExecuteQuery();
            var internalName = fc.InternalName;
            return internalName;
        } // EndMethod: getInternalFieldName

        internal static int getListItemID(List cList, string keyVal, string keyID, ClientContext ctx)
        {
            //var cList = ctx.Web.Lists.GetByTitle(strlist);
            int rtnID = 0;
            var query = new CamlQuery
            {
                ViewXml = "<View><Query><Where><Eq><FieldRef Name='" + keyID + "'/><Value Type='Text'>" + keyVal + "</Value></Eq></Where></Query><RowLimit>1</RowLimit><ViewFields><FieldRef Name='ID' /></ViewFields><QueryOptions /></View>"
            };
            var rtn = cList.GetItems(query);
            ctx.Load(rtn);
            ctx.ExecuteQuery();

            foreach (ListItem oListItem in rtn)
            {
                rtnID = oListItem.Id;
            }
            return rtnID;
        } // EndMethod: getListItemID

    } // EndClass: Query Assistants


    public class GeneralLogging
    {

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


        /// Error Log Tracking
        public static void WriteExceptionToLog(Exception ex, string app)
        {
            try
            {

                Uri AppLogUrl = new Uri(ConfigurationManager.AppSettings["AppLogUrl"].ToString());
                ClientContext ctx = getProjectSpCtx(AppLogUrl);
                AddNewListItem(ex, ctx, app);

                ex.Data["Log"] = new List<string>();
                using (StreamWriter sr = System.IO.File.AppendText(@"\\kineticsys.sharepoint.com\sites\IntranetPortal\adm\ETL\Lists\AppLog\Logs\Log.txt"))
                {
                    sr.WriteLine("=>" + DateTime.Now + " " + " An Error occurred: " + ex.StackTrace + " Message: " + ex.Message + "\n\n");
                    sr.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static List<string> Output = new List<string>();


        /// Error Log Tracking
        


        /// Status Log Tracking            
        public static void WriteStatusHistoryToLog()
        {
            try
            {
                using (StreamWriter sr = System.IO.File.AppendText("Log.txt"))
                {
                    sr.WriteLine("=================================================================================================");
                    foreach (string item in Output)
                    { sr.WriteLine("=>" + DateTime.Now + " " + item + "\n\n"); }
                    sr.WriteLine("=================================================================================================");
                    sr.Flush();
                }
            }
            catch (Exception)
            {
                throw;
            }
        } // EndMethod: WriteStatusToLog

        //Add new list item 
        public static void AddNewListItem(Exception ex, ClientContext ctx, string app)
        {
            var AppLogList = ConfigurationManager.AppSettings["AppLogList"];
            List spList = ctx.Web.Lists.GetByTitle(AppLogList);
            ListItemCreationInformation creationInfo = new ListItemCreationInformation();
            ListItem oListItem = spList.AddItem(creationInfo);

            oListItem["Title"] = app;
            oListItem["Messsage"] = ex.Message;
            oListItem["innerMsg"] = ex.InnerException.Message;
            oListItem["Data"] = ex.Data;
            oListItem["Trace"] = ex.StackTrace;

            oListItem.Update();
            ctx.ExecuteQuery();
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





} // EndClass: General Logging


public class LogWriter
{
    private string m_exePath = string.Empty;

    public LogWriter(string logMessage)
    {
        // this.LogWrite(logMessage);
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
        catch (Exception ex)
        {
        }
    }
    }
    

    /*
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
*/







