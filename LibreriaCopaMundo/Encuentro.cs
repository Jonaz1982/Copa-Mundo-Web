using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Text;
using ControlFecha;

public class Encuentro
{
    //Metodo para preparar listas para edición
    public static void Preparar(DropDownList ddlCampeonato,
                            DropDownList ddlFase
                            )
    {
        try
        {
            //Obtener la lista de Estadios
            DataTable tbl = Campeonato.ObtenerLista();

            //Configurar las listas desplegables
            ddlCampeonato.DataSource = tbl;
            ddlCampeonato.DataTextField = "Campeonato";
            ddlCampeonato.DataValueField = "Id";
            ddlCampeonato.DataBind();

            //Obtener la lista de Fases
            tbl = Fase.ObtenerLista();

            //Configurar las listas desplegables
            ddlFase.DataSource = tbl;
            ddlFase.DataTextField = "Fase";
            ddlFase.DataValueField = "Id";
            ddlFase.DataBind();

        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al preparar listas para edición:\n" + ex.Message);
        }

    }

    //Metodo para preparar listas para edición
    public static void Preparar(int IdCampeonato,
                                int IdFase,
                                int IdGrupo,
                                DropDownList ddlPais1,
                                DropDownList ddlPais2,
                                DropDownList ddlEstadio)
    {
        try
        {
            DataTable tbl;
            //Obtener la lista de Paises
            if(IdFase>=1)
                tbl = Pais.ObtenerLista(IdCampeonato);
            else
                tbl = Grupo.ObtenerListaPaises(IdGrupo);

            //Configurar las listas desplegables
            ddlPais1.DataSource = tbl;
            ddlPais1.DataTextField = "Pais";
            ddlPais1.DataValueField = "Id";
            ddlPais1.DataBind();

            //Configurar las listas desplegables
            ddlPais2.DataSource = tbl;
            ddlPais2.DataTextField = "Pais";
            ddlPais2.DataValueField = "Id";
            ddlPais2.DataBind();

            //Obtener la lista de Estadios
            tbl = Estadio.ObtenerLista(IdCampeonato);

            //Configurar las listas desplegables
            ddlEstadio.DataSource = tbl;
            ddlEstadio.DataTextField = "Estadio";
            ddlEstadio.DataValueField = "Id";
            ddlEstadio.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al preparar listas para edición:\n" + ex.Message);
        }

    }

    //Metodo para preparar listas para edición
    public static void Preparar(int IdCampeonato,
                                int IdFase,
                                int IdGrupo,
                                GridView gvEncuentro)
    {
        try
        {
            //Obtener la lista de Encuentros para edición
            DataTable tbl = Obtener(IdCampeonato, IdFase, IdGrupo);
            gvEncuentro.DataSource = tbl;
            gvEncuentro.DataKeyNames = new String[] { "Id" };
            gvEncuentro.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al preparar listas para edición:\n" + ex.Message);
        }

    }

    //Metodo para preparar listas para edición
    public static void Preparar(int IdCampeonato,
                                DropDownList ddlGrupo)
    {
        try
        {
            //Obtener la lista de Estadios
            DataTable tbl = Grupo.ObtenerLista(IdCampeonato);

            //Configurar las listas desplegables
            ddlGrupo.DataSource = tbl;
            ddlGrupo.DataTextField = "Grupo";
            ddlGrupo.DataValueField = "Id";
            ddlGrupo.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al preparar listas para edición:\n" + ex.Message);
        }

    }

    //Metodo para listar los Encuentros para edición
    public static DataTable Obtener(int IdCampeonato,
                                    int IdFase,
                                    int IdGrupo)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarEncuentrosEdicion '" + IdCampeonato +
                                    "','" + IdFase + 
                                    "', '" + IdGrupo + 
                                    "'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Encuentros:\n" + ex.Message);
        }

    }

    //Método para preparar la edicion de un Encuentro
    public static void IniciarEdicion(int Id,
                                    DropDownList ddlCampeonato,
                                    DropDownList ddlFase,
                                    DropDownList ddlPais1,
                                    TextBox txtGoles1,
                                    Image imgBandera1,
                                    DropDownList ddlPais2,
                                    TextBox txtGoles2,
                                    Image imgBandera2,
                                    DropDownList ddlEstadio,
                                    DatePicker dpFecha
        )
    {
        //Se esta editando un Encuentro existente?
        if (Id != -1)
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Encuentro" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                ddlCampeonato.SelectedValue = dr["IdCampeonato"].ToString();
                ddlFase.SelectedValue = dr["IdFase"].ToString();
                ddlPais1.SelectedValue = dr["IdPais1"].ToString();
                imgBandera1.ImageUrl = "hdlImagen.ashx?Id=" + dr["IdPais1"].ToString() + "&Campo=Bandera";
                imgBandera1.DataBind();
                ddlPais2.SelectedValue = dr["IdPais2"].ToString();
                imgBandera2.ImageUrl = "hdlImagen.ashx?Id=" + dr["IdPais2"].ToString() + "&Campo=Bandera";
                imgBandera2.DataBind();
                ddlEstadio.SelectedValue = dr["IdEstadio"].ToString();
                dpFecha.CalendarDate = (DateTime)dr["Fecha"];
                txtGoles1.Text = dr["Goles1"].ToString();
                txtGoles2.Text = dr["Goles2"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            ddlCampeonato.SelectedIndex=-1; 
            ddlFase.SelectedIndex=-1; 
            ddlPais1.SelectedIndex=-1; 
            ddlPais2.SelectedIndex=-1; 
            ddlEstadio.SelectedIndex=-1;
            dpFecha.CalendarDate = DateTime.Now;
            txtGoles1.Text = "0";
            txtGoles2.Text = "0";
        }
        HttpContext.Current.Session["IdEncuentroEditado"] = Id;
    }

    //Método para preparar la edicion de un Encuentro
    public static void IniciarEdicion(int Id,
                                    DropDownList ddlPais1,
                                    TextBox txtGoles1,
                                    DropDownList ddlPais2,
                                    TextBox txtGoles2,
                                    DropDownList ddlEstadio,
                                    DatePicker dpFecha
        )
    {
        //Se esta editando un Encuentro existente?
        if (Id != -1)
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Encuentro" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                ddlPais1.SelectedValue = dr["IdPais1"].ToString();
                ddlPais2.SelectedValue = dr["IdPais2"].ToString();
                ddlEstadio.SelectedValue = dr["IdEstadio"].ToString();
                dpFecha.CalendarDate = Utilidades.ObtenerFecha(dr, "Fecha");
                txtGoles1.Text = dr["Goles1"].ToString();
                txtGoles2.Text = dr["Goles2"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            ddlPais1.SelectedIndex = -1;
            ddlPais2.SelectedIndex = -1;
            ddlEstadio.SelectedIndex = -1;
            dpFecha.CalendarDate = DateTime.Now;
            txtGoles1.Text = "0";
            txtGoles2.Text = "0";
        }
        HttpContext.Current.Session["IdEncuentroEditado"] = Id;
    }

    //Método para Guardar un Encuentro
    public static Boolean Guardar(DropDownList ddlCampeonato,
                                    DropDownList ddlFase,
                                    DropDownList ddlPais1,
                                    TextBox txtGoles1,
                                    DropDownList ddlPais2,
                                    TextBox txtGoles2,
                                    DropDownList ddlEstadio,
                                    DatePicker dpFecha
                                )
    {
        try
        {
            return Guardar((int)HttpContext.Current.Session["IdEncuentroEditado"],
                            int.Parse(ddlCampeonato.SelectedValue),
                            int.Parse(ddlFase.SelectedValue),
                            int.Parse(ddlPais1.SelectedValue),
                            int.Parse(txtGoles1.Text),
                            int.Parse(ddlPais2.SelectedValue),
                            int.Parse(txtGoles2.Text),
                            int.Parse(ddlEstadio.SelectedValue),
                            (dpFecha.IsValidDate ? dpFecha.CalendarDate : DateTime.MinValue)
                            );
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al actualizar Encuentro\n" + ex.Message);
        }
    }//Guardar

    //Método para Guardar un Encuentro
    public static Boolean Guardar(int Id,
                                    int IdCampeonato,
                                    int IdFase,
                                    int IdPais1,
                                    int Goles1,
                                    int IdPais2,
                                    int Goles2,
                                    int IdEstadio,
                                    DateTime Fecha
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (IdCampeonato > 0 &&
            IdFase> 0 &&
            IdPais1 > 0 &&
            Goles1 >= 0 &&
            IdPais2 > 0 &&
            Goles2 >= 0 &&
            IdEstadio > 0 &&
            Fecha != null
            )
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("SET DATEFORMAT DMY EXEC spActualizarEncuentro '" + Id +
                                    "','" + IdCampeonato + 
                                    "', '" + IdFase + 
                                    "', '" + IdPais1 + 
                                    "', '" + Goles1 + 
                                    "', '" + IdPais2 + 
                                    "', '" + Goles2 + 
                                    "', '" + IdEstadio +
                                    "', '" + Fecha.ToShortDateString() +
                                    "'");
            try
            {
                //Recuperar el objeto para consultas a la base de datos
                BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
                //Ejecutar la consulta
                Guardado = bd.Actualizar(strSQL.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al actualizar Encuentro\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar


    //Método para Guardar un Encuentro (transaccional)
    public static Boolean Guardar(int Id,
                                int IdCampeonato,
                                int IdFase,
                                int IdPais1,
                                int Goles1,
                                int IdPais2,
                                int Goles2,
                                int IdEstadio,
                                DateTime Fecha,
                                SqlTransaction t
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (IdCampeonato > 0 &&
           IdFase > 0 &&
           IdPais1 > 0 &&
           Goles1 >= 0 &&
           IdPais2 > 0 &&
           Goles2 >= 0 &&
           IdEstadio > 0 &&
           Fecha != null
           )
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarEncuentro '" + Id +
                                    "','" + IdCampeonato +
                                    "', '" + IdFase +
                                    "', '" + IdPais1 +
                                    "', '" + Goles1 +
                                    "', '" + IdPais2 +
                                    "', '" + Goles2 +
                                    "', '" + IdEstadio +
                                    Fecha.ToShortDateString() +
                                    "'");

            try
            {
                //Recuperar el objeto para consultas a la base de datos
                BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
                //Ejecutar la consulta
                Guardado = bd.Actualizar(strSQL.ToString(), t);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al actualizar Encuentro\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar

    //Método para Eliminar un Encuentro
    public static Boolean Eliminar(int Id)
    {
        try
        {
            String strSQL = "DELETE FROM Encuentro" +
                              " WHERE Id='" + Id + "'";
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.Actualizar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al eliminar Encuentro\n" + ex.Message);
        }
    }//Eliminar


    //Obtener la clave primaria de un Encuentro
    public static int ObtenerId(int IdCampeonato, 
                                int IdFase, 
                                int IdPais1,
                                int IdPais2)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarEncuentro '" + 
                                "','" + IdCampeonato +
                                    "', '" + IdFase +
                                    "', '" + IdPais1 +
                                    "', '" + IdPais2 +
                                    "'");
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Encuentro:\n" + ex.Message);
        }
    }

    //Obtener la clave primaria de un Encuentro (transaccional)
    public static int ObtenerId(int IdCampeonato,
                                int IdFase,
                                int IdPais1,
                                int IdPais2, 
                                SqlTransaction t)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarEncuentro '" +
                                "','" + IdCampeonato +
                                    "', '" + IdFase +
                                    "', '" + IdPais1 +
                                    "', '" + IdPais2 +
                                    "'", t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Encuentro:\n" + ex.Message);
        }
    }

}
