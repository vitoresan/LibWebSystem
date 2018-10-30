using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestrutura.Domain.Interfaces
{
    public interface IRepositorio
    {
        ISessao Sessao { get; }

        Int32 Execute(String sql, dynamic param = null);

        IEnumerable<T> Query<T>(String sql, dynamic param = null, Boolean buffered = true);

        IEnumerable<TReturn> Query<TFirst, TSecond, TReturn>(String sql, Func<TFirst, TSecond, TReturn> map, dynamic param = null, IDbTransaction transaction = null, Boolean buffered = true, String splitOn = "Id", Int32? commandTimeout = null);

        IEnumerable<dynamic> Query(String sql, dynamic param = null, Boolean buffered = true);

        SqlMapper.GridReader QueryMultiple(String sql, dynamic param = null, IDbTransaction transaction = null, Int32? commandTimeout = null, CommandType? commandType = null);

    }
}
