<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx"  Inherits="csvUploadWeb.Pages.upload" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SharePoint CSV Upload</title>
    <script src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js" type="text/javascript"></script>
    <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/jQuery/jquery-1.9.1.min.js"></script>      
    <script src="../Scripts/app.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
    </head>

    <body>

     <form id="frm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" ></asp:ScriptManager>

              
     <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" PopupControlID="pnlPopup" TargetControlID="btnInstructions" BackgroundCssClass="modalBackground" CancelControlID="btnHide" DropShadow="false" RepositionMode="RepositionOnWindowResize" ViewStateMode="Enabled"   ></ajaxToolkit:ModalPopupExtender>
     <asp:Panel ID="pnlPopup" runat="server" CssClass="modalPopup" Width="600" Height="900"  HorizontalAlign="left">
              
                <h1>Instructions</h1>
                <ul id="listInstructions">
                    <li>List <a href="https://kineticsys.sharepoint.com/sites/projects/construction/SPP/000000000/Lists/Test%20Document%20Libraries/main.aspx?viewid=ad6bc77c%2Dafc8%2D47d2%2D96e1%2D8df38e233902" target="_blank" ><b> Structure</b></a>  CSV files <b>must contain</b> all fields in list (even if no data)</li>
                    <li>Sample CSV File structure   <a href="https://kineticsys.sharepoint.com/sites/IntranetPortal/adm/ETL/Documents/ITL.CSV"                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             ssssssssssssssssssssssssssssssssssssssssssssssssssssssssssss</q>target="_blank" >Valid CSV Example File </a><small></small></li>
                    <li>Note: these fields types are <b>not</b> valid in the CSV file and not updatable:
                        <ul>
                            <li>Calculated Fields</li>
                            <li>Other fields included in lookup (other than primary lookup field)</li>
                            <li>System Fields (Modified, Created, Modified By, Created By etc.)</li>
                        </ul>
                    </li>
                    <li>Date Fields should follow the dd/mm/yyyy format (e.g. 5/1/2019)</li>
                    <li>CSV Values must match the target list field type.  For example "T" in a numerically defined field will throw an error. </li>
                    <li>CSV Field Order is not neccessary but all fields need to be represented in the CSV file - even if the data is empty</li>
                </ul>
                <asp:Button ID="btnHide" runat="server" Text="Close" class="btnProcess" />
          
     </asp:Panel>
       

    <!-- Chrome control placeholder -->
     <div id="chrome_ctrl_placeholder"></div>

     <!-- The chrome control also makes the SharePoint Website stylesheet available to your page -->
     <h1 class="ms-accentText"></h1>
         



    <div id="Instructions">  
        <asp:Button runat="server" ID="btnInstructions" Text="Instructions" CssClass="btnProcess" /> 
    </div>


     <div id="msgHtml" class="msg"></div>

     <div id="MainContent">

        <div class="tabContainer">
                                                                                                                                                                                
                    <div id="uploadCtl">
                        <asp:UpdateProgress ID="updProgress" AssociatedUpdatePanelID="UpdatePanel" runat="server"   > 
                                <ProgressTemplate>      
                                     <div id="progress">
                                            <img src="../Images/ajax-loader.gif"  />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
  
                            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                                <ContentTemplate>
                                    <asp:Button ID="Button1_Click1" runat="server" Text="Process CSV" OnClick="Button1_Click" CssClass="btnProcess" Width="150" />
                                    <asp:Label ID="msg" runat="server" Text=""></asp:Label>
                                </ContentTemplate>
                            </asp:UpdatePanel>         

                        


                        <table id="frmtbl">
                                <tr>
                                    <td>File Path (must be CSV)<span style="color:red;">*</span></td>
                                    <td>
                                        <asp:TextBox ID="csvFile" runat="server" Width="300" ></asp:TextBox> 
                                        <ajaxToolkit:TextBoxWatermarkExtender runat="server" TargetControlID="csvFile"  WatermarkText="File path (C:\\users\\temp\\load.csv)" id="wmCsvFile" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width:30%;">
                                        Action <small>(Record transactions based on "Title" field)</small></td>
                                    <td>
                                        <asp:RadioButtonList ID="rbAction" runat="server" CellPadding="0" CellSpacing="0">
                                            <asp:ListItem Value="AddNew" Text="        &lt;span class=&quot;b&quot;&gt;New Records&lt;/span&gt;          (Only Records not in List but in CSV File added)" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="AddAll" Text="        &lt;span class=&quot;b&quot;&gt;All records&lt;/span&gt;          (All records in CSV added - May create duplicates)"></asp:ListItem>
                                            <asp:ListItem Value="Update" Text="          &lt;span class=&quot;b&quot;&gt;Update &lt;/span&gt;              (Records in CSV file and List Updated.  New Records in CSV File Added)"></asp:ListItem>
                                            <asp:ListItem Value="Delete" Text="         &lt;span class=&quot;b&quot;&gt;Delete&lt;/span&gt;               (Records in CSV file and List Deleted - Rolback)"></asp:ListItem>
                                            <asp:ListItem Value="DeleteAll" Text="  &lt;span class=&quot;b&quot;&gt;Delete All (Admin)&lt;/span&gt;  (Delete all List Records.  No CSV File required)"></asp:ListItem>
                                        </asp:RadioButtonList>
            
                                    </td>
                                </tr>
                                <tr>
                                    <td>Target List (currently only ITL is available but more will be added)<span style="color:red;">*</span></td>
                                    <td>
                                        <asp:RadioButtonList ID="ddTargetList" runat="server"  TextAlign="Left"  RepeatDirection="Horizontal">                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                                            <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                                            <asp:ListItem Value="RSP" Enabled="False"></asp:ListItem> 
                                            <asp:ListItem Value="Components"  Enabled="False"></asp:ListItem> 
                                        </asp:RadioButtonList>
                                       
                                    </td>
                                </tr>
                                  <tr><td>Project:<span style="color:red;"></span></td><td><asp:DropDownList ID="ddlProjName" runat="server"></asp:DropDownList></td></tr>
                                  <tr><td>Project Site:<span style="color:red;">*</span></td><td><asp:TextBox ID="txtProjURL" runat="server" Width="600"></asp:TextBox></td></tr>
          
                           </table>
                    </div>                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
                 
        </div>
     </div>

        
        <!--</asp:Panel>-->
    



    

            
</form> 
 </body>
    </html>