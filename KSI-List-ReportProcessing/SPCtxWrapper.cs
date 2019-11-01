using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSI_List_ReportProcessing
{
    class SPCtxWrapper
    {
        private ClientContext ctx;
        public StringBuilder sbInsertTable = new StringBuilder();
        public StringBuilder sbInsertParm = new StringBuilder();


        
        public SPCtxWrapper(string siteUrl)
        {
            ctx = QueryAssistants.getProjectSpCtx(new Uri(siteUrl));

        }

        public List GetListByName(Web web, string listName)
        {
            try
            {
                var list = web.Lists.GetByTitle(listName);
                ctx.Load(list);
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ListItemCollection GetListItems(string listname, CamlQuery query)
        {
            try
            {
                var list = ctx.Web.Lists.GetByTitle(listname);
                var items = list.GetItems(query);
                ctx.Load(items);
                ctx.ExecuteQuery();
                return items;
            }
            catch (Exception)
            {

                throw;
            }
        }


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
                    script.AppendFormat("[{0}] {1},", colName, "decimal(18, 5)");
                    break;
                default:
                    script.AppendFormat("[{0}] {1},", colName, "Varchar(MAX)");
                    break;
            }
        }


        public StringBuilder ReadListSchema(List list, string listName, string viewName, ListItem item)
        {
            StringBuilder script = new StringBuilder();
            var listFieldCollection = list.Fields;
            ctx.Load(listFieldCollection);
            ctx.ExecuteQuery();
            script.AppendFormat("CREATE TABLE [{0}] (", listName);

            foreach (var field in listFieldCollection)
            {
                if (FilterColumn(field))
                    CreateScript(field.Title.Replace(' ', '_'), field.FieldTypeKind, script);
            }
            //script.Append("[Created_By] Varchar(MAX), [Modified_By] Varchar(MAX), [Modified] Varchar(MAX), [Created] Varchar(MAX)");
            script.Append(");");
            return script;
        }

        private bool FilterColumn(Field field)
        {
            //if (!field.SchemaXml.Contains("BdcField"))
            //{
            if (field.FieldTypeKind == FieldType.Computed || field.Hidden)
            {
                return false;
            }
            else if (field.ReadOnlyField && field.FieldTypeKind != FieldType.Lookup)
            {
                return false;
            }
            else if (field.FieldTypeKind == FieldType.ContentTypeId)
            {
                return false;
            }

            //else if (!field.CanBeDeleted && field.InternalName != "Attachments")
            //    {
            //return false;
            //    }
            else
            {
                return true;
            }
            //else
            //{
            //    return true;
            //}
        }

        public List<string> GetViewColumns(View view)
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



        public List<string> GetListColumns(List list)
        {
            try
            {
                List<string> viewcolumns = new List<string>();
                var listfields = list.Fields;
                ctx.Load(listfields);
                ctx.ExecuteQuery();
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


    }
}
