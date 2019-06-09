<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx.cs"  Inherits="csvUploadWeb.Pages.upload" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SharePoint CSV Upload</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no"/>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js"></script>

    <script src="../Scripts/app.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
    
    
    
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
            <div id="errorMessagePlaceHolder"></div>
        </ContentTemplate>
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
                    <asp:ListItem Value="AddNew" Text="Add New" >Only new records added from CSV</asp:ListItem>
                    <asp:ListItem Value="AddAll" Text="Add All" >All records on CSV added even if pre-exisitng</asp:ListItem>
                    <asp:ListItem Value="Upate" Text="Update" >Existing records deleted and replaced from CSV</asp:ListItem>
                    <asp:ListItem Value="Delete" Text="Delete all list records" >Delete all list records</asp:ListItem>
                    <asp:ListItem Value="DeleteCSV" Text="Delete only records on CSV" ></asp:ListItem>
                    
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


          <div id="renderAnnouncements"></div>

            
</form> 
 </body>
    </html>
