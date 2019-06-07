using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.SharePoint.Client;
using SPHelpers;
using SPA = SPHelpers.QueryAssistants;
using SPL = SPHelpers.GeneralLogging;


namespace uploadCSV
{

    class Program
    {
        internal static string usr = "SpAppAdmin@kinetics.net";
        internal static string pw = "West3451";
        internal static ClientContext ctx;
        internal static ArgsObject AO;

        #region State Management - Enables a minimized state of the console window
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        [DllImport("Kernel32")]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern short GetAsyncKeyState(int vkey);
        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        const int SW_SHOWMINIMIZED = 2;
        const int SW_SHOWDEFAULT = 10;
        const int VK_RCONTROL = 0xA3;
        const int VK_RSHIFT = 0xA1;
        #endregion


        static void Main(string[] args)
        {
            AO = new ArgsObject(args.ToList());

            #region State Management - Enables a minimized state of the console window
            IntPtr hwnd;
            hwnd = GetConsoleWindow();
            // Enables a minimized & invisible state for the console window            
            if (AO.State != StateOptions.Show)
            {
                if (AO.State == StateOptions.Hide)
                { ShowWindow(hwnd, SW_HIDE); }
                else
                { ShowWindow(hwnd, SW_SHOWMINIMIZED); }
            }

            // Added better visual representation of Arguments for end user troubleshooting
            for (int i = 0; i < args.Length; i++)
            { Console.WriteLine(args[i].ToString()); }

            // Holding the Right-Control key pauses operation so the user can review the arguments.
            if (GetAsyncKeyState(VK_RCONTROL) != 0 && GetAsyncKeyState(VK_RSHIFT) != 0)
            {
                ShowWindow(hwnd, SW_SHOWDEFAULT);
                Console.ReadLine();
            }
            #endregion

            try
            {
                ctx = new ClientContext(AO.URL);
                var passWord = new SecureString();
                foreach (char c in pw.ToCharArray()) passWord.AppendChar(c);
                ctx.Credentials = new SharePointOnlineCredentials(usr, passWord);
                Web web = ctx.Web;
                ctx.Load(web);
                ctx.ExecuteQuery();
                ctx.RequestTimeout = 500 * 300;
                switch (AO.Command)
                {
                    // This will force the creation of a brand new entry even if it already exists.
                    case "ADD":
                        SP_Update(true); break;

                    // This will add an entry if it doesn't exist, but will only update if it does exist.
                    case "UPDATE":
                        SP_Update(); break;

                    // This will delete 1 or MANY entries in the list
                    case "DELETE":
                        SP_Delete(); break;

                    // Dumps a CSV of requested data - work in progress....
                    case "READ":
                        SP_Reader(); break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(" Msg: " + ex.Message + " Soource: " + ex.Source);
                SPL.WriteExceptionToLog(ex);
            }
            SPL.WriteStatusHistoryToLog();
        }//End Main




        #region SharePoint Workers

        public static void SP_Update(bool ForceCreate = false)
        {
            string message = AO.PrimaryKey.Name + "-" + AO.PrimaryKey.Value + "-" + " Update Failed" + Environment.NewLine;
            var list = ctx.Web.Lists.GetByTitle(AO.Table);

            #region Find REAL Column Names            
            AO.PrimaryKey.TrueName = SPA.getInternalFieldName(AO.Table, AO.PrimaryKey.Name, ctx);
            foreach (ColumnName item in AO.Values)
            { item.TrueName = SPA.getInternalFieldName(AO.Table, item.Name, ctx); }
            #endregion

            int EID = SPA.getListItemID(list, AO.PrimaryKey.Value, AO.PrimaryKey.TrueName, ctx);
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem lstItem;
            if (EID == 0 || ForceCreate == true)
            {
                lstItem = list.AddItem(itemCreateInfo);
                lstItem.ParseAndSetFieldValue(AO.PrimaryKey.TrueName, AO.PrimaryKey.Value);
                EID = lstItem.Id;
            }
            else
            {
                EID = SPA.getListItemID(list, AO.PrimaryKey.Value, AO.PrimaryKey.TrueName, ctx);
                lstItem = list.GetItemById(EID);
            }
            foreach (ColumnName item in AO.Values)
            {
                if (item.TrueName.Length >= 1)
                {
                    lstItem.ParseAndSetFieldValue(item.TrueName, item.Value);
                    message = AO.PrimaryKey.Name + "-" + AO.PrimaryKey.Value + "-" + " Updated Successfully" + Environment.NewLine;
                }
                else
                { message = item.Name + "-" + item.Value + "-" + " Real Name Lookup Failed" + Environment.NewLine; }
                lstItem.Update();
            }
            ctx.ExecuteQuery();
            SPL.Output.Add(message);
        } // EndMethod: SP_Update


        public static void SP_Delete()
        {
            string message = AO.PrimaryKey.Name + "-" + AO.PrimaryKey.Value + "-" + " Delete Failed" + Environment.NewLine;
            ListItem Entry;
            var list = ctx.Web.Lists.GetByTitle(AO.Table);
            AO.PrimaryKey.TrueName = SPA.getInternalFieldName(AO.Table, AO.PrimaryKey.Name, ctx);

            // Includes a delete on the primary key's | seperated value
            if (AO.PrimaryKey.Name != "" && AO.PrimaryKey.Value != "")
            { AO.Values.Add(new ColumnName(AO.PrimaryKey.Name, AO.PrimaryKey.Value)); }

            foreach (var item in AO.Values)
            {
                int EID = 0;
                if (item.Name != "" && item.Value == "")
                { EID = SPA.getListItemID(list, item.Name, AO.PrimaryKey.TrueName, ctx); }
                else if (item.Name != "" && item.Value != "")
                { EID = SPA.getListItemID(list, item.Value, AO.PrimaryKey.TrueName, ctx); }
                if (EID != 0 && EID != -1)
                {
                    Entry = list.GetItemById(EID);
                    Entry.DeleteObject();
                    message = AO.PrimaryKey.Name + "-" + AO.PrimaryKey.Value + "-" + " Deleted Successfully" + Environment.NewLine;
                }
            }
            ctx.ExecuteQuery();
            SPL.Output.Add(message);
        } // EndMethod: SP_Delete

        public static void SP_Reader()
        {
            // need to research

            var list = ctx.Web.Lists.GetByTitle(AO.Table);
            AO.PrimaryKey.TrueName = SPA.getInternalFieldName(AO.Table, AO.PrimaryKey.Name, ctx);
            foreach (ColumnName item in AO.Values)
            { item.TrueName = SPA.getInternalFieldName(AO.Table, item.Name, ctx); }
            AO.Values.Insert(0, AO.PrimaryKey);

            CamlQuery cq = new CamlQuery();
            cq.ViewXml = @"<Query><OrderBy><FieldRef Name='Title'/></OrderBy></Query>";
            ListItemCollection lic = list.GetItems(cq);
            ctx.Load(lic);
            ctx.ExecuteQuery();
            List<string> Lines = new List<string>();
            foreach (var item in lic)
            {
                string str = "";
                foreach (var entry in AO.Values)
                { str = str.TrimStart() + "\t" + entry.Name + "|" + item.FieldValues[entry.TrueName]; }
                if (str != "")
                { Lines.Add(str); }
            }
            if (Lines.Count >= 1)
            { System.IO.File.WriteAllLines(AO.OutputPath, Lines); }
            SPL.Output.Add(AO.PrimaryKey.Name + "-" + AO.PrimaryKey.Value + "-" + " Read Successfully" + Environment.NewLine);
        }
        #endregion




        #region CustomConstructs

        public class ColumnName
        {
            public string Name, TrueName, Value;
            public ColumnName(string n, string v) { Name = n; TrueName = ""; Value = v; }
        } // EndClass: ColumnName

        public enum StateOptions
        {
            Show,
            Minimize,
            Hide
        }

        public class ArgsObject
        {
            public string Command, URL, Table, OutputPath;
            public StateOptions State = StateOptions.Minimize;
            public ColumnName PrimaryKey;
            public List<ColumnName> Values = new List<ColumnName>();
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

                    if (new string[] { "ADD", "UPDATE", "DELETE", "READ" }.Contains(args[0].ToUpper().Trim()) == true)
                    { Command = args[0].ToUpper().Trim(); }
                    else
                    { Command = "ERROR"; }

                    if (args[1].ToUpper().Trim().StartsWith("HTTP") == true)
                    { URL = args[1].Trim(); }

                    if (args[2].Trim().Length >= 1)
                    { Table = args[2].Trim(); }

                    if (args[3].Contains("|") == true)
                    { PrimaryKey = new ColumnName(args[3].Split('|')[0].Trim(), args[3].Split('|')[1]); }
                    else if (args[3].Trim().Lenagth >= 1)
                    { PrimaryKey = new ColumnName(args[3].Trim(), ""); }

                    if (args.Count > 4)
                    {
                        for (int i = 4; i < args.Count; i++)
                        {
                            if (args[i].Contains("|") == true)
                            {
                                string[] pairs = args[i].Split('|');
                                Values.Add(new ColumnName(pairs[0].Trim(), pairs[1].Trim()));
                            }
                            else if (args[i].Trim().Length >= 1)
                            { Values.Add(new ColumnName(args[i].Trim(), "")); }
                        }
                    }

                    if (Command == "READ")
                    {
                        if (Values.Last().Value != "")
                        {
                            OutputPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Trim('\\') + "\\Default.CSV";
                        }
                        else
                        {
                            if (System.IO.Path.IsPathRooted(Values.Last().Name) == true)
                            { OutputPath = Values.Last().Name; }
                            else
                            { OutputPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Trim('\\') + "\\" + Values.Last().Name; }
                            if (Values.Count >= 1)
                            { Values.RemoveAt(Values.Count - 1); }
                        }
                    }
                    else
                    { OutputPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location).Trim('\\') + "\\Default.CSV"; }
                }
            }
        } // EndClass: ArgsObject
        #endregion




    }
}

