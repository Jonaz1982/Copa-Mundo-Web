<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ucDecidir.ascx.cs" Inherits="ucDecidir" %>

<table>
    <tr>
	    <td colspan="2" bgcolor="midnightblue">
		    <font face="verdana" color="white">
			    <asp:Label id="lblTitulo" runat="server">Titulo</asp:Label></font>
	    </td>
    </tr>
    <tr>
	    <td bgcolor="white" align="center">
		    <font face="verdana">
			    <asp:Label id="lblMensaje" runat="server">Mensaje</asp:Label>
			    <br>
			    <asp:Button id="btnSi" runat="server" Text="Sí" Width="50px" CommandName="Decidir" OnClick="btnSi_Click"></asp:Button>
			    <asp:Button id="btnNo" runat="server" Text="No" Width="50px" CommandName="Decidir" OnClick="btnNo_Click"></asp:Button></font>
	    </td>
    </tr>
</table>