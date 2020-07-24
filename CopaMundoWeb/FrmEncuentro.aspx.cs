using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrmEncuentro : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //Definir subprograma para el evento cuando se decida la eliminacion
        dEliminarEncuentro.Decidir += new EventHandler(dEliminarEncuentro_Decidir);

        //Verificar si el Usuario ha ingresado
        if (Session["SesionIniciada"] == null || !(Boolean)Session["SesionIniciada"])
            Response.Redirect("frmAcceso.aspx");
        else
        {
            //Verificar si es la primera vez que se carga la página
            if (!IsPostBack)
            {
                //Consultar los Encuentros
                try
                {
                    Encuentro.Preparar(ddlCampeonato, ddlFase);
                    MostrarPanel(0);
                }
                catch (Exception ex)
                {
                    Utilidades.Mensaje("Error grave:\n" + ex.Message);
                    MostrarPanel(-1);
                }
            }
        }
    }

    //Metodo para activar panel
    private void MostrarPanel(int numero)
    {
        //Todos los paneles inicialmente invisibles
        pnlConsultar.Enabled = false;
        pnlEdicionEncuentro.Visible = false;
        gvEncuentro.Enabled = false;
        pnlBotones.Visible = false;
        dEliminarEncuentro.Visible = false;
        switch (numero)
        {
            case -1:

                break;
            case 0:
                pnlConsultar.Enabled = true;
                gvEncuentro.Enabled = true;
                pnlBotones.Visible = true;
                break;
            case 1:
                pnlEdicionEncuentro.Visible = true;
                break;
            case 2:
                dEliminarEncuentro.Visible = true;
                break;
        }
    }
    protected void ddlCampeonato_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListarGrupos();
    }
    protected void ddlFase_SelectedIndexChanged(object sender, EventArgs e)
    {
        ListarGrupos();
    }
    private void ListarGrupos()
    {
        if (ddlCampeonato.SelectedIndex >= 0 && ddlFase.SelectedIndex >= 0)
        {
            Encuentro.Preparar(int.Parse(ddlCampeonato.SelectedValue), ddlGrupo);
        }
        else
            ddlGrupo.DataSource = null;
    }

    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlCampeonato.SelectedIndex >= 0 && ddlFase.SelectedIndex >= 0 && ddlGrupo.SelectedIndex >= 0)
        {
            Encuentro.Preparar(int.Parse(ddlCampeonato.SelectedValue),
                                        int.Parse(ddlFase.SelectedValue),
                                        int.Parse(ddlGrupo.SelectedValue),
                                        gvEncuentro);
        }
        else
        {
            if (ddlCampeonato.SelectedIndex >= 0 && ddlFase.SelectedIndex >= 0)
            {
                Encuentro.Preparar(int.Parse(ddlCampeonato.SelectedValue),
                                            int.Parse(ddlFase.SelectedValue),
                                            -1,
                                            gvEncuentro);
            }
        }
    }




    protected void btnAgregarEncuentro_Click(object sender, ImageClickEventArgs e)
    {
        //Titulo para el panel
        lblTituloEdicion.Text = "Editando datos de un nuevo Encuentro";
        //Mostrar el panel de edición de Encuentro
        MostrarPanel(1);
        //Iniciar la edición
        Encuentro.IniciarEdicion(-1,
                                ddlPais1,
                                txtGoles1,
                                ddlPais2,
                                txtGoles2,
                                ddlEstadio,
                                dpFecha
                                );
    }
    protected void btnModificarEncuentro_Click(object sender, ImageClickEventArgs e)
    {
        if (gvEncuentro.SelectedIndex >= 0)
        {
            MostrarPanel(1);
            Encuentro.IniciarEdicion((int)gvEncuentro.SelectedDataKey.Value,
                                    ddlPais1,
                                    txtGoles1,
                                    ddlPais2,
                                    txtGoles2,
                                    ddlEstadio,
                                    dpFecha
                                    );
            lblTituloEdicion.Text = "Editando datos del encuentro <STRONG>N° " +
                                    ddlPais1.SelectedItem.Text + " vs " +
                                    ddlPais2.SelectedItem.Text + "</STRONG>";
        }
        else
            Utilidades.Mensaje("Debe seleccionar una Encuentro");
    }
    protected void btnEliminarEncuentro_Click(object sender, ImageClickEventArgs e)
    {
        if (gvEncuentro.SelectedIndex >= 0)
        {
            dEliminarEncuentro.Titulo = "Eliminando Encuentro";
            dEliminarEncuentro.Mensaje = "Está seguro?";
            MostrarPanel(2);
        }
        else
            Utilidades.Mensaje("Debe seleccionar un Encuentro");
    }
    protected void btnAceptarEncuentro_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Verificar si pudo actualizar el registro
            if (Encuentro.Guardar(ddlCampeonato,
                                ddlFase,
                                ddlPais1,
                                txtGoles1,
                                ddlPais2,
                                txtGoles2,
                                ddlEstadio,
                                dpFecha))
            {
                //Retornar a la lista
                MostrarPanel(0);
                //Actualizar la lista de los Encuentros
                Encuentro.Preparar(int.Parse(ddlCampeonato.SelectedValue),
                                int.Parse(ddlFase.SelectedValue),
                                int.Parse(ddlGrupo.SelectedValue),
                                gvEncuentro);

                //Mensaje que confirma la operación
                Utilidades.Mensaje("La información del Encuentro fue actualizada");
            }
            else
                Utilidades.Mensaje("No se pudo actualizar la información del Encuentro");
        }
        catch (Exception ex)
        {
            Utilidades.Mensaje(ex.Message);
        }
    }
    private void dEliminarEncuentro_Decidir(object sender, EventArgs e)
    {
        //El Usuario eligio afirmativamente?
        if (dEliminarEncuentro.Decision)
        {
            try
            {
                if (Encuentro.Eliminar((int)gvEncuentro.SelectedDataKey.Value))
                {
                    //Actualizar la lista de los Encuentros
                    Encuentro.Preparar(int.Parse(ddlCampeonato.SelectedValue),
                                    int.Parse(ddlFase.SelectedValue),
                                    int.Parse(ddlGrupo.SelectedValue),
                                    gvEncuentro);

                    Utilidades.Mensaje("El Encuentro fue eliminado");
                    MostrarPanel(0);
                }
            }
            catch (Exception ex)
            {
                Utilidades.Mensaje(ex.Message);
            }
        }
        else
            MostrarPanel(0);
    }
    protected void btnCancelarEncuentro_Click(object sender, ImageClickEventArgs e)
    {
        MostrarPanel(0);
    }

    protected void gvEncuentro_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Utilidades.CambiarPagina(gvEncuentro, e.NewPageIndex, 
                                    Encuentro.Obtener(int.Parse(ddlCampeonato.SelectedValue),
                                        int.Parse(ddlFase.SelectedValue),
                                        int.Parse(ddlGrupo.SelectedValue)));
    }
    protected void ddlGrupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlGrupo.SelectedIndex >= 0)
            Encuentro.Preparar(int.Parse(ddlCampeonato.SelectedValue),
                                    int.Parse(ddlFase.SelectedValue),
                                    int.Parse(ddlGrupo.SelectedValue),
                                    ddlPais1, 
                                    ddlPais2,
                                    ddlEstadio);
    }
    protected void ddlPais1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPais1.SelectedIndex >= 0)
        {
            imgBandera1.ImageUrl = "hdlImagen.ashx?Id=" + ddlPais1.SelectedValue + "&Campo=Bandera";
            imgBandera1.DataBind();
        }
    }
    protected void ddlPais2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPais2.SelectedIndex >= 0)
        {
            imgBandera2.ImageUrl = "hdlImagen.ashx?Id=" + ddlPais2.SelectedValue + "&Campo=Bandera";
            imgBandera2.DataBind();
        }
    }
}