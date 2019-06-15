<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx"  Inherits="csvUploadWeb.Pages.upload" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>




<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SharePoint CSV Upload</title>
    
    <script src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" type="text/javascript"></script>
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js"></script>      
    <script src="../Scripts/app.js"></script>
    <script src="../Scripts/jquery.lineProgressbar.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
    <link href="../Scripts/jquery.lineProgressbar.css" rel="stylesheet" />
    </head>

    <body>
      <form id="frm" runat="server">
    


          <!-- Chrome control placeholder -->
     <div id="chrome_ctrl_placeholder"></div>

     <!-- The chrome control also makes the SharePoint Website stylesheet available to your page -->
     <h1 class="ms-accentText">Form</h1>

    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <div id="msgHtml" class="msg"></div>
    
     <div id="MainContent">
         main

      
    <!-- ModalPopupExtender -->
    <ajaxToolkit:ModalPopupExtender ID="mp1" runat="server" PopupControlID="modalPanel" TargetControlID="btnShow" CancelControlID="btnClose" BackgroundCssClass="modalBackground"></ajaxToolkit:ModalPopupExtender>


    <asp:Panel ID="modalPanel" runat="server" CssClass="modalPopup" align="center" style = "display:none" Width="600" Height="600" BackColor="SkyBlue" BorderStyle="Double" BorderColor="Red" BorderWidth="3" >
    
    <ajaxToolkit:TabContainer ID="tabs" runat="server" ActiveTabIndex="1"  >
             <ajaxToolkit:TabPanel HeaderText="Instructions" ID="tabInstructions" runat="server"  >
                 <HeaderTemplate>
                    Instructions
                 </HeaderTemplate>
                 <ContentTemplate>
         
                - Uploader based on the following <a href="https://kineticsys.sharepoint.com/sites/projects/construction/SPP/000000000/Lists/Test%20Document%20Libraries/main.aspx" target="_blank" >List</a><br />
                - All CSV Files must contain these fields  <a href="https://kineticsys.sharepoint.com/sites/projects/construction/SPP/000000000/Lists/Test%20Document%20Libraries/main.aspx" target="_blank" >CSV Example File </a><br />
            
                 </ContentTemplate>
             </ajaxToolkit:TabPanel>
             <ajaxToolkit:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                 <HeaderTemplate>
                     Upload
                 </HeaderTemplate>
                 <ContentTemplate>
                    <div id="uploadCtl">
                        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel" runat="server"   > 
                                <ProgressTemplate>      
                                     <div class="modal">
                                         <div class="center">
                                            <img src="../Images/ajax-loader.gif" id="progress" />
                                        </div>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
  
                        <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Button1_Click1" runat="server" Text="Process CSV" OnClick="Button1_Click" CssClass="btnProcess" />
                                    <asp:Label ID="msg" runat="server" Text=""></asp:Label>
                                    
                                </ContentTemplate>
                            </asp:UpdatePanel>         
                             
                        <table id="frmtbl">
                                <tr>
                                    <td>File Path (must be CSV)<span style="color:red;">*</span></td>
                                    <td><input type="text" id="csvFile" runat="server" value="C:\temp\ITL.csv" />&nbsp;&nbsp;</td>
                                </tr>
                                <tr>
                                    <td>Action (Record transactions based on "Title" field)</td>
                                    <td>
                                        <asp:RadioButtonList ID="rbAction" runat="server">
                                            <asp:ListItem Value="AddNew" Text="        &lt;span class=&quot;b&quot;&gt;New Records&lt;/span&gt;          (Only Records not in List but in CSV File added)"></asp:ListItem>
                                            <asp:ListItem Value="AddAll" Text="        &lt;span class=&quot;b&quot;&gt;All records&lt;/span&gt;          (All records in CSV added - May create duplicates)"></asp:ListItem>
                                            <asp:ListItem Value="Upate" Text="          &lt;span class=&quot;b&quot;&gt;Update &lt;/span&gt;              (Records in CSV file and List Updated.  New Records in CSV File Added)"></asp:ListItem>
                                            <asp:ListItem Value="Delete" Text="         &lt;span class=&quot;b&quot;&gt;Delete&lt;/span&gt;               (Records in CSV file and List Deleted - Rolback)"></asp:ListItem>
                                            <asp:ListItem Value="DeleteAll" Text="  &lt;span class=&quot;b&quot;&gt;Delete All (Admin)&lt;/span&gt;  (Delete all List Records.  No CSV File required)"></asp:ListItem>
                                        </asp:RadioButtonList>
            
                                    </td>
                                </tr>
                                <tr>
                                    <td>Target List<span style="color:red;">*</span></td>
                                    <td>
                                        <asp:RadioButtonList ID="ddTargetList" runat="server">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                                            <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                                            <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr><td>Project:<span style="color:red;">*</span></td><td><asp:DropDownList ID="ddlProjName" runat="server" Width="600px"></asp:DropDownList></td></tr>
                                <tr><td>Project Site:</td><td><asp:TextBox ID="txtProjURL"  runat="server"  Width="600px"></asp:TextBox></td></tr>
                           </table>
                    </div>
                 </ContentTemplate>
             </ajaxToolkit:TabPanel>

             <ajaxToolkit:TabPanel ID="TabPanel4" runat="server" HeaderText="TabPanel4">
             </ajaxToolkit:TabPanel>
         </ajaxToolkit:TabContainer>
   

    <asp:Button ID="btnClose" runat="server" Text="Close" />
</asp:Panel>
    

             <asp:Button ID="btnShow" runat="server" Text="Show Modal Popup" />
 



  


</div>

            
</form> 
 </body>
    </html>
