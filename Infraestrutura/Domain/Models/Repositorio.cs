using Dapper;
using Infraestrutura.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;

namespace Infraestrutura.Domain.Models
{
    public class Repositorio : IRepositorio
    {

        private ISessao _sessao;

        public ISessao Sessao { get { return _sessao; } }

        public Repositorio(ISessao session)
        {
            _sessao = session;
        }

        public Int32 Execute(String sql, dynamic param = null)
        {
            return SqlMapper.Execute(_sessao.Conexao, sql, param as object, _sessao.Transacao, commandTimeout: _sessao.Timeout);
        }

        public IEnumerable<T> Query<T>(String sql, dynamic param = null, Boolean buffered = true)
        {
            return SqlMapper.Query<T>(_sessao.Conexao, sql, param as Object, _sessao.Transacao, buffered, _sessao.Timeout);
        }

        public IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(String sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, Boolean buffered = true, String splitOn = "Id", Int32? commandTimeout = null)
        {
            return SqlMapper.Query(_sessao.Conexao, sql, map, param as object, transaction, buffered, splitOn);
        }

        public IEnumerable<dynamic> Query(String sql, dynamic param = null, Boolean buffered = true)
        {
            return SqlMapper.Query(_sessao.Conexao, sql, param as object, _sessao.Transacao, buffered);
        }

        public Dapper.SqlMapper.GridReader QueryMultiple(string sql, dynamic param = null, IDbTransaction transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return SqlMapper.QueryMultiple(_sessao.Conexao, sql, param, transaction, commandTimeout, commandType);
        }
    }
}
