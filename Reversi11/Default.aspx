<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Reversi11.Default" %>

<%@ Register src="Plansza.ascx" tagname="Plansza" tagprefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <font size=" "+10">Riversi</font><br />
            Filip Ratkowski 2017 <p />

            <table>
                <tr>
                    <td><uc1:Plansza ID="Plansza1" runat="server" /></td>
                    <td>&nbsp; &nbsp; &nbsp;</td>
                    <td valign="top">
                        <table>
                            <tr>
                                <td><asp:Label ID="Label1" runat="server" Text="Label"></asp:Label></td>
                                <td>&nbsp; &nbsp;</td>
                                <td><asp:Label ID="Label2" runat="server" Text="Label"></asp:Label></td>
                            </tr>
                            <tr>
                                <td><asp:ListBox ID="ListBox1" runat="server"></asp:ListBox></td>
                                <td>&nbsp; &nbsp;</td>
                                <td><asp:ListBox ID="ListBox2" runat="server"></asp:ListBox></td>
                            </tr>
                        </table>
                     </td>
                </tr>
            </table>
            <p />
            Ruch ma gracz
            <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
            <p />
            <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="true" Text="Podpowiadaj ruch" />
            <p />
            <asp:Button ID="Button1" runat="server" Text="Rozpocznij nową gre" OnClick="Button1_Click" />
            %nbsp;
            <asp:Button ID="Button2" runat="server" Text="Wykonaj najlepszy ruch" OnClick="Button2_Click" />
            <p />
            Komunikaty:<br />
            <asp:Label ID="Label3" runat="server"></asp:Label>
        </div>
    </form>
</body>
</html>
