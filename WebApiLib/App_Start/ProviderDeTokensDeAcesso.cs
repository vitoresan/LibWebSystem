namespace WebApiLib.App_Start
{
    using Infraestrutura.Domain.Core.Servico;
    using Microsoft.Owin.Security.OAuth;
    using System.Security.Claims;
    using System.Threading.Tasks;

    public class ProviderDeTokensDeAcesso : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) => context.Validated();

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            if ((new UsuarioLogin(BdConfig.Connection).Logar(context.UserName, context.Password)))
            {
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                // identity.AddClaim(new Claim("sub", context.UserName));
                // identity.AddClaim(new Claim("role", "user"));
                context.Validated(identity);
            }
            else
            {
                context.SetError("acesso inválido", "As credenciais do usuário não conferem....");
                return;
            }
        }
    }
}