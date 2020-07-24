using System;
using System.IO;
using System.Web.Hosting;
using System.Web;


public class Conexion
{
    public static Boolean Establecer()
    {
        Boolean Establecida = false;

        BaseDatos bd = new BaseDatos();

        //Abrir el archivo con la cadena de conexion
        StreamReader sr = Archivo.AbrirArchivo(HostingEnvironment.MapPath("~/") +
                                    "Conexion.txt");
        //Se pudo abrir el archivo
        if (sr != null)
        {
            //String[] lineas = new String[1];
            //lineas[0] = Utilidades.Encriptar(sr.ReadLine());
            //Archivo.GuardarArchivo(HostingEnvironment.MapPath("~/") +
            //                        "Conexion.txt", lineas);

            //Asignar la cadena de conexion
            bd.CadenaConexion = Utilidades.Desencriptar(sr.ReadLine());

            Establecida = bd.Conectar();

        }
        HttpContext.Current.Session["bd"] = bd;

        return Establecida;

    }

}

