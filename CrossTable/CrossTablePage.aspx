<%-- Закомментированный код можно использовать при наличии Мастерпейджа. Снизу всё закомментировать --%>
<%--<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="CrossTablePage.aspx.cs" Inherits="CrossTable.CrossTablePage" %>


<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
            CellPadding="3">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
            </Columns>
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="LightGray" Font-Bold="True" ForeColor="Black" />
        </asp:GridView>
    </div>
    <asp:Button ID="btnUpdate" runat="server" onclick="btnUpdate_Click" Text="Сохранить" />
</asp:Content>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrossTablePage.aspx.cs" Inherits="CrossTable.CrossTablePage" %>
<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>CrossTablePage</title>
    <style type="text/css">
        .Number,.NomenclatureName,.NomenclatureCode,.TransferQuantityMO,.Availability,.TotalQuantityMO,
        .NomenclatureNameAnalog,.NomenclatureCodeAnalog,.History,.AutorHeadSelect,.ManagerSelect,.TransferQuantity,
        .CostInRub,.Total,.Term{
            white-space: pre-line;
            word-wrap: break-word;
        }

        .Number{
            max-width: 200px;
            font-size: 15px;
        }
        .NomenclatureName{
            max-width: 200px;
            font-size: 15px;
        }
        .NomenclatureCode{
            max-width: 200px;
            font-size: 15px;
        }
        .TransferQuantityMO{
            max-width: 200px;
            font-size: 15px;
        }
        .Availability{
            max-width: 200px;
            font-size: 15px;
        }
        .TotalQuantityMO{
            max-width: 200px;
            font-size: 15px;
        }

        .NomenclatureNameAnalog{
            max-width: 200px;
            font-size: 15px;
        }
        .NomenclatureCodeAnalog{
            max-width: 200px;
            font-size: 15px;
        }
        .History{
            max-width: 200px;
            font-size: 15px;
        }
        .AutorHeadSelect{
            max-width: 200px;
            font-size: 15px;
        }
        .ManagerSelect{
            max-width: 200px;
            font-size: 15px;
        }
        .TransferQuantity{
            max-width: 200px;
            font-size: 15px;
        }
        .CostInRub{
            max-width: 200px;
            font-size: 15px;
        }
        .Total{
            max-width: 200px;
            font-size: 15px;
        }
        .Term{
            max-width: 200px;
            font-size: 15px;
        }
    </style>
</head>
<body>
    <form runat="server">
        <asp:ScriptManager runat="server">
            <Scripts>
                <asp:ScriptReference Name="MsAjaxBundle" />
                <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                <asp:ScriptReference Name="WebFormsBundle" />
            </Scripts>
        </asp:ScriptManager>

        <div>
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px"
                CellPadding="3">
                <FooterStyle BackColor="White" ForeColor="#000066" />
                <Columns>
                </Columns>
                <RowStyle ForeColor="#000066" />
                <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
                <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
                <HeaderStyle BackColor="LightGray" Font-Bold="True" ForeColor="Black" />
            </asp:GridView>
        </div>
        <asp:Button ID="btnUpdate" runat="server" onclick="btnUpdate_Click" Text="Сохранить" />
    </form>
</body>
</html>