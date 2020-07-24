using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class FrmAcceso : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnIngresar_Click(object sender, EventArgs e)
    {
        if (Conexion.Establecer())
        {
            if (Usuario.ValidarAcceso(txtUsuario.Text, txtClave.Text) )
                Response.Redirect("FrmCampeonato.aspx");
            else
                Utilidades.Mensaje("Acceso denegado");
        }
        else
            Utilidades.Mensaje("No se pudo acceder la base de datos");
    }
}