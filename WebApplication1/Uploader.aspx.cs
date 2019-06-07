using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using sp = Microsoft.SharePoint.Client;
using System.Web.Hosting;
using System.Text;

namespace Uploads
{
    public partial class Uploader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getProjectList();
            getFileTable();
        }

        public void getProjectList()
        {
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            //var ctx = spContext.CreateUserClientContextForSPHost();
            //ClientContext ctx = new ClientContext("https://kineticsys.sharepoint.com/sites/projects");

            Uri uri = new Uri("https://kineticsys.sharepoint.com/sites/projects");
            ClientContext ctx = getProjectSpCtx(uri);

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
                ddlProjCat.Items.Add(new System.Web.UI.WebControls.ListItem(hyp.Description, hyp.Url));
            }
        }

        public void getFileTable()
        {
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            //var ctx = spContext.CreateUserClientContextForSPHost();
            //ClientContext ctx = new ClientContext("https://kineticsys.sharepoint.com/sites/projects");

            Uri uri = new Uri("https://kineticsys.sharepoint.com/sites/projects");
            ClientContext ctx = getProjectSpCtx(uri);

            Web web = ctx.Web;
            List list = ctx.Web.Lists.GetByTitle("CSV Uploader");
            var q = new CamlQuery() { ViewXml = "<View><Query /><ViewFields><FieldRef Name='FileLeafRef' /><FieldRef Name='Title' /><FieldRef Name='Target_x0020_List' /><FieldRef Name='Project' /><FieldRef Name='_x0023_Records' /><FieldRef Name='FileSizeDisplay' /><FieldRef Name='Last_x0020_Run' /><FieldRef Name='Completed' /></ViewFields><QueryOptions /></View>" };
            sp.ListItemCollection li = list.GetItems(q);
            ctx.Load(li);
            ctx.ExecuteQuery();

            StringBuilder sb = new StringBuilder();
            sb.Append("<br/><br/>");
            sb.Append("<table border='1' cellpadding='3'>");



            foreach (var item in li)
            {
                var file = item.FieldValues["FileLeafRef"];
                var projSiteURL = item.FieldValues["Project"];
                string projSite = string.Format("{0}", item.FieldValues["Title"]);
                string targetList = string.Format("{0}", item.FieldValues["Target_x0020_List"]);
                string Project = string.Format("{0}", item.FieldValues["Project"]);

                sb.Append("<tr>");

                sb.Append("<td>");
                sb.Append(file);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(projSiteURL);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(projSite);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(projSite);
                sb.Append("</td>");

                sb.Append("<td>");
                sb.Append(Project);
                sb.Append("</td>");

                sb.Append("</tr>");
            }
            sb.Append("</table>");

            CVSUploader.Text = sb.ToString();
            //return sb.ToString();
        }

        protected void uploadFile_Click(object sender, EventArgs e)

        {
            // Deploy to library
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            //var ctx = spContext.CreateUserClientContextForSPHost();

            Uri uri = new Uri("https://kineticsys.sharepoint.com/sites/projects");
            ClientContext ctx = getProjectSpCtx(uri);

            var lib = "CSV Uploader";
            //var file = filePath.Text;
            var file = @"C:\Temp\ITL.csv";
            var fn = "ITL.csv";
            uploadCSVFile(ctx, lib, file, fn);

            //var csvPath = @"\\kineticsys.sharepoint.com\sites\dev\CSV%20Uploader\ITL.csv";
            //Uri projSiteUrl = new Uri("https://kineticsys.sharepoint.com/sites/dev");
            //var tList = "ITLUploadTest";
            //var lookup = "costCode";

            //string csvPath = @"\\kineticsys.sharepoint.com\sites\dev\CSV%20Uploader\ITL.csv";
            //string fileName = "ProjectKPI.csv";
            //string sourcePath = @"\\eho-erp-ln2\Transfer\Tisoware\151\";
            //string targetPath = @"\\kineticsys.sharepoint.com\sites\IntranetPortal\adm\ETL\Documents";



            //i.Program.UploadITLCSV(csvPath, projSiteUrl, "yes", tList, fn, lookup);
            //i.Program.UploadKPICSV();

        }

        public void uploadCSVFile(ClientContext ctx, string libraryName, string filePath, string fn)
        {
            Web web = ctx.Web;
            FileCreationInformation newFile = new FileCreationInformation();
            newFile.Overwrite = true;
            newFile.Content = System.IO.File.ReadAllBytes(filePath);
            newFile.Url = System.IO.Path.GetFileName(filePath);

            List docs = web.Lists.GetByTitle(libraryName);

            // Add file to the library.
            Microsoft.SharePoint.Client.File uploadFile = docs.RootFolder.Files.Add(newFile);
            sp.ListItem item = uploadFile.ListItemAllFields;
            item["Title"] = "ITL";
            item["Project"] = "project";
            item["_x0023_Records"] = "RECORDS";
            item["Target_x0020_List"] = "Target";
            item["_x0023_Records"] = 22;

            uploadFile.ListItemAllFields.Update();

            ctx.Load(uploadFile);
            ctx.ExecuteQuery();
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
    }



