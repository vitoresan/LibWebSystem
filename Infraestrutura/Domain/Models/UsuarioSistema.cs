namespace Infraestrutura.Domain.Models
{
    public class UsuarioSistema : Usuario
    {
        public string Login { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
    }
}
