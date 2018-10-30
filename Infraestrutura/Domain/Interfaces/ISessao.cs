namespace Infraestrutura.Domain.Interfaces
{
    using System;
    using System.Data;

    public interface ISessao : IDisposable
    {
        IDbConnection Conexao { get; }

        Int32 Timeout { get; }

        IDbTransaction Transacao { get; }

        void Begin(IsolationLevel isolation = IsolationLevel.ReadCommitted);

        void Commit();

        void Rollback();
    }
}
