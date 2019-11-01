using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using Microsoft.SharePoint.Client;
using System.Configuration;
using System.Net;
using System.Web;
using System.IO;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using P = ProcessHelpers;
using Microsoft.Azure.Management.DataFactories.Models;
using Microsoft.Azure.Management.DataFactories.Runtime;



namespace ExcelRestAPI
{
    class kFactorLoad 
    {
        private static HttpWebResponse response;

        public static object Request { get; private set; }

        static void Main(string[] args)
        {
            try
            {
            //Delete KFactor List
                var kFactorList = "Project KPI Master K-Factor";
                ClientContext ctxFactor = P.QueryAssistants.getProjectSpCtx(new Uri("https://kineticsys.sharepoint.com/sites/IntranetPortal/adm/ETL"));
                List spList = ctxFactor.Web.Lists.GetByTitle(kFactorList);
                //DeleteListItem(spList, ctxFactor);
                truncateTable();

                //Get Projects from Project Catalog
                ClientContext ctxCatalog = P.QueryAssistants.getProjectSpCtx(new Uri("https://kineticsys.sharepoint.com/sites/projects"));
                List oList = ctxCatalog.Web.Lists.GetByTitle("KPPS Projects Catalog");

                 
                //CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='K_x002d_FactorInclude'/><Value Type='Boolean'>1</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title'/><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };
                CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='K_x002d_FactorInclude' /><Value Type='Boolean'>1</Value></Eq></Where> <OrderBy><FieldRef Name='Project_x0020_Name' Ascending='False' /></OrderBy></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><RowLimit>20</RowLimit><QueryOptions /></View>" };
                //CamlQuery camlQuery = new CamlQuery() { ViewXml = "<View><Query><Where><Eq><FieldRef Name='Title' /><Value Type='Text'>622000336</Value></Eq></Where></Query><ViewFields><FieldRef Name='Title' /><FieldRef Name='Proj_x0020_Site_x0020_URL'/><FieldRef Name='Project_x0020_Name'/></ViewFields><QueryOptions /></View>" };

                ListItemCollection sites = oList.GetItems(camlQuery);
                ctxCatalog.Load(sites);
                ctxCatalog.ExecuteQuery();

                foreach (ListItem site in sites)
                {
                    var hyperLink = (FieldUrlValue)site["Proj_x0020_Site_x0020_URL"];
                    string projSiteURL = hyperLink.Url.Replace("/SitePages/Home.aspx", "");
                    //string url = hyperLink.Url;
                    var projCatalogNum = site["Title"].ToString();
                    var prjCatalogName = site["Project_x0020_Name"].ToString();
                    var valid = "";

                 
                 //   getItems(oList, ctxCatalog);
               //     Console.WriteLine(prjCatalogName + "\r\n");

                    ClientContext ctxProj = P.QueryAssistants.getProjectSpCtx(new Uri(projSiteURL));
                    string ExcelRESTUrl = projSiteURL + "/_vti_bin/ExcelRest.aspx/pro/03%20Finance/03.04%20MP%20Plan-Productivity/Kinetics-MP%20Plan%20and%20Labor%20Productivity%20Tracker.xlsx/model/Ranges('KFRollup')?$format=html";
                    //string ExcelRESTUrl = "https://kineticsys.sharepoint.com/sites/projects/construction/midat/622000336/_vti_bin/ExcelRest.aspx/pro/03%20Finance/03.04%20MP%20Plan-Productivity/Kinetics-MP%20Plan%20and%20Labor%20Productivity%20Tracker.xlsx/model/Ranges('KFRollup')?$format=html";

                    var fileToTestFor = projSiteURL + "/pro/03%20Finance/03.04%20MP%20Plan-Productivity/Kinetics-MP%20Plan%20and%20Labor%20Productivity%20Tracker.xlsx";
                    bool fileExists = FileExists(fileToTestFor, projSiteURL);

                    if (fileExists)
                    {
                        
                        string result = ParseExcelData(ExcelRESTUrl, ctxProj, projCatalogNum);

                        //"622000336","AZ CHROM","1.00","1.00","0.00","0.00","62.0%","3/24/2019","3,569","2,196","1,373","2,196","0.62","#VALUE!",
                        //result  "\"622000336\",\"AZ CHROM\",\"1.00\",\"1.00\",\"0.00\",\"0.00\",\"62.0%\",\"3/24/2019\",\"3,569\",\"2,196\",\"1,373\",\"2,196\",\"0.62\",\"#VALUE!\","  string

                        string[] data = result.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

                        //Console.Write(projCatalogNum + "-" + prjCatalogName +  '\r');
                        //Console.Read();
                        
                        if (data.Length > 1)
                        {
                            var projNo = data[0].Replace("\"", "");
                            var projName = data[1].Replace("\"", "");
                            decimal oKFactor = parseDecimal(data[2].Replace("\"", ""));
                            decimal dKFactor = parseDecimal(data[3].Replace("\"", ""));
                            decimal sKFactor = parseDecimal(data[4].Replace("\"", ""));
                            decimal fKFactor = parseDecimal(data[5].Replace("\"", ""));
                            decimal pctCmpte = parseDecimal(data[6].Replace("\"", ""));

                            DateTime dtLstUptd;
                            string testDt1 = data[7].Replace("\"", "");
                            DateTime testDt2;
                            if (DateTime.TryParse(testDt1, out testDt2))
                            {
                                dtLstUptd = DateTime.Parse(testDt1);
                            }
                            else
                            {
                                dtLstUptd = DateTime.UtcNow;
                            }

                            //DateTime dtLstUptd = DateTime.Parse(data[7].Replace("\"", ""));
                            decimal currBudMh = parseDecimal(data[8].Replace("\"", ""));
                            decimal burnedMHs = parseDecimal(data[9].Replace("\"", ""));
                            decimal remainMHs = parseDecimal(data[10].Replace("\"", ""));
                            decimal earnedMHs = parseDecimal(data[11].Replace("\"", ""));
                            decimal fpKFactor = parseDecimal(data[12].Replace("\"", ""));
                            decimal cProjHrs = 0;
                            decimal fProjHrs = 0;

                            valid = "valid";
                            LoadSQLData(projCatalogNum, prjCatalogName, oKFactor, dKFactor, sKFactor, fKFactor, pctCmpte, dtLstUptd, currBudMh, burnedMHs, remainMHs, earnedMHs, fpKFactor, cProjHrs,fProjHrs, valid, hyperLink);
                            //AddNewListItem(projCatalogNum, prjCatalogName, oKFactor, dKFactor, sKFactor, fKFactor, pctCmpte, dtLstUptd, currBudMh, burnedMHs, remainMHs, earnedMHs, fpKFactor, cProjHrs,fProjHrs, valid);
                        }
                        else
                        {
                            valid = "File exists but no KFRollup Range";
                            LoadSQLData(projCatalogNum, prjCatalogName, 0, 0, 0, 0, 0, DateTime.Today, 0, 0, 0, 0, 0, 0, 0, valid, hyperLink);
                            //AddNewListItem(projCatalogNum, prjCatalogName, 0, 0, 0, 0, 0, DateTime.Today, 0, 0, 0, 0, 0, 0, 0, valid);
                        }
                    }
                    else
                    {
                        valid = "File does not exists";
                        LoadSQLData(projCatalogNum, prjCatalogName, 0, 0, 0, 0, 0, DateTime.Today, 0, 0, 0, 0, 0, 0, 0, valid, hyperLink);
                        //AddNewListItem(projCatalogNum, prjCatalogName, 0, 0, 0, 0, 0, DateTime.Today, 0, 0, 0, 0, 0, 0, 0, valid);
                    }
                    
                }

                        P.GeneralLogging.AddStatusLog("KSIFactor", "Completed Successfully");
                    }


            catch (Exception ex)
            {
                //Console.Write(ex.Message);
                
                //P.GeneralLogging.AddExceptionLog(ex, "KSIFactor");
            }
        }


        private static List<ListItem> getItems(List list, ClientContext ctx)
        {
            ListItemCollection items = list.GetItems(CamlQuery.CreateAllItemsQuery());
            ctx.Load(items, icol => icol.Include(i => i.DisplayName));
            ctx.ExecuteQuery();

            var result = new List<ListItem>();
            for (int i = 0, len = items.Count; i < len; i++)
            {
                result.Add(items[i]);
            }
            return result;
        }



        public static void LoadSQLData(string projNo, string projName, Decimal oKFactor, Decimal dKFactor, Decimal sKFactor, Decimal fKFactor, Decimal pctCmpte, DateTime dtLstUptd, Decimal currBudMh, Decimal burnedMHs, Decimal remainMHs, Decimal earnedMHs, Decimal fpKFactor, Decimal cProjhrs, Decimal fProjHrs, string comments, FieldUrlValue site)
        {

            DBContextWrapper dbcontext = new DBContextWrapper();
            DBContextWrapper trackingDBContext = new DBContextWrapper(ConfigurationManager.AppSettings["DBConnection"]);

            int success = 0;
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            try
            {
                success = dbcontext.InsertData(projNo, projName, oKFactor, dKFactor, sKFactor, fKFactor, pctCmpte, dtLstUptd, currBudMh, burnedMHs, remainMHs, earnedMHs, fpKFactor, cProjhrs, fProjHrs, comments, site);
            }
            catch (Exception ex)
            {

            }

            if (success == -1)
            {
                Console.WriteLine("Item ID: {0} Name: {1} Loaded to SQL Table Successfully.", projNo, projName);
            }
            else
            {
                Console.WriteLine("Item ID: {0} Name: {1} Did not Load.", projNo, projName);
            }
        }
        

        public static void truncateTable()
        {

            DBContextWrapper dbcontext = new DBContextWrapper();
            DBContextWrapper trackingDBContext = new DBContextWrapper(ConfigurationManager.AppSettings["DBConnection"]);

            int success = 0;

            try
            {
                success = dbcontext.truncateTable();
            }
            catch (Exception ex)
            {

            }

            if (success == 1)
            {
                Console.WriteLine("Table Truncated");
            }
        }

        public static bool FileExists(string fileUrl, string url)
        {
            ClientContext ctx = P.QueryAssistants.getProjectSpCtx(new Uri(url));
            Microsoft.SharePoint.Client.File file = ctx.Web.GetFileByUrl(fileUrl);
            bool bExists = false;
            try
            {
                ctx.Load(file);
                ctx.ExecuteQuery();
                bExists = file.Exists;
            }
            catch { }
            if (bExists)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static string ParseExcelData(string url, ClientContext ctx, string projNo)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Credentials = ctx.Credentials;
            req.Headers["X-FORMS_BASED_AUTH_ACCEPTED"] = "f";
            HttpWebResponse response;

            try
            {
                response = (HttpWebResponse)req.GetResponse();
            }
            catch (WebException ex)
            {
                HttpWebResponse webResponse = (HttpWebResponse)ex.Response;
                if (webResponse.StatusCode == HttpStatusCode.NotFound) //Excel range not found
                {
                    return projNo;   //"\"622000336\"|\"AZ CHROM\"|\"1.00\"|\"1.00\"|\"0.00\"|\"0.00\"|\"62.0%\"|\"3/24/2019\"|\"0\"|\"2,196\"|\"1,373\"|\"2,196\"|\"0.62\"|\"#VALUE!\";
                }
                else
                {
                    throw;
                }
            }


            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string str = reader.ReadToEnd();

            string txt = "</div></td></table></div>";

            string re1 = ".*?"; // Non-greedy match on filler
            string re2 = "(<\\/td>)";   // Tag 1

            Regex r = new Regex(re1 + re2, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            Match m = r.Match(txt);
            if (m.Success)
            {
                String tag1 = m.Groups[1].ToString();
            }

            //int last = str.LastIndexOf("</td>");
            //string proj = str.Substring(last-10, 10);

            string projh = chtml(str);
            string[] stringarr = new string[] { projh };

            return stringarr[0];
        }

        private static string chtml(string html)
        {
            var texts = GetTextsFromHtml(html);
            return texts;
        }

        private static string GetTextsFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            return GetTextsFromNode(htmlDoc.DocumentNode.ChildNodes);
        }

        private static string GetTextsFromNode(HtmlNodeCollection nodes)
        {
            string texts = "";
            foreach (var node in nodes)
            {
                if (node.Name.ToLowerInvariant() == "style")
                    continue;
                if (node.HasChildNodes)
                {
                    texts = texts + GetTextsFromNode(node.ChildNodes);
                    //texts = "\"" + texts + GetTextsFromNode(node.ChildNodes) + "\",";
                }
                else
                {
                    var innerText = node.InnerText;
                    if (!string.IsNullOrWhiteSpace(innerText))
                    {
                        if (node.Name.ToLowerInvariant() == "span")
                            texts = texts + " " + node.InnerText + "\n";
                        else
                            texts = "\"" + texts + node.InnerText + "\"|";

                    }
                }
            }
            return texts;
        }

        static string GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, " ");
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "xx", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }

        public static void AddNewListItem(string projNo, string projName, Decimal oKFactor, Decimal dKFactor, Decimal sKFactor, Decimal fKFactor, Decimal pctCmpte, DateTime dtLstUptd, Decimal currBudMh, Decimal burnedMHs, Decimal remainMHs, Decimal earnedMHs, Decimal fpKFactor, decimal cProjhrs, decimal fProjHrs, string comments)
        {
            try
            {
                ClientContext ctx = P.QueryAssistants.getProjectSpCtx(new Uri("https://kineticsys.sharepoint.com/sites/IntranetPortal/adm/ETL"));
                List oList = ctx.Web.Lists.GetByTitle("Project KPI Master K-Factor");
                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                ListItem oListItem = oList.AddItem(itemCreateInfo);

                oListItem["Title"] = projNo;
                oListItem["ProjectName"] = projName;
                oListItem["OverallK_x002d_Factor"] = oKFactor;
                oListItem["DetailingKFactor"] = dKFactor;
                oListItem["ShopK_x002d_Factor"] = sKFactor;
                oListItem["FieldK_x002d_Factor"] = fKFactor;
                oListItem["_x0025_Complete"] = pctCmpte;
                oListItem["DateLastUpdated"] = dtLstUptd;
                oListItem["CurrentBudget"] = currBudMh;
                oListItem["BurnedMHS"] = burnedMHs;
                oListItem["RemainingMHS"] = remainMHs;
                oListItem["EarnedMHS"] = earnedMHs;
                oListItem["FieldProjectedK_x002d_Factor"] = fpKFactor;
                oListItem["Computer_x0020_Projected_x0020_M"] = cProjhrs;
                oListItem["Field_x0020_Projected_x0020_MHs"] = fProjHrs;
                oListItem["Comments"] = comments;

                oListItem.Update();
                ctx.ExecuteQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        static Decimal parseDecimal(string n)
        {

            Decimal i = 0;
            Decimal rtn = 0;

            bool isConvertable = Decimal.TryParse(n, out i);

            if (isConvertable)
                rtn = Decimal.Parse(n);

            if (!isConvertable)
                rtn = 0;

            return rtn;

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

    }
}



/*
 * 
 * first try



    //str.Select<"td">
    //str.Remove(1000);

    //string str = "d";
    //char[] b = new char[str.Length];


    using (StringReader sr = new StringReader(str))
    {
      //  sr.Read(b,1,1);



    }

    dataStream.Close();


    //HtmlDocument doc = new HtmlDocument();
    //doc.Load(reader);
    // var elementsWithStyleAttribute = doc.DocumentNode.SelectNodes("//<td>");
    //foreach (var element in elementsWithStyleAttribute)
    //{
    //element.Attributes["style"].Remove();
    //}



    HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

    //var desc = Regex.Replace(htmlDoc, "(<style.+?</style>)|(<script.+?</script>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //desc = Regex.Replace(desc, "(<img.+?>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //desc = Regex.Replace(desc, "(<o:.+?</o:.+?>)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //desc = Regex.Replace(desc, "<!--.+?-->", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //desc = Regex.Replace(desc, "class=.+?>", ">", RegexOptions.IgnoreCase | RegexOptions.Singleline);
    //desc = Regex.Replace(desc, "class=.+?\s", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);


    //htmlDoc.LoadHtml(html);
    //var elem = htmlDoc.DocumentNode.SelectNodes("p/br");
    //htmlDoc.Load(responseFromServer);

    //htmlDoc.DocumentNode.RemoveClass();

    //var x = htmlDoc.DocumentNode.SelectNodes("//<td>");
asaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa            // Display the content.aaaaaaaaaaccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccccc
    //Console.WriteLine(responseFromServer);
    // Clean up the streams and the response.
    //reader.Close();
    //response.Close();

    }
    */











