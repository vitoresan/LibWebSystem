namespace Infraestrutura.Domain.Interfaces
{
    using System.Data;

    public interface IDatabase
    {
        IDbConnection Conexao { get; }
    }
}
