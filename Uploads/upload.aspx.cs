using System;
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
 

namespace csvUploadWeb.Pages
{
    public partial class upload : System.Web.UI.Page
    {

        protected void Page_PreInit(object sender, EventArgs e)
        {
    
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // define initial script, needed to render the chrome control
            string script = @"
                function chromeLoaded() {
                    $('body').show();
                }

                //function callback to render chrome after SP.UI.Controls.js loads
                function renderSPChrome() {
                    //Set the chrome options for launching Help, Account, and Contact pages
                    var options = {
                        'appTitle': document.title,
                        'onCssLoaded': 'chromeLoaded()'
                    };

                    //Load the Chrome Control in the divSPChrome element of the page
                    var chromeNavigation = new SP.UI.Controls.Navigation('divSPChrome', options);
                    chromeNavigation.setVisible(true);
                }";

            //register script in page
            //Page.ClientScript.RegisterClientScriptBlock(typeof(Default), "BasePageScript", script, true);

            //getProjectList();
        }

   

        protected void Button1_Click1(object sender, EventArgs e)
        {
            // Deploy to library
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            //var ctx = spContext.CreateUserClientContextForSPHost();

            //Uri uri = new Uri("https://kineticsys.sharepoint.com/sites/dev");
            //var ctx = getProjectSpCtx(uri);
            var lib = "CSV Uploader";
            var file = @"C:\Users\John.Mimiaga\Downloads\ITL.csv";
            var fn = "ITL.csv";
            //UploadDocumentContent(ctx, lib, file, fn);

            var csvPath = @"\\kineticsys.sharepoint.com\sites\dev\CSV%20Uploader\ITL.csv";
            Uri projSiteUrl = new Uri("https://kineticsys.sharepoint.com/sites/dev");
            var tList = "ITLUploadTest";
            var lookup = "costCode";

            //string csvPath = @"\\kineticsys.sharepoint.com\sites\dev\CSV%20Uploader\ITL.csv";
            //string fileName = "ProjectKPI.csv";
            //string sourcePath = @"\\eho-erp-ln2\Transfer\Tisoware\151\";
            //string targetPath = @"\\kineticsys.sharepoint.com\sites\IntranetPortal\adm\ETL\Documents";



            //i.Program.UploadITLCSV(csvPath, projSiteUrl, "yes", tList, fn, lookup);
            //i.Program.UploadKPICSV();

        }


        public void UploadDocumentContent(ClientContext ctx, string libraryName, string filePath, string fn)
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


        public void getProjectList()
        {
            //var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);
            //var ctx = spContext.CreateUserClientContextForSPHost();


            //ClientContext ctx = new ClientContext("https://kineticsys.sharepoint.com/sites/projects");
            Uri uri = new Uri("https://kineticsys.sharepoint.com/sites/projects");

            ClientContext ctx = getProjectSpCtx(uri);

                Web web = ctx.Web;
                List list = ctx.Web.Lists.GetByTitle("KPPS Project Catalog");
                var q = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Active</Value></Eq></Where></Query><ViewFields><FieldRef Name='Project_x0020_Name' /><FieldRef Name='Proj_x0020_Site_x0020_URL' /></ViewFields><QueryOptions /></View>" };
                sp.ListItemCollection li = list.GetItems(q);
                ctx.Load(li);
                ctx.ExecuteQuery();


            


            foreach (var item in li)
            {
                var hyp = (FieldUrlValue)item["Proj_x0020_Site_x0020_URL"];
                

                string URL = string.Format("{0}", item.FieldValues["Proj_x0020_Site_x0020_URL"]);
                string text = string.Format("{0}", item.FieldValues["Project_x0020_Name"]);
                ddlProjCat.Items.Add(new System.Web.UI.WebControls.ListItem(hyp.Url, hyp.Description));
            }

        }


        //sp.ListItemCollection collListItem = list.GetItems(query);
        //ctx.Load(collListItem);
        //ctx.ExecuteQuery();
        //Microsoft.SharePoint.Client.ListItemCollection collection = list.GetItems(query);
        //ctx.Load(collection);
        //ctx.ExecuteQuery();

            
      


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

            //using (ClientContext ctx = new ClientContext(UriProject)
            //{
            //Credentials = new SharePointOnlineCredentials(accountName, accountPwd)
            //})

            //ClientContext ctx = new ClientContext(UriProject);
            //ctx.Credentials = new SharePointOnlineCredentials(item, secureString));

            return ctx;
        }

      
    }

}





