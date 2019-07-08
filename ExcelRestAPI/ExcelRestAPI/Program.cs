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


namespace ExcelRestAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri uriProject = new Uri("https://kineticsys.sharepoint.com/sites/projects/construction/SPP/000000000");
            ClientContext ctx = getProjectSpCtx(uriProject);

      

            string url = "https://kineticsys.sharepoint.com/sites/projects/construction/midat/622000336/_vti_bin/ExcelRest.aspx/pro/03%20Finance/03.04%20MP%20Plan-Productivity/Kinetics-MP%20Plan%20and%20Labor%20Productivity%20Tracker.xlsx/model/Ranges('KFRollup')?$format=html";


     


            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.Credentials = ctx.Credentials;
            req.Headers["X-FORMS_BASED_AUTH_ACCEPTED"] = "f";
            HttpWebResponse response = (HttpWebResponse)req.GetResponse();

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
                Console.Write("(" + tag1.ToString() + ")" + "\n");
            }

          

            //int last = str.LastIndexOf("</td>");

            //string proj = str.Substring(last-10, 10);

            

            string projh = chtml(str);


            string[] stringarr = new string[] {projh};

            





        }



        static string chtml(string html)
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
                            texts = "\"" + texts + node.InnerText +"\"," ;
                    }
                }
            }

            
            return texts;
        }







        static string  GetPlainTextFromHtml(string htmlString)
        {
            string htmlTagPattern = "<.*?>";
            var regexCss = new Regex("(\\<script(.+?)\\</script\\>)|(\\<style(.+?)\\</style\\>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            htmlString = regexCss.Replace(htmlString, string.Empty);
            htmlString = Regex.Replace(htmlString, htmlTagPattern, " ");
            htmlString = Regex.Replace(htmlString, @"^\s+$[\r\n]*", "xx", RegexOptions.Multiline);
            htmlString = htmlString.Replace("&nbsp;", string.Empty);

            return htmlString;
        }




        /*


      
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













            // Display the content.
            //Console.WriteLine(responseFromServer);
            // Clean up the streams and the response.
            //reader.Close();
            //response.Close();

            }
            */


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






