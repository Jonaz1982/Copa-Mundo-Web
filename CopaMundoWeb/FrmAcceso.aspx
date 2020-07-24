<%@ Page Title="" Language="C#" MasterPageFile="~/Principal.master" AutoEventWireup="true" CodeFile="FrmAcceso.aspx.cs" Inherits="FrmAcceso" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <table>
        <tr>
            <td class="auto-style2" colspan="2" style="color: #FFFFFF; font-weight: 700; text-align: center; background-color: #993333">Acceso</td>
        </tr>
        <tr>
            <td style="background-color: #FFCC99">Usuario</td>
            <td>
                <asp:TextBox ID="txtUsuario" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td style="background-color: #FFCC99">Clave</td>
            <td>
                <asp:TextBox ID="txtClave" runat="server" TextMode="Password"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2" style="text-align: center">
                <asp:Button ID="btnIngresar" runat="server" OnClick="btnIngresar_Click" Text="Ingresar" />
            </td>
        </tr>
    </table>

</asp:Content>


