using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Infraestrutura.Domain.Models
{
    public class Usuario
    {
        [Obsolete]
        public Usuario()
        {

        }

        [JsonConstructor]
        public Usuario(string nome, string telefone, string email, string cpf, bool se_Ativo, List<int> id_Tipo_Usuario)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            CPF = cpf;
            Se_Ativo = se_Ativo;
            Id_Tipo_Usuario = id_Tipo_Usuario;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public bool Se_Ativo { get; set; }
        public string Data_Cadastro { get; set; }
        public List<int> Id_Tipo_Usuario { get; set; }
    }
}
