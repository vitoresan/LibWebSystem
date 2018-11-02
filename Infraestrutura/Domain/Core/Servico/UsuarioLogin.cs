namespace Infraestrutura.Domain.Core.Servico
{
    using Infraestrutura.Domain.Interfaces;
    using Infraestrutura.Domain.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;

    public class UsuarioLogin
    {
        IDatabase _database;
        ISessao _sessao;
        IRepositorio _repositorio;

        public UsuarioLogin(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString));
        }

        public bool Logar(string usuario, string senha)
        {
            try
            {
                _sessao = new Sessao(_database);
                _repositorio = new Repositorio(_sessao);

                return Enumerable.Count(_repositorio.Query(gerarSelectUsuarios(), new { usuario, senha })) > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string gerarSelectUsuarios() => "select * from Usuario where login = @usuario and senha = @senha and Se_Ativo = 1";
    }
}
