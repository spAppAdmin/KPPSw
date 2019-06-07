<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="upload.aspx.cs" Inherits="csvUploadWeb.Pages.upload" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>UPLOAD</title>
    
      <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.8.1/jquery.min.js"></script>  
      <script type="text/javascript" src="//ajax.aspnetcdn.com/ajax/4.0/1/MicrosoftAjax.js"></script>
        <script src="../Scripts/app.js"></script>
    <link href="../Scripts/app.css" rel="stylesheet" />
        
 </head>

    <body>
        <form id="frm" runat="server">

   <table>
        <tr><td>File Path (must be CSV)</td><td><input type="text" id="File"/></td></tr>
        <tr><td>Target List</td><td>
            <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                <asp:ListItem Value="RSI"></asp:ListItem> 
            </asp:RadioButtonList>
            </td></tr>
            <tr><td>Project:</td><td><asp:DropDownList ID="ddlProjCat" runat="server"></asp:DropDownList></td></tr>
            <tr><td>Project Site:</td><td><asp:TextBox ID="txtProjCatURL"  runat="server"></asp:TextBox></td></tr>
            <tr><td>#Records:</td><td><asp:TextBox ID="txtRecords"  runat="server"></asp:TextBox></td></tr>
            <tr><td>Last Run Date:</td><td><asp:TextBox ID="txtLastRun"  runat="server"></asp:TextBox></td></tr>
   </table>

            <asp:Button ID="Button1" runat="server" Text="Upload File" OnClick="Button1_Click1" />
</form> 
 </body>
    </html>
