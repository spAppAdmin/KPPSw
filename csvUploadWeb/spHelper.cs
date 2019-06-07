using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SPA = csvUploadWeb.QueryAssistants;
using SPL = csvUploadWeb.GeneralLogging;


namespace csvUploadWeb
{
    internal static class Extensions
    {
        internal static string ForceLength(this string str, int Padding)
        {
            if (str.Length > Padding)
            { return str.Substring(0, Padding); }
            else
            { return str.PadRight(Padding); }
        }
    }

    internal static class QueryAssistants
    {
        // want to expand this to only re
        internal static Errors Execute(ClientContext ctx)
        {
            try
            {
                ctx.ExecuteQuery();
                return Errors.Okay;
            }
            catch (Exception ex)
            {
                SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source);
                if (ex.Message.ToUpper().Contains("TIME") == true)
                { return Errors.Failed_TimedOut; }
                else if (ex.Message.ToUpper().Contains("2,097,152") == true)
                { return Errors.Failed_Size; }
                else
                { return Errors.Failed_Other; }
            }
        }



        internal static string getInternalFieldName(string strList, string field, ClientContext ctx)
        {
            string internalName = "";
            Field fc = null;
            try
            {
                field = field.Trim();
                var cList = ctx.Web.Lists.GetByTitle(strList);
                fc = cList.Fields.GetByInternalNameOrTitle(field);
                ctx.Load(cList);
                ctx.Load(fc);
            }
            catch (Exception ex)
            { SPL.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source); }

            SPA.Execute(ctx);
            if (fc != null)
            { internalName = fc.InternalName; }
            return internalName;
        } // EndMethod: getInternalFieldName

        internal static int getListItemID(List cList, string keyVal, string keyID, ClientContext ctx)
        {
            int rtnID = 0;
            try
            {
                var query = new CamlQuery
                {
                    ViewXml = Q.oView +
                                    Q.oQuery +
                                        Q.oWhere +
                                            Q.oEq +
                                                Q.oFieldRef.Named(keyID) + Q.cFieldRef +
                                                Q.oValue.Typed("Text") + keyVal + Q.cValue +
                                            Q.cEq +
                                        Q.cWhere +
                                    Q.cQuery +
                                    "<RowLimit>1</RowLimit><ViewFields><FieldRef Name='ID' /></ViewFields><QueryOptions />" +
                                Q.cView
                };
                var rtn = cList.GetItems(query);
                ctx.Load(rtn);
                SPA.Execute(ctx);
                rtnID = rtn[0].Id;
            }
            catch (Exception ex)
            { GeneralLogging.LogEntries.Add("Event => ErrorMessage: " + ex.Message + " ErrorSource: " + ex.Source); }
            return rtnID;
        } // EndMethod: getListItemID

    } // EndClass: Query Assistants

    internal static class GeneralLogging
    {
        internal static List<string> LogEntries = new List<string>();

        internal static void WriteHistoryToLog()
        {
            bool done = false;
            LogEntries.Add("=================================================================================================");

            // makes a brute force attempt to obtain log file write access and gives up/console closes after 10 attempts.
            for (int i = 0; i < 10; i++)
            {
                if (done == false)
                {
                    try
                    {
                        using (StreamWriter sr = System.IO.File.AppendText("Log.txt"))
                        {
                            foreach (string item in LogEntries)
                            { sr.WriteLine(item + "\n\n"); }
                            sr.WriteLine("\n\n");
                            sr.Flush();
                            done = true;
                        }
                    }
                    catch (Exception)
                    {
                        System.Threading.Thread.Sleep(200);
                    }
                }
            }
        } // EndMethod: WriteStatusToLog

    } // EndClass: General Logging

    #region CustomConstructs

    public class ColumnName
    {
        public string Name, TrueName, Value;
        public ColumnName(string n, string v, string t = "") { Name = n; TrueName = t; Value = v; }
    } // EndClass: ColumnName

    public enum Errors
    {
        Failed_TimedOut,
        Failed_Size,
        Failed_Other,
        Okay
    }

    public enum StateOptions
    {
        Show,
        Minimize,
        Hide
    } // Window States Enum

    public class ArgsObject
    {
        public string Command, URL, Table, FilePath;
        public StateOptions State = StateOptions.Minimize;
        public ColumnName PrimaryKey;
        public List<ColumnName> Values = new List<ColumnName>();
        private ArgsObject() { }
        public ArgsObject(List<string> args)
        {
            if (args.Count >= 4)
            {
                switch (args[0].ToUpper().Trim())
                {
                    case "HIDE": State = StateOptions.Hide; args.RemoveAt(0); break;
                    case "SHOW": State = StateOptions.Show; args.RemoveAt(0); break;
                    default: State = StateOptions.Minimize; break;
                }



                if (new string[] { "ADD", "UPDATE", "DELETE", "READ", "IMPORT" }.Contains(args[0].ToUpper().Trim()) == true)
                { Command = args[0].ToUpper().Trim(); }
                else
                { Command = "ERROR"; }


                if (args[1].ToUpper().Trim().StartsWith("HTTP") == true)
                { URL = args[1].Trim(); }


                if (args[2].Trim().Length >= 1)
                { Table = args[2].Trim(); }


                if (Command == "IMPORT")
                {
                    PrimaryKey = new ColumnName("ID", "", "ID");
                    Values.Add(new ColumnName(args[3].Trim(), ""));
                }
                else if (args[3].Contains("|") == true)
                {
                    string[] pair = args[3].Split('|');
                    if (pair[0].ToUpper() == "ID")
                    { PrimaryKey = new ColumnName(pair[0].Trim(), pair[1], "ID"); }
                    else
                    { PrimaryKey = new ColumnName(pair[0].Trim(), pair[1]); }
                }
                else if (args[3].Trim().Length >= 1)
                {
                    if (args[3].ToUpper() == "ID")
                    { PrimaryKey = new ColumnName(args[3].Trim(), "", "ID"); }
                    else
                    { PrimaryKey = new ColumnName(args[3].Trim(), ""); }
                }


                if (args.Count > 4)
                {
                    for (int i = 4; i < args.Count; i++)
                    {
                        if (args[i].Contains("|") == true)
                        {
                            string[] pair = args[i].Split('|');
                            if (pair[0].ToUpper() == "ID")
                            { Values.Add(new ColumnName(pair[0].Trim(), pair[1].Trim(), "ID")); }
                            else
                            { Values.Add(new ColumnName(pair[0].Trim(), pair[1].Trim())); }
                        }
                        else if (args[i].Trim().Length >= 1)
                        {
                            if (args[i].ToUpper() == "ID")
                            { Values.Add(new ColumnName(args[i].Trim(), "", "ID")); }
                            else
                            { Values.Add(new ColumnName(args[i].Trim(), "")); }
                        }
                    }
                }


                if (Command == "READ" || Command == "IMPORT")
                {
                    if (Values.Last().Value != "")
                    {
                        FilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Trim('\\') + "\\Default.CSV";
                    }
                    else
                    {
                        if (System.IO.Path.IsPathRooted(Values.Last().Name) == true)
                        { FilePath = Values.Last().Name; }
                        else
                        { FilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Trim('\\') + "\\" + Values.Last().Name; }
                        if (Values.Count >= 1)
                        { Values.RemoveAt(Values.Count - 1); }
                    }
                }
                else
                { FilePath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Trim('\\') + "\\Default.CSV"; }
            }
        }
    } // EndClass: ArgsObject

    public static class Q
    { //"<RowLimit>1</RowLimit><ViewFields><FieldRef Name='ID' /></ViewFields><QueryOptions /></View>"
        #region Basic CAML Query - Opening Types
        public static string oQuery = "<Query>";
        public static string oWhere = "<Where>";
        public static string oAnd = "<And>";
        public static string oOr = "<Or>";
        public static string oBeginsWith = "<BeginsWith>";
        public static string oContains = "<Contains>";
        public static string oDataRangesOverlap = "<DataRangesOverlap>";
        public static string oEq = "<Eq>";
        public static string oGeq = "<Geq>";
        public static string oGt = "<Gt>";
        public static string oIn = "<In>";
        public static string oIncludes = "<Includes>";
        public static string oIsNotNull = "<IsNotNull>";
        public static string oIsNull = "<IsNull>";
        public static string oLeq = "<Leq>";
        public static string oLt = "<Lt>";
        public static string oNeq = "<Neq>";
        public static string oNotIncludes = "<NotIncludes>";
        public static string oMonth = "<Month>";
        public static string oNow = "<Now>";
        public static string oUserID = "<UserID>";
        public static string oXML = "<XML>";
        public static string oValues = "<Values>";
        public static string oViewFields = "<ViewFields>";
        public static string oView = "<View>";

        public static string oToday(int DayOffset = 0) { return DayOffset == 0 ? "<Today>" : "<Today Offset = '" + DayOffset + "'>"; }
        public static string oGroupBy(bool Collapse = false) { return Collapse == false ? "<GroupBy>" : "<GroupBy Collapse = 'TRUE'>"; }
        public static string oOrderBy(Options options = Options.Default)
        {
            switch (options)
            {
                case Options.Both: return "<OrderBy Override = 'TRUE' UseIndexForOrderBy = 'TRUE'>";
                case Options.Override: return "<OrderBy Override = 'TRUE'>";
                case Options.UseIndexForOrderBy: return "<OrderBy UseIndexForOrderBy = 'TRUE'>";
                default: return "<OrderBy>";
            }
        }
        #endregion

        #region Basic CAML Query - Closing Types
        public static string cQuery = "</Query>";
        public static string cWhere = "</Where>";
        public static string cAnd = "</And>";
        public static string cOr = "</Or>";
        public static string cBeginsWith = "</BeginsWith>";
        public static string cContains = "</Contains>";
        public static string cDataRangesOverlap = "</DataRangesOverlap>";
        public static string cEq = "</Eq>";
        public static string cGeq = "</Geq>";
        public static string cGt = "</Gt>";
        public static string cIn = "</In>";
        public static string cIncludes = "</Includes>";
        public static string cIsNotNull = "</IsNotNull>";
        public static string cIsNull = "</IsNull>";
        public static string cLeq = "</Leq>";
        public static string cLt = "</Lt>";
        public static string cNeq = "</Neq>";
        public static string cNotIncludes = "</NotIncludes>";
        public static string cMonth = "</Month>";
        public static string cNow = "</Now>";
        public static string cUserID = "</UserID>";
        public static string cXML = "</XML>";
        public static string cValues = "</Values>";
        public static string cViewFields = "</ViewFields>";
        public static string cView = "</View>";

        // These have basic static class "Opening" versions
        public static string cToday = "</Today>";
        public static string cGroupBy = "</GroupBy>";
        public static string cOrderBy = "</OrderBy>";

        // These have advanced instantiated class "Opening" versions        
        public static string cFieldRef = "</FieldRef>";
        public static string cValue = "</Value>";
        public static string cMembership = "</Membership>";
        public static string cListProperty = "</ListProperty>";
        #endregion

        #region Support Enums
        public enum fTypes { Default, Average, Count, Max, Min, Sum, StdDeviation, Variance }
        public enum Members { Default, Custom, SPWeb_AllUsers, SPGroup, SPWeb_Groups, CurrentUserGroups, SPWeb_Users }
        public enum Options { Default, Both, Override, UseIndexForOrderBy }
        #endregion

        #region Advanced CAML Elements
        public static string oMembership(Members e = Members.Default, string CustomMemberGroup = "")
        {
            string ret = "<Membership>";
            if (CustomMemberGroup != "") { e = Members.Custom; }
            switch (e)
            {
                case Members.SPWeb_AllUsers: ret = ret.Replace(">", " Type='SPWeb.AllUsers'>"); break;
                case Members.SPGroup: ret = ret.Replace(">", " Type='SPGroup'>"); break;
                case Members.SPWeb_Groups: ret = ret.Replace(">", " Type='SPWeb.Groups'>"); break;
                case Members.CurrentUserGroups: ret = ret.Replace(">", " Type='CurrentUserGroups'>"); break;
                case Members.SPWeb_Users: ret = ret.Replace(">", "Type='SPWeb.Users'>"); break;
                case Members.Custom: ret = ret.Replace(">", "Type='" + CustomMemberGroup.Trim() + "'>"); break;
            }
            return ret;
        }


        public class oListProperty
        {
            private string ret = "<ListProperty>";
            public oListProperty() { }
            public override string ToString() { return ret; }
            public static string Standard = "<ListProperty>";

            public oListProperty AutoHyperLink(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " AutoHyperLink='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty AutoHyperLinkNoEncoding(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " AutoHyperLinkNoEncoding='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty AutoNewLine(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " AutoNewLine='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty Default(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Default='" + val + "'>"); } return this; }
            public oListProperty ExpandXML(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " ExpandXML='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty HTMLEncode(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " HTMLEncode='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty Select(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Select='" + val + "'>"); } return this; }
            public oListProperty StripWS(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " StripWS='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty URLEncode(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " URLEncode='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oListProperty URLEncodeAsURL(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " URLEncodeAsURL='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
        } // EndClass: oListProperty


        public class oValue
        {
            private string ret = "<Value>";
            public oValue() { }
            public override string ToString() { return ret; }
            public static oValue Typed(string type = "Text") { return new oValue().Type(type); }

            public oValue IncludeTimeValue(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " IncludeTimeValue='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oValue Type(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Type='" + val + "'>"); } return this; }
        } // EndClass: oValue


        public class oFieldRef
        {
            private string ret = "<FieldRef>";
            public oFieldRef(fTypes e = fTypes.Default)
            {
                switch (e)
                {
                    case fTypes.Average: ret = ret.Replace(">", " Type='AVG'>"); break;
                    case fTypes.Count: ret = ret.Replace(">", " Type='COUNT'>"); break;
                    case fTypes.Max: ret = ret.Replace(">", " Type='MAX'>"); break;
                    case fTypes.Min: ret = ret.Replace(">", " Type='MIN'>"); break;
                    case fTypes.Sum: ret = ret.Replace(">", " Type='SUM'>"); break;
                    case fTypes.StdDeviation: ret = ret.Replace(">", " Type='STDEV'>"); break;
                    case fTypes.Variance: ret = ret.Replace(">", " Type='VAR'>"); break;
                    default: break;
                }
            }
            public override string ToString() { return ret; }
            public static oFieldRef Named(string name) { return new oFieldRef().Name(name); }

            public oFieldRef Alias(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Alias='" + val + "'>"); } return this; }
            public oFieldRef Ascending(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Ascending='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oFieldRef CreateURL(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " CreateURL='" + val + "'>"); } return this; }
            public oFieldRef DisplayName(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " DisplayName='" + val + "'>"); } return this; }
            public oFieldRef Explicit(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Explicit='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oFieldRef Format(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Format='" + val + "'>"); } return this; }
            public oFieldRef ID(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " ID='" + val + "'>"); } return this; }
            public oFieldRef Key(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Key='" + val + "'>"); } return this; }
            public oFieldRef List(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " List='" + val + "'>"); } return this; }
            public oFieldRef LookupId(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " LookupId='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
            public oFieldRef Name(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " Name='" + val + "'>"); } return this; }
            public oFieldRef RefType(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " RefType='" + val + "'>"); } return this; }
            public oFieldRef ShowField(string val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " ShowField='" + val + "'>"); } return this; }
            public oFieldRef TextOnly(bool val)
            { if (ret.Contains('/') == false) { ret = ret.Replace(">", " TextOnly='" + (val == true ? "TRUE" : "FALSE") + "'>"); } return this; }
        } // EndClass: FieldRef
        #endregion

    } // EndClass Q

    #endregion CustomConstructs
   
} // EndNamespace


