using System;
using System.Web;
using System.Data;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Text;

public class Grupo
{
    public static void Preparar(int IdCampeonato,
                                GridView gvGrupo
        )
    {
        try
        {
            //Obtener la lista de Grupos para edición
            DataTable tbl = Obtener(IdCampeonato);
            gvGrupo.DataSource = tbl;
            gvGrupo.DataKeyNames = new String[] { "Id" };
            gvGrupo.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }

    public static void PrepararPaises(int IdGrupo,
                                GridView gvSeleccion
        )
    {
        try
        {
            //Obtener la lista de Grupos para edición
            DataTable tbl = ObtenerPaises(IdGrupo);
            gvSeleccion.DataSource = tbl;
            gvSeleccion.DataKeyNames = new String[] { "IdPais" };
            gvSeleccion.DataBind();
        }
        catch (Exception ex)
        {
            throw new ArgumentException(ex.Message);
        }
    }


    //Metodo para listar los Grupos para edición
    public static DataTable Obtener(int IdCampeonato)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarGruposEdicion '" + IdCampeonato + "'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Grupos de un Campeonato:\n"+ex.Message);
        }

    }//Obtener

    //Metodo para listar los Grupos de un Campeonato
    public static DataTable ObtenerLista(int IdCampeonato)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarGrupos '" + IdCampeonato + "'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Grupos de un Campeonato:\n" + ex.Message);
        }
    }//ObtenerLista

    //Metodo para listar los Paises de un Grupo para edición
    public static DataTable ObtenerPaises(int IdGrupo)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarGrupoPaisesEdicion '" + IdGrupo + "'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Selecciones de un Grupo de un Campeonato:\n" + ex.Message);
        }
    }

    //Metodo para listar los Paises de un Grupo
    public static DataTable ObtenerListaPaises(int IdGrupo)
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir la cadena de consulta
            String strSQL = "EXEC spListarPaisesGrupo '" + IdGrupo + "'";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Selecciones de un Grupo de un Campeonato:\n" + ex.Message);
        }
    }

    //Método para preparar la edicion de un Grupo
    public static void IniciarEdicion(int Id,
                                    TextBox txtGrupo,
                                    DropDownList ddlCampeonato
                                    )
    {
        //Se esta editando un Grupo existente?
        if (Id != -1)
        {
            //Consultar el registro actualizado
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            String strSQL = "SELECT *" +
                            " FROM Grupo" +
                            " WHERE Id='" + Id + "'";
            DataTable tbl = bd.Consultar(strSQL);
            if (tbl != null && tbl.Rows.Count > 0)
            {
                //Actualizar los controles con los datos del registro
                DataRow dr = tbl.Rows[0];
                ddlCampeonato.SelectedValue = dr["IdCampeonato"].ToString();
                txtGrupo.Text = dr["Grupo"].ToString();
            }
        }
        else
        {
            //Dejar los controles vacíos
            txtGrupo.Text = "";
            ddlCampeonato.SelectedIndex = -1;
        }
        HttpContext.Current.Session["IdGrupoEditado"] = Id;
    }

    //Método para Guardar un Grupo
    public static Boolean Guardar(TextBox txtGrupo,
                                DropDownList ddlCampeonato
                                )
    {
        try
        {
            return Guardar((int)HttpContext.Current.Session["IdGrupoEditado"],
                            int.Parse(ddlCampeonato.SelectedValue),
                            txtGrupo.Text);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al actualizar Grupo\n" + ex.Message);
        }
    }//Guardar

    //Método para Guardar un Grupo
    public static Boolean Guardar(int Id,
                                int IdCampeonato,
                                String Grupo
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (IdCampeonato > 0 &&
            !Grupo.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarGrupo '" + Id +
                            "','" + Grupo +
                            "','" + IdCampeonato +
                             "'");

            //Ejecutar la consulta
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            try
            {
                Guardado = bd.Actualizar(strSQL.ToString());
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al actualizar Grupo\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar

    //Método para Guardar un Grupo
    public static Boolean Guardar(int Id,
                                int IdCampeonato,
                                String Grupo,
                                SqlTransaction t
                                )
    {
        Boolean Guardado = false;
        //Son válidos todos los datos?
        if (IdCampeonato > 0 &&
            !Grupo.Equals(String.Empty))
        {
            //Construir cadena de consulta
            StringBuilder strSQL = new StringBuilder();
            strSQL.Append("EXEC spActualizarGrupo '" + Id +
                            "','" + Grupo +
                            "','" + IdCampeonato +
                             "'");

            //Ejecutar la consulta
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
            try
            {
                Guardado = bd.Actualizar(strSQL.ToString(), t);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Error al actualizar Grupo\n" + ex.Message);
            }
        }
        return Guardado;
    }//Guardar


    //Método para Eliminar un Grupo
    public static Boolean Eliminar(int Id)
    {
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
        String strSQL = "DELETE FROM Grupo" +
                              " WHERE Id='" + Id + "'";
        return bd.Actualizar(strSQL);
    }//Eliminar


    //Obtener la clave primaria de un Grupo
    public static int ObtenerId(int IdCampeonato,
                                String Grupo)
    {
        //Consultar el registro actualizado
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
        try
        {
            return bd.ObtenerId("EXEC spBuscarGrupo '" + IdCampeonato + "', '" + Grupo + "'");
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Grupo:\n" + ex.Message);
        }

    }//ObtenerId

    //Obtener la clave primaria de un Grupo
    public static int ObtenerId(int IdCampeonato,
                                String Grupo,
                                SqlTransaction t)
    {
        //Consultar el registro actualizado
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];
        try
        {
            return bd.ObtenerId("EXEC spBuscarGrupo '" + IdCampeonato + "', '" + Grupo + "'", t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al obtener clave primaria de Grupo:\n" + ex.Message);
        }
    }//ObtenerId

    //Metodo para verificar que el registro de un pais en un grupo
    public static Boolean VerificarGrupoPais(int IdGrupo, int IdPais)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        String strSQL = "SELECT * FROM GrupoPais WHERE IdGrupo='" + IdGrupo +
                        "' AND IdPais='" + IdPais + "'";

        try
        {
            DataTable tbl = bd.Consultar(strSQL);
            //verificar que la consulta devuelve registros
            if (tbl != null && tbl.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al buscar un Pais en un Grupo:\n" + ex.Message);
        }
    }//VerificarGrupoPais


    //Metodo para verificar que el registro de un pais en un grupo
    public static Boolean VerificarGrupoPais(int IdGrupo, int IdPais,
                                            SqlTransaction t)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        String strSQL = "SELECT * FROM GrupoPais WHERE IdGrupo='" + IdGrupo +
                        "' AND IdPais='" + IdPais + "'";

        try
        {
            DataTable tbl = bd.Consultar(strSQL, t);
            //verificar que la consulta devuelve registros
            if (tbl != null && tbl.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al buscar un Pais en un Grupo:\n" + ex.Message);
        }
    }//VerificarGrupoPais

    //Metodo para Agregar un pais a un Grupo
    public static Boolean AgregarGrupoPais(int IdGrupo, int IdPais)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        String strSQL = "INSERT INTO GrupoPais (IdGrupo, IdPais) VALUES('" + IdGrupo +
                        "', '" + IdPais + "')";

        try
        {
            return bd.Actualizar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al agregar un Pais en un Grupo:\n" + ex.Message);
        }
    }//AgregarGrupoPais

    //Metodo para Agregar un pais a un Grupo
    public static Boolean AgregarGrupoPais(int IdGrupo, int IdPais,
                                            SqlTransaction t)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        String strSQL = "INSERT INTO GrupoPais (IdGrupo, IdPais) VALUES('" + IdGrupo +
                        "', '" + IdPais + "')";

        try
        {
            return bd.Actualizar(strSQL, t);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al agregar un Pais en un Grupo:\n" + ex.Message);
        }
    }//AgregarGrupoPais


}

