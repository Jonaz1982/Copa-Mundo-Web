<%@ Page Title="" Language="C#" MasterPageFile="~/Menu.master" AutoEventWireup="true" CodeFile="FrmCampeonato.aspx.cs" Inherits="FrmCampeonato" %>
<%@ Register src="ucDecidir.ascx" tagname="ucDecidir" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" Runat="Server">
    
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvCampeonato" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None" OnSelectedIndexChanged="gvCampeonato_SelectedIndexChanged">
                    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" />
                    </Columns>
                    <EditRowStyle BackColor="#999999" />
                    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                    <SortedAscendingCellStyle BackColor="#E9E7E2" />
                    <SortedAscendingHeaderStyle BackColor="#506C8C" />
                    <SortedDescendingCellStyle BackColor="#FFFDF8" />
                    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                </asp:GridView>
            </td>
            <td>

                <asp:ImageButton ID="btnLogo" runat="server" OnClick="btnLogo_Click" />

                <asp:Panel ID="pnlLogo" runat="server" style="text-align: center">
                    <asp:FileUpload ID="fuLogo" runat="server" />
                    <br />
                    <asp:ImageButton ID="btnAceptarLogo" runat="server" ImageUrl="~/Imagenes/Aceptar.gif" OnClick="btnAceptarLogo_Click" />
                    <asp:ImageButton ID="btnCancelar" runat="server" ImageUrl="~/Imagenes/Cancelar.gif" OnClick="btnCancelar_Click" />
                </asp:Panel>

            </td>
        </tr>
        <tr>
            <td align="center" colspan="2">
                <asp:Panel ID="pnlBotones" runat="server">
                    <asp:ImageButton ID="btnAgregarCampeonato" ImageUrl="Imagenes/Agregar.gif" 
                        ToolTip="Agregar Campeonato" runat="server" onclick="btnAgregarCampeonato_Click" />
                    <asp:ImageButton ID="btnModificarCampeonato" ImageUrl="Imagenes/Modificar.gif" 
                        ToolTip="Modificar Campeonato" runat="server" 
                        onclick="btnModificarCampeonato_Click" />
                    <asp:ImageButton ID="btnEliminarCampeonato" ImageUrl="Imagenes/Eliminar.gif" 
                        ToolTip="Retirar Grupo Seleccionado de la Campeonato" runat="server" 
                        onclick="btnEliminarCampeonato_Click" />
                    <asp:ImageButton ID="btnGrupos" ImageUrl="Imagenes/Grupos.jpg" 
                        ToolTip="Registrar Grupos del Campeonato Seleccionado" runat="server" 
                        onclick="btnGrupos_Click" />
                    <asp:ImageButton ID="btnImportar" ImageUrl="Imagenes/Importar.jpg" 
                        ToolTip="Importar Información del Campeonato Seleccionado" runat="server" 
                        onclick="btnImportar_Click" />
                </asp:Panel>
                <uc1:ucdecidir ID="dEliminarCampeonato" runat="server" />
                </td>
            </tr>
    </table>
</asp:Content>

