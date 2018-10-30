using Infraestrutura.Domain.Models.FormularioCadastro;
using Newtonsoft.Json;
using System;

namespace Infraestrutura.Domain.Models
{
    public class Livro
    {

        [Obsolete]
        public Livro()
        {

        }

        [JsonConstructor]
        public Livro(string titulo, string autor, int num_paginas, string ano_Publicacao, string local_Publicacao, string endereco, CamposCadastroLivro camposLivro, string cdd, int qtd_Exemplares)
        {
            Titulo = titulo;
            Autor = autor;
            Num_Paginas = num_paginas;
            CamposLivro = camposLivro;
            Endereco = endereco;
            Ano_Publicacao = ano_Publicacao;
            Local_Publicacao = local_Publicacao;
            CDD = cdd;
            Qtd_Exemplares = qtd_Exemplares;
        }

        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public int Num_Paginas { get; set; }
        public string Endereco { get; set; }
        public string Ano_Publicacao { get; set; }
        public string Local_Publicacao { get; set; }
        public string CDD { get; set; }
        public bool Se_Emprestado { get; set; }
        public int Qtd_Exemplares { get; set; }
        public CamposCadastroLivro CamposLivro { get; set; }
    }
}
