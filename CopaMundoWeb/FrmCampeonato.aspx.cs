using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class FrmCampeonato : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gvCampeonato.DataSource = Campeonato.ObtenerCampeonatos();
            gvCampeonato.DataKeyNames = new String[]{"Id"};//el id
            gvCampeonato.DataBind();
            mostrarPanel(0);
        }
    }


    private void mostrarPanel(int panel)
    {
        gvCampeonato.Enabled = false;
        pnlBotones.Visible = false;

        pnlLogo.Visible = false;
        dEliminarCampeonato.Visible = false;
        switch (panel)
        {
            case 0:
                gvCampeonato.Enabled = true;
                pnlBotones.Visible = true;
                break;
            case 1:
                pnlLogo.Visible = true;
                break;
            case 3:
                dEliminarCampeonato.Visible = true;
                break;

        }
    }


    protected void gvCampeonato_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(gvCampeonato.SelectedIndex>= 0)
        btnLogo.ImageUrl = "hdLogo.ashx?Id=" + gvCampeonato.SelectedDataKey.Value;//leee ese id
    }
    protected void btnLogo_Click(object sender, ImageClickEventArgs e)
    {
        Session["LogoCampeonato"] = true;
        mostrarPanel(1);
    }
    protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
    {
        mostrarPanel(0);
    }
    protected void btnAceptarLogo_Click(object sender, ImageClickEventArgs e)
    {
        //virificar si hay archivo seleccionado
        if (fuLogo.HasFile)
        {
            //try
            //{

                if ((Boolean)Session["LogoCampeonato"])
                {

                    //verificar si pudo actualizar el logo en la base de datos
                    if (Campeonato.ActualizarLogo((int)gvCampeonato.SelectedDataKey.Value, Utilidades.CargarImagen(fuLogo)))
                    {
                        btnLogo.ImageUrl = "hdLogo.ashx?Id=" + gvCampeonato.SelectedDataKey.Value;//leee ese id
                        mostrarPanel(0);
                    }
                }
                else
                {
                    String r = Campeonato.Importar((int)gvCampeonato.SelectedDataKey.Value,
                                                    new StreamReader(fuLogo.FileContent));
                    if (r.Equals(String.Empty))
                        Utilidades.Mensaje("Información importada exitosamente");
                    else
                        Utilidades.Mensaje(r);
                }
            //}
            //catch (Exception ex)
            //{
            //    Utilidades.Mensaje(ex.Message);
            //}
        }
        else
            Utilidades.Mensaje("No me mame galllo");
    }
    protected void btnImportar_Click(object sender, ImageClickEventArgs e)
    {
        if (gvCampeonato.SelectedIndex >= 0)
        {
            Session["LogoCampeonato"] = false;
            mostrarPanel(1);
        }
        else
        {
            Utilidades.Mensaje("Debe seleccionar un campeonato");
        }
    }
    protected void btnAgregarCampeonato_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnModificarCampeonato_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnEliminarCampeonato_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void btnGrupos_Click(object sender, ImageClickEventArgs e)
    {

    }
}