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


namespace Uploads
{
    public partial class Uploader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            getProjectList();
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


