using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.SharePoint.Client;
using System.IO;
using Microsoft.SharePoint.Client.Taxonomy;
using System.Configuration;
using System.Security;

namespace KSI_List_ReportProcessing
{
    /// <summary>
    /// This class provide methods specific to archival application.
    /// </summary>
    class SharePointContextWrapper
    {

        #region Member
        private ClientContext _context;
        public StringBuilder sbInsertTable = new StringBuilder();
        public StringBuilder sbInsertParm = new StringBuilder();
        #endregion

        #region Public Constructor
        public SharePointContextWrapper(string siteUrl)
        {
            _context = new ClientContext(siteUrl);

            string accountName = ConfigurationManager.AppSettings["AccountName"];
            char[] pwdChars = ConfigurationManager.AppSettings["AccountPwd"].ToCharArray();
            SecureString accountPwd = new SecureString();
            for (int i = 0; i < (int)pwdChars.Length; i++)
            {
                accountPwd.AppendChar(pwdChars[i]);
            }

            _context.Credentials = new SharePointOnlineCredentials(accountName, accountPwd);



            //_context.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the List object in a web by its Name
        /// </summary>
        /// <param name="web">Web object</param>
        /// <param name="listName">Name of the List</param>
        /// <returns>List object</returns>
        public List GetListByName(Web web, string listName)
        {
            try
            {
                var list = web.Lists.GetByTitle(listName);
                _context.Load(list);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get List Items based on the query
        /// </summary>
        /// <param name="listname">name of the list</param>
        /// <param name="query">filter query</param>
        /// <returns>ListItemCollection object</returns>
        public ListItemCollection GetListItems(string listname, CamlQuery query)
        {
            try
            {
                var list = _context.Web.Lists.GetByTitle(listname);
                var items = list.GetItems(query);
                _context.Load(items);
                _context.ExecuteQuery();
                return items;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// Map the Sharepoint Columns to SQL table Columns
        /// </summary>
        /// <param name="colName">Name of the Column</param>
        /// <param name="colType">column tyoe</param>
        /// <param name="script">script</param>
        private void CreateScript(string colName, FieldType colType, StringBuilder script)
        {
            switch (colType)
            {
                case FieldType.Boolean:
                    script.AppendFormat("[{0}] {1},", colName, "BIT");
                    break;
                case FieldType.DateTime:
                    script.AppendFormat("[{0}] {1},", colName, "DATETIME");
                    break;
                case FieldType.Integer:
                    script.AppendFormat("[{0}] {1},", colName, "Int");
                    break;
                case FieldType.Lookup:
                    script.AppendFormat("[{0}] {1},", colName, "Varchar(MAX)");
                    break;
                case FieldType.Attachments:
                    script.AppendFormat("[{0}] {1},", colName, "Varchar(MAX)");
                    break;
                case FieldType.Number:
                    script.AppendFormat("[{0}] {1},", colName, "Varchar(MAX)");
                    break;
                default:
                    script.AppendFormat("[{0}] {1},", colName, "Varchar(MAX)");
                    break;
            }
        }

        /// <summary>
        /// Read View Schema for creating new table script
        /// </summary>
        /// <param name="list">List to be Archieved</param>
        /// <param name="viewName">View of List to be Archieved</param>
        /// <returns>Script to Create new Table based on View name</returns>
        public StringBuilder ReadListSchema(List list, string listName, string viewName, ListItem item)
        {
            StringBuilder script = new StringBuilder();
            var listFieldCollection = list.Fields;
            _context.Load(listFieldCollection);
            _context.ExecuteQuery();
            script.AppendFormat("CREATE TABLE [{0}] (", listName);

            foreach (var field in listFieldCollection)
            {
                if (FilterColumn(field))
                    CreateScript(field.Title.Replace(' ', '_'), field.FieldTypeKind, script);


            }
            script.Append("[Created_By] Varchar(MAX), [Modified_By] Varchar(MAX), [Modified] Varchar(MAX), [Created] Varchar(MAX)");
            script.Append(");");
            return script;
        }

        private bool FilterColumn(Field field)
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

        /// <summary>
        /// Build the Insert query based on View Schema.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="viewName"></param>
        public void GenerateInsertQuery(List list, string viewName)
        {
            var listFieldCollection = list.Fields;
            _context.Load(listFieldCollection);
            _context.ExecuteQuery();
            int count = 1;
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
        public List<string> GetViewColumns(View view)
        {
            try
            {
                List<string> viewcolumns = new List<string>();
                var fields = view.ViewFields;
                _context.Load(fields);
                _context.ExecuteQuery();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="view"></param>
        /// <returns></returns>
        public List<string> GetListColumns(List list)
        {
            try
            {
                List<string> viewcolumns = new List<string>();
                var listfields = list.Fields;
                _context.Load(listfields);
                _context.ExecuteQuery();
                foreach (var field in listfields)
                {
                    if (FilterColumn(field))
                        viewcolumns.Add(field.InternalName);
                }
                viewcolumns.AddRange(Constants.DEFAULTFIELDS.Split(','));
                return viewcolumns;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Populate the Values to be inserted in the Database for the given List Item.
        /// </summary>
        /// <param name="item">List item whose values to be inserted.</param>
        /// <param name="viewColumns">list of view columns whose datahas to be stored.</param>
        /// <returns></returns>
        public string GetInsertValue(ListItem item, List<string> viewColumns, int itemId, string listName)
        {
            StringBuilder sbInsertValue = new StringBuilder();
            foreach (var vc in viewColumns)
            {
                try
                {

                    if (vc == "LinkTitle")
                    {
                        string itemname = string.Empty;
                        if (item["Title"] != null)
                        {
                            itemname = item["Title"].ToString().Replace("'", "");
                        }
                        sbInsertValue.AppendFormat("'{0}'", itemname);
                        sbInsertValue.Append(",");
                        continue;
                    }

                    if (item[vc] is FieldLookupValue || item[vc] is FieldLookupValue[])
                    {
                        if (item[vc] is FieldLookupValue[])
                        {
                            foreach (var user in item[vc] as FieldLookupValue[])
                            {
                                var u = user as FieldLookupValue;
                                sbInsertValue.AppendFormat("'{0}'", u.LookupValue.Replace("'", ""));
                                sbInsertValue.Append(";");
                            }
                        }
                        else
                        {
                            var flv = item[vc] as FieldLookupValue;
                            sbInsertValue.AppendFormat("'{0}'", flv.LookupValue.Replace("'", ""));
                        }

                    }
                    else if (vc == "Attachments" && Convert.ToBoolean(item[vc]))
                    {
                        try
                        {
                            string attachmentPath = Environment.GetEnvironmentVariable("AttachmentPath");
                            item.Context.Load(item.AttachmentFiles);
                            item.Context.ExecuteQuery();
                            string attachmentpath = string.Empty;
                            foreach (var attachment in item.AttachmentFiles)
                            {
                                var f = Microsoft.SharePoint.Client.File.OpenBinaryDirect(item.Context as ClientContext, attachment.ServerRelativeUrl);
                                item.Context.ExecuteQuery();
                                StreamReader sr = new StreamReader(f.Stream);
                                var filecontents = Encoding.UTF8.GetBytes(sr.ReadToEnd());
                                sr.Close();
                                string id = string.Concat("_", itemId.ToString());
                                int startIndex = attachment.FileName.IndexOf(".");
                                string attachmentName = attachment.FileName.Insert(startIndex, id);
                                string fullpath = Path.Combine(attachmentPath, attachmentName);
                                try
                                {
                                    FileStream fs = System.IO.File.Create(fullpath);
                                    fs.Write(filecontents, 0, filecontents.Length);
                                    fs.Close();
                                }
                                catch (Exception)
                                {
                                    Console.WriteLine("Adding Attachment");
                                }

                                attachmentpath += fullpath + ";";
                            }
                            sbInsertValue.AppendFormat("'{0}'", attachmentpath.Replace("'", ""));
                        }
                        catch (Exception)
                        {
                            sbInsertValue.AppendFormat("'{0}'", string.Empty);
                            sbInsertValue.Append(",");
                        }
                    }
                    else
                    {
                        if (item[vc] == null)
                        {
                            sbInsertValue.AppendFormat("'{0}'", string.Empty);

                        }
                        else
                        {
                            if (item[vc] is TaxonomyFieldValueCollection)
                            {
                                foreach (var term in item[vc] as TaxonomyFieldValueCollection)
                                {
                                    sbInsertValue.AppendFormat("'{0}'", term.Label.Replace("'", ""));
                                    sbInsertValue.Append(";");
                                }
                            }
                            sbInsertValue.AppendFormat("'{0}'", item[vc].ToString().Replace("'", ""));

                        }
                    }
                    sbInsertValue.Append(",");

                }

                catch (Exception)
                {
                    sbInsertValue.AppendFormat("'{0}'", string.Empty);
                    sbInsertValue.Append(",");
                }

            }

            sbInsertValue.Remove(sbInsertValue.Length - 1, 1);
            return sbInsertValue.ToString();
        }


        public Web GetWeb()
        {
            return _context.Web;
        }
        #endregion

    }
}
