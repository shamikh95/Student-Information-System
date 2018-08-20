<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"
    CodeBehind="CourseRegistration.aspx.cs" Inherits="Student_Information_System.CourseRegistration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<center>
<h2 style="font-size: 60px">Course Registration</h2>
</center>
    <form id="Form1" runat="server">
    <div id="div1" title="Hello" style="text-align: center; position: fixed; top: 0%; left:40% ; background-color: #F1F3F8" align="center" runat="server">
        <center>
            <table id="creditTable" style="color: #000000">
                <tr>
                    <td>
                        You have selected:
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>/40 Credits
                    </td>
                </tr>
            </table>
        </center>
    </div>
    <asp:Table ID="myTable" runat="server" BorderStyle="Solid" GridLines="Both" ForeColor="Black">
    </asp:Table>
    <asp:Table ID="myTable1" runat="server" BorderStyle="Solid" GridLines="Both">
    </asp:Table>
    <asp:Button ID="Button1" runat="server" Text="Submit" OnClick="Button1_Click" />
    </form>
</asp:Content>
