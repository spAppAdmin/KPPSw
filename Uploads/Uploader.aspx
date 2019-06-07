<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Uploader.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Uploads.Uploader" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>

        #frmtbl tr td {
            padding: 10px;
        }

    </style>

    <div>
        <h2>SharePoint CSV File Uploads</h2>
    
    <table id="frmtbl" style="font-family: Calibri; font-size: medium; font-weight: normal; color: #3366CC; table-layout: auto; margin-top: 60px; border-collapse: collapse; border-spacing: 2px; empty-cells: show">
        <tr><td>File Path (must be CSV)</td><td><input type="text" id="File"/></td></tr>
        <tr><td>Target List</td><td dir="auto">
            <asp:RadioButtonList ID="rbTargetList" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                <asp:ListItem Value="RSI"></asp:ListItem> 
            </asp:RadioButtonList>
            </td></tr>
            <tr><td>Project:</td><td><asp:DropDownList ID="ddlProjCat" runat="server" Width="800px"></asp:DropDownList></td></tr>
            <tr><td>Project Site:</td><td><asp:TextBox ID="txtProjCatURL"  runat="server" Width="800px" Wrap="False"></asp:TextBox></td></tr>
            <tr><td>#Records:</td><td><asp:TextBox ID="txtRecords"  runat="server" Width="200px"></asp:TextBox></td></tr>
            <tr><td>Last Run Date:</td><td><asp:TextBox ID="txtLastRun"  runat="server" Width="200px"></asp:TextBox></td></tr>
        </table>

            <br />
            <strong>
            <asp:Button ID="uploadFile" runat="server" Text="Upload CSV" style="font-size: large; background-color: #00CCFF;" CssClass="auto-style2" />
            </strong>
        </div>
</asp:Content>

