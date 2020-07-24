using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Text;

public class Estadio
{
    public static void Preparar(DropDownList ddlCiudad,
                            GridView gvEstadio
    )
    {
        try
        {
            //Obtener la lista de Ciudades
            DataTable tbl = Ciudad.ObtenerLista();

            //Configurar las listas desplegables
            ddlCiudad.DataSource = tbl;
            ddlCiudad.DataTextField = "Ciudad";
            ddlCiudad.DataValueField = "Id";
            ddlCiudad.DataBind();

            //Obtener la lista de Estadios para edición
            tbl = Obtener();
            gvEstadio.DataSource = tbl;
            gvEstadio.DataKeyNames = new String[] { "Id" };
            gvEstadio.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al preparar listas para edición:\n" + ex.Message);
        }

    }

    //Metodo para listar los Estadios 
    public static DataTable ObtenerLista()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarEstadios";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Estadios:\n" + ex.Message);
        }
    }

    //Metodo para listar los Estadios de un Campeonato
    public static DataTable ObtenerLista(int IdCampeonato)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarEstadiosCampeonato '"+IdCampeonato+"'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Estadios:\n" + ex.Message);
        }
    }

    //Metodo para listar los Estadios para edición
    public static DataTable Obtener()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarEstadiosEdicion";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Estadios:\n" + ex.Message);
        }

    }

    //Método para preparar la edicion de un Estadio
    public static void IniciarEdicion(int Id,
                                    TextBox txtEstadio,
                                    DropDownList ddlCiudad,
                                    TextBox txtCapacidad
                                    )
    {
        //Se esta editando un Estadio existente?
        if (Id != -1)
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Estadio" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                txtEstadio.Text = dr["Estadio"].ToString();
                ddlCiudad.SelectedValue = dr["IdCiudad"].ToString();
                txtCapacidad.Text = dr["Capacidad"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            txtEstadio.Text = "";
            ddlCiudad.SelectedIndex = -1;
            txtCapacidad.Text = "";
        }
        HttpContext.Current.Session["IdEstadioEditado"] = Id;
    }

    //Método para Guardar un Estadio
    public static Boolean Guardar(TextBox txtEstadio,
                                    DropDownList ddlCiudad,
                                    TextBox txtCapacidad
                                    )
    {
        try
        {
            return Guardar((int)HttpContext.Current.Session["IdEstadioEditado"],
                            txtEstadio.Text,
                            int.Parse(ddlCiudad.SelectedValue),
                            int.Parse(txtCapacidad.Text));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al actualizar Estadio\n" + ex.Message);
        }
    }//Guardar

    //Método para Guardar un Estadio
    public static Boolean Guardar(int Id,
                                String Estadio,
                                int IdCiudad,
                                int Capacidad
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!Estadio.Equals(String.Empty) &&
            IdCiudad > 0 &&
            Capacidad>0)
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarEstadio '" + Id +
                            "','" + Estadio +
                            "','" + IdCiudad +
                            "','" + Capacidad +
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
                throw new ArgumentException("Error al actualizar Estadio\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar


    //Método para Guardar un Estadio (transaccional)
    public static Boolean Guardar(int Id,
                                String Estadio,
                                int IdCiudad,
                                int Capacidad,
                                SqlTransaction t
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!Estadio.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarEstadio '" + Id +
                            "','" + Estadio +
                            "','" + IdCiudad +
                            "','" + Capacidad +
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
                throw new ArgumentException("Error al actualizar Estadio\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar

    //Método para Eliminar un Estadio
    public static Boolean Eliminar(int Id)
    {
        try
        {
            String strSQL = "DELETE FROM Estadio" +
                              " WHERE Id='" + Id + "'";
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.Actualizar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al eliminar Estadio\n" + ex.Message);
        }
    }//Eliminar


    //Obtener la clave primaria de un Estadio
    public static int ObtenerId(String Estadio)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarEstadio '" + Estadio + "'");
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Estadio:\n" + ex.Message);
        }
    }

    //Obtener la clave primaria de un Estadio (transaccional)
    public static int ObtenerId(String Estadio, SqlTransaction t)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarEstadio '" + Estadio + "'", t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Estadio:\n" + ex.Message);
        }
    }

    //Metodo para Obtener la imagen de la Foto en binario
    public static Byte[] ObtenerFoto(int Id)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerImagen("Estadio", "Foto", "Id=" + Id);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //Metodo para guardar la imagen de la Foto en binario
    public static Boolean GuardarFoto(int Id, Byte[] Imagen)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ActualizarImagen("Estadio", "Foto", "Id=" + Id, Imagen);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}
