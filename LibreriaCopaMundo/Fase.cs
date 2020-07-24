using System;
using System.Data;
using System.Web;

public class Fase
{

    //Metodo para listar los Fases 
    public static DataTable ObtenerLista()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarFases";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Fases:\n" + ex.Message);
        }
    }

}
