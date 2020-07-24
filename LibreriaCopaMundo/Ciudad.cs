using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Text;

public class Ciudad
{
    public static void Preparar(DropDownList ddlPais,
                            GridView gvCiudad
    )
    {
        try
        {
            //Obtener la lista de paises
            DataTable tbl = Pais.ObtenerLista();

            //Configurar las listas desplegables
            ddlPais.DataSource = tbl;
            ddlPais.DataTextField = "Pais";
            ddlPais.DataValueField = "Id";
            ddlPais.DataBind();

            //Obtener la lista de Ciudades para edición
            tbl = Obtener();
            gvCiudad.DataSource = tbl;
            gvCiudad.DataKeyNames = new String[] { "Id" };
            gvCiudad.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al preparar listas para edición:\n" + ex.Message);
        }

    }

    //Metodo para listar los Ciudades 
    public static DataTable ObtenerLista()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarCiudades";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Ciudades:\n" + ex.Message);
        }
    }

    //Metodo para listar los Ciudades para edición
    public static DataTable Obtener()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarCiudadesEdicion";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Ciudades:\n" + ex.Message);
        }

    }

    //Método para preparar la edicion de un Ciudad
    public static void IniciarEdicion(int Id,
                                    TextBox txtCiudad,
                                    DropDownList ddlPais
                                    )
    {
        //Se esta editando un Ciudad existente?
        if (Id != -1)
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Ciudad" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                txtCiudad.Text = dr["Ciudad"].ToString();
                ddlPais.SelectedValue = dr["IdPais"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            txtCiudad.Text = "";
            ddlPais.SelectedIndex =-1;
        }
        HttpContext.Current.Session["IdCiudadEditada"] = Id;
    }

    //Método para Guardar un Ciudad
    public static Boolean Guardar(TextBox txtCiudad,
                                    DropDownList ddlPais
                                )
    {
        try
        {
            return Guardar((int)HttpContext.Current.Session["IdCiudadEditada"],
                            txtCiudad.Text,
                            int.Parse(ddlPais.SelectedValue));
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al actualizar Ciudad\n" + ex.Message);
        }
    }//Guardar

    //Método para Guardar un Ciudad
    public static Boolean Guardar(int Id,
                                String Ciudad,
                                int IdPais
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!Ciudad.Equals(String.Empty) &&
            IdPais>0)
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarCiudad '" + Id +
                            "','" + Ciudad +
                            "','" + IdPais +
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
                throw new ArgumentException("Error al actualizar Ciudad\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar


    //Método para Guardar un Ciudad (transaccional)
    public static Boolean Guardar(int Id,
                                String Ciudad,
                                int IdPais,
                                SqlTransaction t
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!Ciudad.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarCiudad '" + Id +
                            "','" + Ciudad +
                            "','" + IdPais +
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
                throw new ArgumentException("Error al actualizar Ciudad\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar

    //Método para Eliminar un Ciudad
    public static Boolean Eliminar(int Id)
    {
        try
        {
            String strSQL = "DELETE FROM Ciudad" +
                              " WHERE Id='" + Id + "'";
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.Actualizar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al eliminar Ciudad\n" + ex.Message);
        }
    }//Eliminar


    //Obtener la clave primaria de un Ciudad
    public static int ObtenerId(String Ciudad)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarCiudad '" + Ciudad + "'");
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Ciudad:\n" + ex.Message);
        }
    }

    //Obtener la clave primaria de un Ciudad (transaccional)
    public static int ObtenerId(String Ciudad, SqlTransaction t)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarCiudad '" + Ciudad + "'", t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Ciudad:\n" + ex.Message);
        }
    }

}
