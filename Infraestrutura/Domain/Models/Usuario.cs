using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Infraestrutura.Domain.Models
{
    public class Usuario
    {
        public Usuario()
        {

        }

        [JsonConstructor]
        public Usuario(string nome, string telefone, string email, string cpf, bool se_Ativo, List<TipoUsuario> tipo)
        {
            Nome = nome;
            Telefone = telefone;
            Email = email;
            CPF = cpf;
            Se_Ativo = se_Ativo;
            Tipo = tipo;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public string CPF { get; set; }
        public bool Se_Ativo { get; set; }
        public string Data_Cadastro { get; set; }
        public List<TipoUsuario> Tipo { get; set; }
    }
}
