<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx.cs"  Inherits="csvUploadWeb.Pages.upload" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SharePoint CSV Upload</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js"></script>  
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js"></script>
<script type="text/javascript" src="/_layouts/15/sp.js"></script>
    <script src="../Scripts/app.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
    
     <script type="text/javascript" language="javascript">


      $(document).ready(function() {  
      //  SP.SOD.executeFunc('sp.js', 'SP.ClientContext', showModalPopUp);  
    });  

    function showModalPopUp() {  
        //Set options for Modal PopUp  
        var options = {  
            url: 'upload.aspx?IsDlg=1', //Set the url of the page  
            title: 'SharePoint Modal Pop Up', //Set the title for the pop up  
            allowMaximize: false,  
            showClose: true,  
            width: 600,  
            height: 400  
        };  
        //Invoke the modal dialog by passing in the options array variable  
        SP.SOD.execute('sp.ui.dialog.js', 'SP.UI.ModalDialog.showModalDialog', options);  
        return false;  
    }  
        </script>




    <script type="text/javascript">
    var hostweburl;

    // Load the SharePoint resources.
    $(document).ready(function () {

        // Get the URI decoded add-in web URL.
        hostweburl =    decodeURIComponent(getQueryStringParameter("SPHostUrl") );

        // The SharePoint js files URL are in the form:
        // web_url/_layouts/15/resource.js
        var scriptbase = hostweburl + "/_layouts/15/";

        // Load the js file and continue to the 
        // success handler.
        $.getScript(scriptbase + "SP.UI.Controls.js") });

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

<div>

    




    <asp:ScriptManager ID="ScriptManager1" runat="server"  ></asp:ScriptManager>
    
    <div id="msgHtml"></div>
    


<asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel" runat="server"  > 
        <ProgressTemplate>       
                 <img src="../Images/progress.gif" id="progress2"  runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
  
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:Button ID="Button1_Click1" runat="server" Text="Process CSV" OnClick="Button1_Click" CssClass="btnProcess"/>
            <asp:Label ID="msg" runat="server" Text="xxx"></asp:Label>
        </ContentTemplate>
    </asp:UpdatePanel>         
</div>

<div>



<asp:UpdatePanel id="ErrorUpdatePanel" runat="server" UpdateMode="Conditional">

</asp:UpdatePanel>
</div>

        

          
    <!--
        <ajaxToolkit:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" >
        <ajaxToolkit:TabPanel ID="TabPanel1" HeaderText="TabPanel1" runat="server">
            <ContentTemplate>T1</ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
         <ContentTemplate>T2</ContentTemplate>
        </ajaxToolkit:TabPanel>
        <ajaxToolkit:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
            <ContentTemplate>T3</ContentTemplate>
        </ajaxToolkit:TabPanel>
    </ajaxToolkit:TabContainer>
    -->


<div>

    <table id="frmtbl">
        <tr>
            <td>File Path (must be CSV)<span style="color:red;">*</span></td>
            <td><input type="text" id="csvFile" runat="server" value="C:\temp\ITL.csv" /></td>
        </tr>
        <tr>
            <td>Action</td>
            <td>
                <asp:RadioButtonList ID="rbAction" runat="server">
                    <asp:ListItem Value="Delete" Text="Delete all list records" ></asp:ListItem>
                    <asp:ListItem Value="Add" Text="Add only new records" ></asp:ListItem>
                    <asp:ListItem Value="Update" Text="Update Records" ></asp:ListItem>
                </asp:RadioButtonList>
            <span><small>Selecting deletes records with matching titles and replaces with new record</small></span>
            </td>
        </tr>
        <tr>
        <td class="auto-style2">Target List<span style="color:red;">*</span></td><td>
            <asp:RadioButtonList ID="ddTargetList" runat="server">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
            </asp:RadioButtonList>
            </td></tr>
            <tr><td class="auto-style2">Project:<span style="color:red;">*</span></td><td><asp:DropDownList ID="ddlProjName" runat="server" Width="600"></asp:DropDownList></td></tr>
            <tr><td class="auto-style2">Project Site:</td><td><asp:TextBox ID="txtProjURL"  runat="server"  Width="600"></asp:TextBox></td></tr>
   </table>

</div>


            
</form> 
 </body>
    </html>
