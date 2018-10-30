namespace Infraestrutura.Domain.Models
{
    using Infraestrutura.Domain.Interfaces;
    using System.Data;

    public class Database : IDatabase
    {
        private readonly IDbConnection _conexao;
        public IDbConnection Conexao { get { return _conexao; } }

        public Database(IDbConnection connection)
        {
            _conexao = connection;
        }
    }
}