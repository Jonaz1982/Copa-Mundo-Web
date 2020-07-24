using System;
using System.Data;

using System.Web;
using System.Web.UI.WebControls;
using System.Text;

public class Usuario
{

    public static Boolean ValidarAcceso(String Usuario, String Clave)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        //Establecer el estado de la sesión en un valor predeterminado
        HttpContext.Current.Session["SesionIniciada"] = false;

        //Cadena de consulta
        String strSQL = "EXEC spValidarAcceso '" + Usuario +
                        "', '" + Clave + "'";
        //Ejecutar la consulta
        DataTable tbl = bd.Consultar(strSQL);

        //Si la consulta devuelve registros, el acceso es válido
        if (tbl != null && tbl.Rows.Count > 0)
        {
            HttpContext.Current.Session["SesionIniciada"] = true;
            HttpContext.Current.Session["IdUsuario"] = (int)tbl.Rows[0]["Id"];
            HttpContext.Current.Session["Nombre"] = tbl.Rows[0]["Nombre"].ToString();
            return true;
        }
        else
            return false;
    }

    //Metodo para listar los Usuarios para edición
    public static DataTable Obtener()
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        //Definir la cadena de consulta
        String strSQL = "EXEC spListarUsuariosEdicion";

        //Retornar el resultado de la consulta
        return bd.Consultar(strSQL);

    }

    //Metodo para Obtener la imagen del Foto en binario
    public static Byte[] ObtenerFoto(int Id)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
        try
        {
            return bd.ObtenerImagen("Usuario", "Foto", "Id=" + Id);
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //Metodo para guardar la imagen del Foto en binario
    public static Boolean GuardarFoto(int Id, Byte[] Imagen)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
        try
        {
            return bd.ActualizarImagen("Usuario", "Foto", "Id=" + Id, Imagen);
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    //Método para preparar la edicion de un Usuario
    public static void IniciarEdicion(int Id,
                                    TextBox txtUsuario,
                                    TextBox txtNombre
                                    )
    {
        //Se esta editando un Usuario existente?
        if (Id != -1)
        {
            //Consultar el registro actualizado
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Usuario" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                txtUsuario.Text = dr["Usuario"].ToString();
                txtNombre.Text = dr["Nombre"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            txtUsuario.Text = "";
            txtNombre.Text = "";
        }
        HttpContext.Current.Session["IdUsuarioEditado"] = Id;
    }

    //Método para Guardar un Usuario
    public static Boolean Guardar(TextBox txtUsuario,
                                TextBox txtNombre
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (!txtUsuario.Text.Equals(String.Empty) &&
            !txtNombre.Text.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarUsuario '" + ((int)HttpContext.Current.Session["IdUsuarioEditado"]).ToString() +
                            "','" + txtUsuario.Text +
                            "','" + txtNombre.Text +
                             "'");

            //Ejecutar la consulta
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            Guardado = bd.Actualizar(strSQL.ToString());
        }
        return Guardado;
    }

    //Método para Eliminar un Usuario
    public static Boolean Eliminar(int Id)
    {
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
        String strSQL = "DELETE FROM Usuario" +
                              " WHERE Id='" + Id + "'";
        return bd.Actualizar(strSQL);
    }


}
