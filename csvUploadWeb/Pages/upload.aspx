<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx"  Inherits="csvUploadWeb.Pages.upload" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SharePoint CSV Upload</title>
    <script src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" type="text/javascript"></script>
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js"></script>      
    <script src="../Scripts/jquery-ui-1.12.1.custom/jquery-ui.js"></script>
    <script type="text/javascript" src="../Scripts/Timer.js"></script>
    <script src="../Scripts/app.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
    
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>


    </head>

    <body>

     <form id="frm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>

    <!-- Chrome control placeholder -->
     <div id="chrome_ctrl_placeholder"></div>

     <!-- The chrome control also makes the SharePoint Website stylesheet available to your page -->
     <h1 class="ms-accentText"></h1>

    <div id="Instructions">  
        <a href="../instructions.html" onclick="popup(this.href); return false" >Instructions</a>
    </div>


     <div id="msgHtml" class="msg"></div>

     <div id="MainContent">
                    <div class="tabContainer">
                        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel" runat="server"  > 
                                <ProgressTemplate>      
                                     <div id="progress" style="text-align:center;">
                                           <img src="../Images/ajax-loader.gif" id="imgProgress"  /><br />
                                           <span id="sw_h">00</span>:
                                           <span id="sw_m">00</span>:
                                           <span id="sw_s">00</span>:
                                           <span id="sw_ms">00</span>
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            

                            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="btnStart" runat="server" Text="Process CSV" OnClick="Button1_Click" CssClass="w3-btn w3-blue w3-large w3-round"  OnClientClick=""  /><asp:Label ID="lblMessage" runat="server"></asp:Label><br />
                                    <asp:Button ID="btnKill" runat="server" Text="Stop Process" OnClick="KillProcess" CssClass="w3-btn w3-red w3-large w3-round" OnClientClick="cancelPostBack();"  />
                             
                                </ContentTemplate>
                            </asp:UpdatePanel>         

                            <table id="frmtbl">
                                <tr>
                                    <td>File Path (must be CSV)<span style="color:red;">*</span></td>
                                    <td>
                                        <asp:TextBox ID="csvFile" runat="server" Width="600" ></asp:TextBox> 
                                        <!--<ajaxToolkit:TextBoxWatermarkExtender runat="server" TargetControlID="csvFile"  WatermarkText="File path (C:\\users\\temp\\load.csv)" id="wmCsvFile" />-->
                                            <div id="csvMsg">
                                              
                                                    <small>Provide local ("C:\temp\upload.csv") or server path ("\\kineticsys.sharepoint.com\sites\projects\construction\nocal\0000789631\Documents\upload.csv").  File is not uploaded or selected.</small>
                                              
                                            </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%;">
                                        Action <small>(Record transactions based on "Title" field)</small></td>
                                    <td>

                                        <asp:RadioButtonList ID="rbAction" runat="server" CellPadding="0" CellSpacing="0" Width="800px">
                                            <asp:ListItem Value="AddNew"  Text="<span class='bimg'><b>New Records</b>   (Adds entry if it doesnt already exist in list)</span>" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="AddAll"  Text="<span class='bimg'><b>All records</b>   (Force the creation of a brand new entry even if it already exists. Duplicates some records.)</span>"></asp:ListItem>
                                            <asp:ListItem Value="Update"  Text="<span class='bimg'><b>Update</b>        (Add an entry if it doesn't exist, update if it does exist.)</span>"></asp:ListItem>
                                            <asp:ListItem Value="Delete"  Text="<span class='bimg'><b>Delete</b>        (Delete records if exists from CSV - Rolback.)</span>"></asp:ListItem>
                                            <asp:ListItem Value="DeleteAll" Text="<span class='aimg'><b>Delete All</b>     (Delete all list records)</span>"></asp:ListItem>
                                        </asp:RadioButtonList>
                                </td>
                                </tr>
                                <tr>
                                    <td>Target List <span style="color:red;">*</span></td>
                                    <td>
                                        <asp:RadioButtonList ID="ddTargetList" runat="server"  TextAlign="Left"  RepeatDirection="Horizontal">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                                            <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                                            <asp:ListItem Value="RSP" Enabled="False"></asp:ListItem> 
                                            <asp:ListItem Value="Components"  Enabled="False"></asp:ListItem> 
                                        </asp:RadioButtonList>
                                        <small>Currently only ITL is available but more will be added</small>
                                    </td>
                                </tr>
                                  <tr><td>Project:<span style="color:red;"></span></td><td><asp:DropDownList ID="ddlProjName" runat="server"></asp:DropDownList></td></tr>
                                  <tr><td>Project Site:<span style="color:red;">*</span></td><td><asp:TextBox ID="txtProjURL" runat="server" Width="600"></asp:TextBox></td></tr>
                           </table>
                        </div>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                    </div>

                </form> 
            </body>
    </html>