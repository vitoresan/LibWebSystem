using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infraestrutura.Conexao
{
    public static class DbConfig
    {
        static DbConfig()
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
