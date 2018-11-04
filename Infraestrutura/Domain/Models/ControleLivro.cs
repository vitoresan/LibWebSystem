namespace Infraestrutura.Domain.Models
{
    public class ControleLivro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int Qtd_Exemplares { get; set; }
        public int Dentro { get; set; }
        public int Fora { get; set; }
    }
}
