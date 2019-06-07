<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Uploader.aspx.cs" MasterPageFile="~/Site.Master" Inherits="Uploads.Uploader" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>

        #frmtbl tr td {
            padding: 10px;
        }

    
    </style>

    <div>
        <table>
            <tr>
                <td>
                    <img src="upload3.png" width="200px" height="200px" /></td>
                <td>
                    <h2 style="color: dodgerblue;">SharePoint CSV File Uploads</h2>
                </td>
            </tr>
        </table>
        
    </div>

    <div>
    <table id="frmtbl" style="font-family: Calibri; font-size: medium; font-weight: normal; color: #3366CC; table-layout: auto; margin-top: 50px; border-collapse: collapse; border-spacing: 1px; empty-cells: show">
        <tr><td>File Path (must be CSV)</td><td>
            <asp:TextBox ID="filePath" runat="server"/></td></tr>
        <tr><td>Target List</td><td dir="auto">
            <asp:RadioButtonList ID="rbTargetList" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Value="ITL" Selected="True"></asp:ListItem> 
                <asp:ListItem Value="RSI"></asp:ListItem> 
            </asp:RadioButtonList>
            </td></tr>
            <tr><td>Project:</td><td><asp:DropDownList ID="ddlProjCat" runat="server" Width="600px"></asp:DropDownList></td></tr>
            <tr><td>Project Site URL:</td><td><asp:TextBox ID="txtProjCatURL"  runat="server" Width="600px" Wrap="False"></asp:TextBox></td></tr>
            <tr><td>#Records:</td><td><asp:TextBox ID="txtRecords"  runat="server" Width="200px"></asp:TextBox>`</td></tr>
            <tr><td>Last Run Date:</td><td><asp:TextBox ID="txtLastRun"  runat="server" Width="200px"></asp:TextBox></td></tr>
        </table>

            <br />
            <strong>
            <asp:Button ID="uploadFile" runat="server" Text="Upload CSV" style="font-size: large; background-color: #00CCFF;" CssClass="auto-style2" OnClick="uploadFile_Click" />
            </strong>
        </div>

    <div>
        <asp:Literal ID="CVSUploader" runat="server" />
        
    </div>

</asp:Content>

