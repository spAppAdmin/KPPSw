<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx.cs"  Inherits="csvUploadWeb.Pages.upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SharePoint CSV Upload</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js"></script>  
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js"></script>
    <script src="../Scripts/app.js"></script>
    <link href="../Scripts/jquery-asProgress-master/dist/css/asProgress.css" rel="stylesheet" />
    <link href="../Scripts/jquery-asProgress-master/examples/css/normalize.css" rel="stylesheet" />
    <script src="../Scripts/jquery-asProgress-master/dist/jquery-asProgress.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
    
    <script>
 
  </script>
    
    <script type="text/javascript">
    var hostweburl;

    // Load the SharePoint resources.
    $(document).ready(function () {

        // Get the URI decoded add-in web URL.
        hostweburl =
            decodeURIComponent(getQueryStringParameter("SPHostUrl")
        );

        // The SharePoint js files URL are in the form:
        // web_url/_layouts/15/resource.js
        var scriptbase = hostweburl + "/_layouts/15/";

        // Load the js file and continue to the 
        // success handler.
        $.getScript(scriptbase + "SP.UI.Controls.js")
    });

    // Function to retrieve a query string value.
    function getQueryStringParameter(paramToRetrieve) {
        var params =
            document.URL.split("?")[1].split("&amp;");
        var strParams = "";
        for (var i = 0; i < params.length; i = i + 1) {
            var singleParam = params[i].split("=");
            if (singleParam[0] == paramToRetrieve)
                return singleParam[1];
        }
    }
</script>


        
 </head>

    <body>
      <form id="frm" runat="server">
            
            <div 
                id="chrome_ctrl_container"
                data-ms-control="SP.UI.Controls.Navigation"  
                data-ms-options=
                    '{  
                        "appHelpPageUrl" : "Help.aspx",
                        "appIconUrl" : "../images/upload2.png",
                        "appTitle" : "SharePoint CSV Uploader",
                        "settingsLinks" : [
                            {
                                "linkUrl" : "Account.aspx",
                                "displayName" : "Account settings"
                            },
                            {
                                "linkUrl" : "Contact.aspx",
                                "displayName" : "Contact us"
                            }
                        ]
                     }'>
            </div>



<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel1" runat="server"  > 
    <ProgressTemplate>       
        <img alt="progress" src="../images/loader1.gif" width="" height="100" style="position:absolute; right:400px; top:400px;"/>
    </ProgressTemplate>
</asp:UpdateProgress>
       
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always" ClientIDMode="AutoID"  ValidateRequestMode="Enabled">
<ContentTemplate>
    <asp:Label ID="lblText" runat="server" Text=""></asp:Label>
    <br />

    <asp:Button ID="Button1" runat="server" Text="Upload File" OnClick="Button1_Click1" />

    
    <asp:Label ID="msg" runat="server" Text="" ForeColor="red"></asp:Label>


    <table id="frmtbl" style="font-family: Calibri; font-size: medium; font-weight: normal; color: #3366CC; table-layout: auto; margin-top: 10px; border-collapse: collapse; border-spacing: 1px; empty-cells: show">
        <tr>
        <td>File Path (must be CSV)<span style="color:red;">*</span></td>
        <td><input type="text" id="csvFile" runat="server" value="C:\temp\ITL.csv" /></td></tr>
        <tr><td>Target List<span style="color:red;">*</span></td><td>
            <asp:RadioButtonList ID="ddTargetList" runat="server">
                <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
            </asp:RadioButtonList>
            </td></tr>
            <tr><td>Project:<span style="color:red;">*</span></td><td><asp:DropDownList ID="ddlProjName" runat="server" Width="600"></asp:DropDownList></td></tr>
            <tr><td>Project Site:</td><td><asp:TextBox ID="txtProjURL"  runat="server"  Width="600"></asp:TextBox></td></tr>
        c                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    <tr><td>#Records:</td><td><asp:TextBox ID="txtRecords"  runat="server"></asp:TextBox></td></tr>
            <tr><td>Last Run Date:</td><td><asp:TextBox ID="txtLastRun"  runat="server"></asp:TextBox></td></tr>
   </table>

</ContentTemplate>
</asp:UpdatePanel>        


<section>
    <h2>Progress Bars</h2>
    <section>
      <div class="row">
        <div class="example">
          <h4>HTML</h4>
          <pre><code data-language="html"></code></pre>
        </div>
        <div class="show">
          <h4>RENDERED HTML</h4>
          <div class="progress" role="progressbar" data-goal="30">
            <div class="progress__bar" style="width: 30%"></div>
          </div>
          <div class="progress" role="progressbar" data-goal="-50" aria-valuemin="-100" aria-valuemax="0">
            <div class="progress__bar"><span class="progress__label"></span></div>
          </div>
          <div class="progress" role="progressbar" data-goal="60" aria-valuemin="0" aria-valuemax="100">
            <div class="progress__bar" style="width: 50%"></div>
          </div>
        </div>
        <div>
          <button id="button_start">start()</button>
          <button id="button_stop">stop()</button>
          <button id="button_go">go('50')</button>
          <button id="button_go_percentage">go('50%')</button>
          <button id="button_finish">finish()</button>
          <button id="button_reset">reset()</button>
        </div>
      </div>
    </section>
  </section>

          <script type="text/javascript">
              jQuery(function ($) {


                    $().asProgress('start');



      $('.progress').asProgress({
        'namespace': 'progress'
      });

        $().asProgress('start');
        /*
      $('#button_start').on('click', function() {
        $('.progress').asProgress('start');
      });
      $('#button_finish').on('click', function() {
        $('.progress').asProgress('finish');
      });
      $('#button_go').on('click', function() {
        $('.progress').asProgress('go', 50);
      });
      $('#button_go_percentage').on('click', function() {
        $('.progress').asProgress('go', '50%');
      });
      $('#button_stop').on('click', function() {
        $('.progress').asProgress('stop');
      });
      $('#button_reset').on('click', function() {
        $('.progress').asProgress('reset');
      });*/
    });
  </script>

            
</form> 
 </body>
    </html>
