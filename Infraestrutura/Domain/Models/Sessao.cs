namespace Infraestrutura.Domain.Models
{
    using Infraestrutura.Domain.Interfaces;
    using System;
    using System.Data;

    public class Sessao : ISessao
    {
        private IDbConnection _conexao;
        private Int32 _timeout;
        private IDbTransaction _transacao;

        public IDbConnection Conexao { get { return _conexao; } }
        public Int32 Timeout { get { return _timeout; } }
        public IDbTransaction Transacao { get { return _transacao; } }

        public Sessao(IDatabase database)
        {
            _conexao = database.Conexao;
        }

        public Sessao(IDatabase database, Int32 timeout)
        {
            _conexao = database.Conexao;
            _timeout = timeout;
        }

        public void Begin(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            if (_conexao.State != ConnectionState.Open)
                _conexao.Open();
            _transacao = _conexao.BeginTransaction(isolation);
        }

        public void Commit()
        {
            _transacao.Commit();
            _transacao = null;
        }

        public void Rollback()
        {
            _transacao.Rollback();
            _transacao = null;
        }

        public void Close(IsolationLevel isolation = IsolationLevel.ReadCommitted)
        {
            if (_conexao.State != ConnectionState.Closed)
                _conexao.Close();
        }

        public void Dispose()
        {
            if (_conexao.State != ConnectionState.Closed)
            {
                if (_transacao != null)
                    _transacao.Rollback();
                _conexao.Close();
                _conexao = null;
            }
            GC.SuppressFinalize(this);
        }

    }
}
