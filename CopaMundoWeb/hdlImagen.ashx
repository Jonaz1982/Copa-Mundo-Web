<%@ WebHandler Language="C#" Class="hdlImagen" %>

using System;
using System.Web;

using System.Web.SessionState;

public class hdlImagen : IHttpHandler, IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        if (context.Request.QueryString["Id"] != null &&
            context.Request.QueryString["Campo"] != null)
        {
            //Obtener la imagen
            byte[] bImagen=null;
            switch (context.Request.QueryString["Campo"])
            {
                case "Logo":
                    bImagen = Campeonato.ObtenerLogo(int.Parse(context.Request.QueryString["Id"]));
                    break;
                case "Foto":
                    bImagen =Usuario.ObtenerFoto(int.Parse(context.Request.QueryString["Id"]));
                    break;
                case "LogoEntidad":
                    bImagen = Pais.ObtenerLogoEntidad(int.Parse(context.Request.QueryString["Id"]));
                    break;
                case "Bandera":
                    bImagen = Pais.ObtenerBandera(int.Parse(context.Request.QueryString["Id"]));
                    break;
                case "FotoEstadio":
                    bImagen = Estadio.ObtenerFoto(int.Parse(context.Request.QueryString["Id"]));
                    break;
            }
            if (bImagen != null)
            {
                //Definir tipo de salida
                context.Response.ContentType = "image/jpeg";
                //Mostrar la imagen
                context.Response.BinaryWrite(bImagen);
            }
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}