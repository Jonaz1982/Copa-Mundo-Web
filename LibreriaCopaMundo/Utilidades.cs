using System;
using System.Web;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Data;

public class Utilidades
{
    //Muestra un mensaje en una tabla HTML en el documento activo
    public static void Mensaje(String Texto)
    {
        HttpContext.Current.Response.Write("<table border='0'>");
        HttpContext.Current.Response.Write("<tr><td align='center' bgcolor='SeaGreen'><span style='color:White; font-family: Arial'><strong>Mensaje</strong></span></td></tr>");
        HttpContext.Current.Response.Write("<td bgcolor='DarkSeaGreen'><span style='font-family: Arial'>" +Texto.Replace("\n","<br>"));
        HttpContext.Current.Response.Write("</td></tr></table>");
    
    }

    // Encripta una cadena de texto
    public static String Encriptar(String Texto)
    {
        byte[] caracteres = Encoding.Unicode.GetBytes(Texto);
        return Convert.ToBase64String(caracteres);
    }

    // Desencripta una cadena de texto
    public static String Desencriptar(String Texto)
    {
        byte[] caracteres = Convert.FromBase64String(Texto);
        //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
        return Encoding.Unicode.GetString(caracteres);
    }

    //Metodo para descargar un archivo
    public static Boolean DescargarArchivo(String Ruta, String Archivo)
    {
        //Nombres de archivo
        FileInfo NombreArchivo = new FileInfo(Ruta + "\\" + Archivo);
        FileStream NombreArchivoBinario = new FileStream(Ruta + "\\" + Archivo, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

        //Leer el archivo como valores binarios
        BinaryReader br = new BinaryReader(NombreArchivoBinario);

        //El archivo a descargar existe
        if (NombreArchivo.Exists)
        {
            try
            {
                long startBytes = 0;
                String lastUpdateTiemStamp = File.GetLastWriteTimeUtc(Ruta).ToString("r");
                String _EncodedData = HttpUtility.UrlEncode(Archivo, Encoding.UTF8) + lastUpdateTiemStamp;

                HttpContext.Current.Response.Clear();
                HttpContext.Current.Response.Buffer = false;
                HttpContext.Current.Response.AddHeader("Accept-Ranges", "bytes");
                HttpContext.Current.Response.AppendHeader("ETag", "\"" + _EncodedData + "\"");
                HttpContext.Current.Response.AppendHeader("Last-Modified", lastUpdateTiemStamp);
                HttpContext.Current.Response.ContentType = "application/octet-stream";
                HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;NombreArchivo=" + NombreArchivo.Name);
                HttpContext.Current.Response.AddHeader("Content-Length", (NombreArchivo.Length - startBytes).ToString());
                HttpContext.Current.Response.AddHeader("Connection", "Keep-Alive");
                HttpContext.Current.Response.ContentEncoding = Encoding.UTF8;

                //Enviar datos
                br.BaseStream.Seek(startBytes, SeekOrigin.Begin);

                //La información se divide en paquetes de 1024 bytes
                int maxCount = (int)Math.Ceiling((NombreArchivo.Length - startBytes + 0.0) / 1024);

                //La información se descarga en paquetes de 1024 bytes
                int i;
                for (i = 0; i < maxCount && HttpContext.Current.Response.IsClientConnected; i++)
                {
                    HttpContext.Current.Response.BinaryWrite(br.ReadBytes(1024));
                    HttpContext.Current.Response.Flush();
                }
                //Si no se transfirieron todos los paquetes
                if (i < maxCount)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                HttpContext.Current.Response.End();
                br.Close();
                NombreArchivoBinario.Close();
            }
        }
        return false;
    }

    // Método estático para cargar imágenes desde un archivo a un vector de octetos usando una aplicación web
    public static Byte[] CargarImagen(FileUpload fu)
    {
        //Obtener contenido del archivo
        Stream s = fu.PostedFile.InputStream;
        MemoryStream ms=new MemoryStream();
        s.CopyTo(ms);
        //Copiarlo a un vector de octetos
        return ms.ToArray();

    }


    public static void CambiarPagina(GridView dg, int Pagina, DataTable tbl)
    {
        dg.DataSource = tbl;
        dg.PageIndex = Pagina;
        dg.DataBind();
    }

    public static DateTime ObtenerFecha(DataRow dr, String Campo)
    {
        try
        {
            return (DateTime)dr[Campo];
        }
        catch
        {
            return DateTime.Now;
        }
    }

}
