using System.Configuration;

namespace KSI_List_ReportProcessing
{
    class Constants
    {

        public static string sqlconstr = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public const string ArchivalListName = "Archival List";



        
        public const string OutputFileName = "{0}-{1}-{2}";
        public const string SUCCESS = "Success";
        public const string FAILURE = "Failure";
        public const string InsertCMD = "INSERT INTO [{0}] ({1}) VALUES ({2})";
        public const string SITEURL = "Site_x0020_URL";
        public const string LISTNAME = "List_x0020_Name";
        public const string VIEWNAME = "List_x0020_View_x0020_Name";
        public const string TABLECREATEDFLAG = "Table_x0020_Created";
        public const string DBNAME = "Database_x0020_Name";
        public const string DBUSERID = "Database_x0020_UserID";
        public const string DBUSERPSW = "Database_x0020_Password";
        public const string DBAEMAIL = "DBA_x0020_Email";
        public const string SUPPORTTEAMEMAIL = "Support_x0020_Team_x0020_Email";
        public const string TABLENAME = "Table_x0020_Name";
        public const string INSERTQUERY = "Insert_x0020_Query";
        public const string MODE = "Mode";
        public const string CASTIRONEMAILSERVICE = "CastIronEmailServiceUrl";
        public const string TEMPLATETYPE = "Template_x0020_Type";
        public const string FROM = "From1";
        public const string EMAILSUBJECT = "Email_x0020_Subject";
        public const string EMAILBODY = "Email_x0020_Body";
        public const string SIGNATURE = "Signature";
        public const string TO = "To";
        public const string BannerUrl = "Banner_x0020_Image_x0020_URL";
        public const string DDLOUTPUTPATH = "DDL_x0020_Output_x0020_Location";
        public const string CONFIGLIST = "Config";
        public const string EMAILSETTINGLIST = "Email Settings";

        public const int RequestTimeout = 100000;
        public const string RequestContentType = "text/json";
        public const string HttpPost = "POST";
        public const string ROWLIMIT = "5000";
        public const string DEFAULTFIELDS = "Author,Editor,Modified,Created";
    }
    public enum Mode
    {
        Online = 1,
        Offline = 2
    }
}
