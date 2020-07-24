using System;
using System.Data;
using System.Web;

using System.IO;
using System.Data.SqlClient;

public class Campeonato
{


    public static DataTable ObtenerCampeonatos()
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        //Cadena de consulta
        String strSQL = "EXEC spListarCampeonatosEdicion";

        //Ejecutar consulta
        return bd.Consultar(strSQL);


    }

    //Metodo para listar los Campeonatos 
    public static DataTable ObtenerLista()
    {
        try
        {
            //Recuperar el objeto para consultas a la base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Definir cadena de consulta
            String strSQL = "EXEC spListarCampeonatos";

            //Retornar el resultado de la consulta
            return bd.Consultar(strSQL);
        }
        catch (Exception ex)
        {
            throw new ArgumentException("Error al listar Campeonatos:\n" + ex.Message);
        }
    }//ObtenerLista

    public static byte[] ObtenerLogo(int Id)
    {
        //Recuperar el objeto para consultas a la base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        //Ejecutar consulta
        return bd.ObtenerImagen("Campeonato", "Logo", "Id=" + Id);
    }


    public static Boolean ActualizarLogo(int Id, byte[] Imagen)
    {
        //recuperar el objeto para consultas a ala base de datos
        BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

        //ejecuta consulta
        return bd.ActualizarImagen("Campeonato", "Logo", "Id=" + Id, Imagen);
    }


    public static String Importar(int IdCampeonato,
                                            StreamReader sr)
    {
        SqlTransaction t = null;
        try
        {

            //recuperar el objeto para consultas a ala base de datos
            BaseDatos bd = (BaseDatos)HttpContext.Current.Session["bd"];

            //Iniciar la transacción
            t = bd.Conexion.BeginTransaction();

            //Leer la primera linea
            String linea = sr.ReadLine();
            //Ignorar encabezados
            if (linea.ToLower().Contains("grupo"))
                linea = sr.ReadLine();

            //Variable para detectar cambio de grupo
            String anteriorGrupo = "";
            int IdGrupo = -1;
            while (linea != null)
            {
                String[] datos = linea.Split(';');
                if (datos.Length == 2)
                {
                    //Verificar cambio de Grupo
                    if (!anteriorGrupo.Equals(datos[0]))
                    {
                        //Importando grupos
                        IdGrupo = Grupo.ObtenerId(IdCampeonato, datos[0], t);
                        //Si no existe el grupo, agregarlo
                        if (IdGrupo == -1)
                        {
                            Grupo.Guardar(-1, IdCampeonato, datos[0], t);
                            IdGrupo = Grupo.ObtenerId(IdCampeonato, datos[0], t);
                        }
                        anteriorGrupo = datos[0];
                    }

                    //Importando Paises
                    int IdPais = Pais.ObtenerId(datos[1], t);
                    //Si no existe el Pais, agregarlo
                    if (IdPais == -1)
                    {
                        Pais.Guardar(-1, datos[1], "Sin Entidad", t);
                        IdPais = Pais.ObtenerId(datos[1], t);
                    }

                    //Importar GrupoPais
                    //Verificar si ya esta registrado el pais en el grupo
                    if(!Grupo.VerificarGrupoPais(IdGrupo, IdPais, t))
                    {
                        Grupo.AgregarGrupoPais(IdGrupo, IdPais, t);
                    }

                }
                else
                {
                    //Importando encuentros

                }

                //Leer siguiente linea
                linea = sr.ReadLine();
            }


            //Finalizar transacción con éxito
            t.Commit();

            return "";
        }
        catch (Exception ex)
        {
            //Devolver los cambios de la transacción
            if (t != null)
            {
                t.Rollback();
            }
            return "Error importando Campeonato:\n" + ex.Message;
            
        }
    }

}

