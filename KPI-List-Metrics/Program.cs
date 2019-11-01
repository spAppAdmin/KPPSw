using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.IO;
using P = ProcessHelpers;

namespace KPI_List_Metrics
{
    class Program
    {
        public StringBuilder sbInsertTable = new StringBuilder();
        public StringBuilder sbInsertParm = new StringBuilder();

        static void Main(string[] args)
        {
            ClientContext ctxCatalog = P.QueryAssistants.getProjectSpCtx(new Uri("https://kineticsys.sharepoint.com/sites/projects"));
            List oList = ctxCatalog.Web.Lists.GetByTitle("KPPS Projects Catalog");

            CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Active</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };
            //CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title' /><Value Type='Text'>622000336</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };

            ListItemCollection projIitems = oList.GetItems(camlQuery);
            ctxCatalog.Load(projIitems);
            ctxCatalog.ExecuteQuery();


            //Get project site from Project Catalog

            foreach (ListItem item in projIitems)
            {
                var hyperLink = (FieldUrlValue)item["Proj_x0020_Site_x0020_URL"];
                string projSiteURL = hyperLink.Url;
                var projCatalogNum = item["Title"].ToString();
                var prjCatalogName = item["Project_x0020_Name"].ToString();


                //Set project site context

                ClientContext ctx = P.QueryAssistants.getProjectSpCtx(new Uri(projSiteURL));

                //get list fields

                List list = ctx.Web.Lists.GetByTitle("ITL");
                string vn = "Default";

                GenerateInsertQuery(list, vn, ctx);



                //StringBuilder script = new StringBuilder();
                //var listFieldCollection = list.Fields;
                //ctx.Load(list);
                //ctx.Load(listFieldCollection);
                //ctx.ExecuteQuery();
            }
        }



        public static void GenerateInsertQuery(List list, string viewName, ClientContext ctx)
        {
            var listFieldCollection = list.Fields;
            ctx.Load(listFieldCollection);
            ctx.ExecuteQuery();
            int count = 1;
             StringBuilder sbInsertParm = new StringBuilder();
            StringBuilder sbInsertTable = new StringBuilder();

            foreach (var field in listFieldCollection)
            {
                try
                {
                    if (FilterColumn(field))
                    {
                        var fieldtitle = field.Title.Replace(' ', '_');
                        sbInsertParm.AppendFormat("@{0}", "p" + count++);
                        sbInsertParm.Append(",");
                        sbInsertTable.AppendFormat("[{0}]", fieldtitle);
                        sbInsertTable.Append(",");
                    }

                }
                catch (Exception)
                {
                    throw;
                }
            }
            sbInsertParm.Remove(sbInsertParm.Length - 1, 1);
            //appending default required columns 
            foreach (var item in Constants.DEFAULTFIELDS.Split(','))
            {
                if (item == "Author")
                {
                    sbInsertTable.Append(string.Format("[{0}]", "Created_By"));
                }
                else if (item == "Editor")
                {
                    sbInsertTable.Append(string.Format("[{0}]", "Modified_By"));
                }
                else
                {
                    sbInsertTable.Append(string.Format("[{0}]", item));
                }
                sbInsertTable.Append(",");
            }
            sbInsertTable.Remove(sbInsertTable.Length - 1, 1);
        }

        /// <summary>
        /// returns all the columns in a View
        /// </summary>
        /// <param name="view">Name of the View</param>
        /// <returns>List of View COlumns</returns>
        /// 


        private static bool FilterColumn(Field field)
        {
            if (!field.SchemaXml.Contains("BdcField"))
            {
                if (field.FieldTypeKind == FieldType.Computed || field.Hidden || field.Sealed)
                {
                    return false;
                }
                else if (field.ReadOnlyField && field.FieldTypeKind != FieldType.Lookup)
                {
                    return false;
                }
                else if (!field.CanBeDeleted && field.InternalName != "Attachments")
                {
                    return false;
                }
                return true;
            }
            else
            {
                return true;
            }
        }




        public List<string> GetViewColumns(View view,ClientContext ctx )
        {
            try
            {
                List<string> viewcolumns = new List<string>();
                var fields = view.ViewFields;
                ctx.Load(fields);
                ctx.ExecuteQuery();
                foreach (var field in fields)
                {
                    viewcolumns.Add(field);
                }
                return viewcolumns;
            }
            catch (Exception)
            {
                throw;
            }
        }






    }
}
