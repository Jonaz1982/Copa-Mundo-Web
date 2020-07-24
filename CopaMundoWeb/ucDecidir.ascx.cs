using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ucDecidir : System.Web.UI.UserControl
{

    public event System.EventHandler Decidir;

    protected void Page_Load(object sender, EventArgs e)
    {

    }

    //Variable interna para guardar la decision del usuario
    private bool decision = false;

    //Propiedad para asignar el titulo
    public string Titulo
    {
        set
        { lblTitulo.Text = value; }
    }

    //Propiedad para asignar el mensaje
    public string Mensaje
    {
        set
        { lblMensaje.Text = value; }
    }

    //Propiedad que devuelve la decision del usuario
    public bool Decision
    {
        get
        { return decision; }
    }

    protected void btnSi_Click(object sender, EventArgs e)
    {
        //El usuario decide afirmativamente
        decision = true;
        //Activar evento para futuro codigo
        this.Decidir(this, new EventArgs());
    }
    protected void btnNo_Click(object sender, EventArgs e)
    {
        //El usuario decide negativamente
        decision = false;
        //Activar evento para futuro codigo
        this.Decidir(this, new EventArgs());
    }

}