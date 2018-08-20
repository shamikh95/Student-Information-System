<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CourseStructure.aspx.cs" Inherits="Student_Information_System.CourseStructure" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <center>
<h2 style="font-size: 60px">Course Structure</h2>
</center>
    <form id="form1" runat="server">
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
        DataSourceID="SqlDataSource1" ForeColor="Black">
        <AlternatingRowStyle BackColor="LightBlue" ForeColor="White" />
        <Columns>
            <asp:BoundField DataField="SNo" HeaderText="SNo" InsertVisible="False" 
                ReadOnly="True" SortExpression="SNo" />
            <asp:BoundField DataField="Subject_Code" HeaderText="Subject Code" 
                SortExpression="Subject_Code" />
            <asp:BoundField DataField="Subject_Name" HeaderText="Subject Name" 
                SortExpression="Subject_Name" />
            <asp:BoundField DataField="Prereq" HeaderText="Pre-requistes" 
                SortExpression="Prereq" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" 
                SortExpression="Credits" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:Student_DBConnectionString %>" 
        SelectCommand="SELECT * FROM [Course_Map]"></asp:SqlDataSource>
    </form>


</asp:Content>
