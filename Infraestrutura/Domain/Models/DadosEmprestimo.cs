using Newtonsoft.Json;
using System;

namespace Infraestrutura.Domain.Models
{
    public class DadosEmprestimo
    {
        [Obsolete]
        public DadosEmprestimo()
        {

        }

        [JsonConstructor]
        public DadosEmprestimo(int id, int id_Livro, int id_Pessoa, DateTime data_Emprestimo, DateTime data_Devolucao_Sugerida, DateTime? data_Devolucao_Realizada, bool? se_Renovacao, int? id_Usuario_Logado, string obs)
        {
            Id = id;
            Id_Livro = id_Livro;
            Id_Pessoa = id_Pessoa;
            Data_Emprestimo = data_Emprestimo;
            Data_Devolucao_Sugerida = data_Devolucao_Sugerida;
            Data_Devolucao_Realizada = data_Devolucao_Realizada;
            Se_Renovacao = se_Renovacao;
            Id_Usuario_Logado = id_Usuario_Logado;
            Obs = obs;
        }

        public int Id { get; set; }
        public int Id_Livro { get; set; }
        public int Id_Pessoa { get; set; }
        public DateTime Data_Emprestimo { get; set; }
        public DateTime Data_Devolucao_Sugerida { get; set; }
        public DateTime? Data_Devolucao_Realizada { get; set; }
        public bool? Se_Renovacao { get; set; }
        public int? Id_Usuario_Logado { get; set; }
        public string Obs { get; set; }
    }
}
