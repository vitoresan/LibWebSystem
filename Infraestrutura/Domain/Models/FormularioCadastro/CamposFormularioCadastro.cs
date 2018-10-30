using System.Collections.Generic;

namespace Infraestrutura.Domain.Models.FormularioCadastro
{
    public class CamposCadastroLivro
    {
        public List<Curso> Cursos { get; set; }
        public List<Biografia_Tipo> Biografias_Tipo { get; set; }
        public List<Esquema_Codificacao> Esquemas_Codificacao { get; set; }
        public List<Forma_Catalogacao_Descritiva> Formas_Catalogacao_Descritiva { get; set; }
        public List<Forma_Item> Formas_Item { get; set; }
        public List<Forma_Literaria> Formas_Literaria { get; set; }
        public List<Forma_Material> Formas_Material { get; set; }
        public List<Ilustracao_Tipo> Ilustracoes_Tipo { get; set; }
        public List<Natureza_Conteudo> Naturezas_Conteudo { get; set; }
        public List<Nivel_Bibliografico> Niveis_Bibliograficos { get; set; }
        public List<Nivel_Codificacao> Niveis_Codificacao { get; set; }
        public List<Nivel_Varias_Partes> Niveis_Varias_Partes { get; set; }
        public List<Publicacao_Governamental_Tipo> Publicacao_Governamental_Tipos { get; set; }
        public List<Publico_Alvo> Publicos_Alvos { get; set; }
        public List<Status_Registro> Status_Registro { get; set; }
        public List<Tipo_Controle> Tipos_Controle { get; set; }
        public List<Tipo_Registro> Tipos_Registro { get; set; }
    }
}
