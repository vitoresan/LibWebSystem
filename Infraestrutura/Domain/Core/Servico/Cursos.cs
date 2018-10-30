using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infraestrutura.Domain.Core.Servico
{
    public class Cursos
    {
        IDatabase _database;
        ISessao _sessao;
        IRepositorio _repositorio;

        public Cursos(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString));
        }

        public List<Curso> RetornarCursos()
        {
            string query = $@"
                SELECT * FROM DBO.CURSOS ORDER BY CURSOS.DESCR
            ";

            try
            {
                _sessao = new Sessao(_database);
                _repositorio = new Repositorio(_sessao);
                return _repositorio.Query<Curso>(query).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
    }
}
