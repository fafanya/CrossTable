<%@ Page Title="About" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="CrossTable.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
    </div>--%>

    <asp:Repeater ID="Repeater1" runat="server">
    <HeaderTemplate>
        <table border="1">
            <tr>
                <th scope="col" style="width: 120px">
                    Name
                </th>
                <th scope="col" style="width: 100px">
                    Country
                </th>
                <%--<th scope="col" style="width: 80px">
                </th>--%>
            </tr>
    </HeaderTemplate>
    <ItemTemplate>
        <tr>
            <td>
                <asp:Label ID="lblCustomerId" runat="server" Text='<%# Eval("Product_Name") %>' Visible = "false" />
                <asp:Label ID="lblContactName" runat="server" Text='<%# Eval("Quantity") %>' />
                <asp:TextBox ID="txtContactName" runat="server" Width="120" Text='<%# Eval("Quantity") %>'
                    Visible="false" />
            </td>
            <td>
                <asp:Label ID="lblCountry" runat="server" Text='<%# Eval("Price") %>' />
                <asp:TextBox ID="txtCountry" runat="server" Width="120" Text='<%# Eval("Price") %>'
                    Visible="false" />
            </td>
            <%--<td>
                <asp:LinkButton ID="lnkEdit" Text="Edit" runat="server" OnClick="OnEdit" />
                <asp:LinkButton ID="lnkUpdate" Text="Update" runat="server" Visible="false" OnClick="OnUpdate" />
                <asp:LinkButton ID="lnkCancel" Text="Cancel" runat="server" Visible="false" OnClick="OnCancel" />
                <asp:LinkButton ID="lnkDelete" Text="Delete" runat="server" OnClick="OnDelete" OnClientClick="return confirm('Do you want to delete this row?');" />
            </td>--%>
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
</asp:Repeater>
<table border="1" style="border-collapse: collapse">
    <tr>
        <%--<td style="width: 150px">
            Name:<br />
            <asp:TextBox ID="txtName" runat="server" Width="140" />
        </td>
        <td style="width: 150px">
            Country:<br />
            <asp:TextBox ID="txtCountry" runat="server" Width="140" />
        </td>--%>
        <td style="width: 100px">
            <asp:Button ID="btnAdd" runat="server" Text="Add" OnClick="Insert" />
        </td>
    </tr>
</table>
</asp:Content>

