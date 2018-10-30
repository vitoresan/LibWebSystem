using System.Web;
using System.Web.Configuration;

namespace WebApiLib
{
    public static class BdConfig
    {
        static BdConfig()
        {
#if DEBUG
            Connection = WebConfigurationManager.ConnectionStrings["local"].ConnectionString;
#else
            Connection = WebConfigurationManager.ConnectionStrings["prod"].ConnectionString;
#endif
        }

        public static string Connection { get; private set; }
        public static string UsuarioRequisicao => HttpContext.Current.User.Identity.Name;
    }
}