using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;


using System.Data.SqlClient;
using System.Text;

public class Pais
{
    //Metodo para listar los paises 
    public static DataTable ObtenerLista()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarPaises";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Paises:\n" + ex.Message);
        }
    }

    //Metodo para listar los paises de un Campeonato
    public static DataTable ObtenerLista(int IdCampeonato)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarPaisesCampeonato '"+IdCampeonato+"'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Paises:\n" + ex.Message);
        }
    }
    //Metodo para listar los Paises para edición
    public static DataTable Obtener()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarPaisesEdicion";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Paises:\n" + ex.Message);
        }

    }

    //Método para preparar la edicion de un Pais
    public static void IniciarEdicion(int Id,
                                    TextBox txtPais,
                                    TextBox txtEntidad
                                    )
    {
        //Se esta editando un Pais existente?
        if (Id != -1)
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Pais" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                txtPais.Text = dr["Pais"].ToString();
                txtEntidad.Text = dr["Entidad"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            txtPais.Text = "";
            txtEntidad.Text = "";
        }
        HttpContext.Current.Session["IdPaisEditado"] = Id;
    }

    //Método para Guardar un Pais
    public static Boolean Guardar(TextBox txtPais,
                                    TextBox txtEntidad
                                )
    {
        try
        {
            return Guardar((int)HttpContext.Current.Session["IdPaisEditado"],
                            txtPais.Text,
                            txtEntidad.Text);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al actualizar Pais\n" + ex.Message);
        }
    }//Guardar

    //Método para Guardar un Pais
    public static Boolean Guardar(int Id,
                                String Pais,
                                String Entidad
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!Pais.Equals(String.Empty) &&
            !Entidad.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarPais '" + Id +
                            "','" + Pais +
                            "','" + Entidad +
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
                throw new ArgumentException("Error al actualizar Pais\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar


    //Método para Guardar un Pais (transaccional)
    public static Boolean Guardar(int Id,
                                String Pais,
                                String Entidad,
                                SqlTransaction t
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!Pais.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarPais '" + Id +
                            "','" + Pais +
                            "','" + Entidad +
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
                throw new ArgumentException("Error al actualizar Pais\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar

    //Método para Eliminar un Pais
    public static Boolean Eliminar(int Id)
    {
        try
        {
            String strSQL = "DELETE FROM Pais" +
                              " WHERE Id='" + Id + "'";
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.Actualizar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al eliminar Pais\n" + ex.Message);
        }
    }//Eliminar


    //Obtener la clave primaria de un Pais
    public static int ObtenerId(String Pais)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarPais '" + Pais + "'");
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de País:\n" + ex.Message);
        }
    }

    //Obtener la clave primaria de un Pais (transaccional)
    public static int ObtenerId(String Pais, SqlTransaction t)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerId("EXEC spBuscarPais '" + Pais + "'", t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de País:\n" + ex.Message);
        }
    }

    //Metodo para Obtener la imagen del Logo de la Entidad en binario
    public static Byte[] ObtenerLogoEntidad(int Id)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerImagen("Pais", "LogoEntidad", "Id=" + Id);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //Metodo para guardar la imagen del Logo de la Entidad en binario
    public static Boolean GuardarLogoEntidad(int Id, Byte[] Imagen)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ActualizarImagen("Pais", "LogoEntidad", "Id=" + Id, Imagen);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    //Metodo para Obtener la imagen de la Bandera en binario
    public static Byte[] ObtenerBandera(int Id)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ObtenerImagen("Pais", "Bandera", "Id=" + Id);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //Metodo para guardar la imagen de la Bandera en binario
    public static Boolean GuardarBandera(int Id, Byte[] Imagen)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            //Ejecutar la consulta
            return bd.ActualizarImagen("Pais", "Bandera", "Id=" + Id, Imagen);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

}

