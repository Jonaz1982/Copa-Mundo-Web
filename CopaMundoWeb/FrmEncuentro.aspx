<%@ Page Title="" Language="C#" MasterPageFile="~/Menu.master" AutoEventWireup="true" CodeFile="FrmEncuentro.aspx.cs" Inherits="FrmEncuentro" %>
<%@ Register src="ucDecidir.ascx" tagname="ucDecidir" tagprefix="uc1" %>

<%@ Register assembly="ControlFecha" namespace="ControlFecha" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    <asp:Panel ID="pnlConsultar" runat="server">
    <table id="Table3" runat="server" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td rowspan="3" bgcolor="#DDDDDD">
                <asp:ImageButton ID="btnBuscar" runat="server" ImageUrl="~/Imagenes/Buscar.gif" ToolTip="Buscar Encuentros" onclick="btnBuscar_Click"/>
            </td>
            <td bgcolor="#DDDDDD">
                Campeonato
            </td>
            <td>
                <asp:DropDownList ID="ddlCampeonato" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlCampeonato_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td bgcolor="#DDDDDD">
                Fase
            </td>
            <td>
                <asp:DropDownList ID="ddlFase" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlFase_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td bgcolor="#DDDDDD">
                Grupo
            </td>
            <td>
                <asp:DropDownList ID="ddlGrupo" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlGrupo_SelectedIndexChanged"></asp:DropDownList>
            </td>
        </tr>
     </table>
    </asp:Panel>
    <table id="Table2" runat="server" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:GridView ID="gvEncuentro" runat="server" BackColor="White" BorderColor="#999999" 
                    BorderStyle="None" BorderWidth="1px" CellPadding="3" 
                    EnableModelValidation="True" GridLines="Vertical" AllowPaging="True" OnPageIndexChanging="gvEncuentro_PageIndexChanging">
                    <AlternatingRowStyle BackColor="#DCDCDC" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td align="center">
                <asp:Panel ID="pnlBotones" runat="server">
                    <asp:ImageButton ID="btnAgregarEncuentro" ImageUrl="Imagenes/Agregar.gif" 
                        ToolTip="Agregar Encuentro" runat="server" onclick="btnAgregarEncuentro_Click" />
                    <asp:ImageButton ID="btnModificarEncuentro" ImageUrl="Imagenes/Modificar.gif" 
                        ToolTip="Modificar Encuentro" runat="server" 
                        onclick="btnModificarEncuentro_Click" />
                    <asp:ImageButton ID="btnEliminarEncuentro" ImageUrl="Imagenes/Eliminar.gif" 
                        ToolTip="Retirar Encuentro" runat="server" 
                        onclick="btnEliminarEncuentro_Click" />
                </asp:Panel>
                <uc1:ucdecidir ID="dEliminarEncuentro" runat="server" />
                </td>
            </tr>
    </table>


    <asp:Panel ID="pnlEdicionEncuentro" runat="server">
        <span style="color: #000000; font:8pt Verdana">
        <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td align="center" bgcolor="#033060" colspan="4">
                    <span style="color: white; font:bold 12pt Verdana">
                    <asp:Label ID="lblTituloEdicion" runat="server" Text="Label"></asp:Label>
                    </span>
                </td>
            </tr>
            <tr>
                <td align="center" bgcolor="#0F50AA" colspan="4">
                    <span style="color: #FFFFFF">
                    Información Encuentro</span>
                </td>
            </tr>
            <tr>
                <td bgcolor="#999999">
                    Fecha
                </td>
                <td bgcolor="#DDDDDD">
                    <cc1:DatePicker ID="dpFecha" runat="server" />
                </td>
                <td bgcolor="#999999">
                    Estadio
                </td>
                <td bgcolor="#DDDDDD">
                     <asp:DropDownList ID="ddlEstadio" runat="server"></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td bgcolor="#999999">
                    País
                </td>
                <td bgcolor="#DDDDDD">
                     <asp:DropDownList ID="ddlPais1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPais1_SelectedIndexChanged"></asp:DropDownList>
                     <asp:Image ID="imgBandera1" runat="server" />
                </td>
                <td bgcolor="#999999">
                    Goles
                </td>
                <td bgcolor="#DDDDDD">
                    <asp:TextBox ID="txtGoles1" runat="server" Width="50px"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td bgcolor="#999999">
                    País
                </td>
                <td bgcolor="#DDDDDD">
                     <asp:DropDownList ID="ddlPais2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPais2_SelectedIndexChanged"></asp:DropDownList>
                     <asp:Image ID="imgBandera2" runat="server" />
                </td>
                <td bgcolor="#999999">
                    Goles
                </td>
                <td bgcolor="#DDDDDD">
                    <asp:TextBox ID="txtGoles2" runat="server" Width="50px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <asp:ImageButton ID="btnAceptarEncuentro" runat="server" 
                        ImageUrl="~/Imagenes/Aceptar.gif" onclick="btnAceptarEncuentro_Click" />
                    <asp:ImageButton ID="btnCancelarEncuentro" runat="server" 
                        ImageUrl="~/Imagenes/Cancelar.gif" onclick="btnCancelarEncuentro_Click" />
                </td>
            </tr>
        </table>  
        </span>    
    </asp:Panel>



</asp:Content>

