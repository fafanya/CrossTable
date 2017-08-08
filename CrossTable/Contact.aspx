<%@ Page Title="Contact" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="CrossTable.Contact" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
            CellPadding="3" DataKeyNames="Product_Name" >
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
                <asp:BoundField DataField="Product_Name" HeaderText="ITEM_N0" InsertVisible="False" ReadOnly="True" SortExpression="Product_Name" />

                <asp:TemplateField HeaderText="NAME" SortExpression="NAME">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("Product_Name") %>' 
                            OnTextChanged="TextBox_TextChanged" BorderStyle="None">
                        </asp:TextBox>
                        <asp:HiddenField ID="HiddenField1" runat="server" Value='<%# Eval("Quantity") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Price" SortExpression="Price">
                    <ItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Price") %>'
                            OnTextChanged="TextBox_TextChanged" BorderStyle="None"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </div>
    <asp:Button ID="btnUpdate" runat="server" onclick="btnUpdate_Click" Text="Update" />
</asp:Content>