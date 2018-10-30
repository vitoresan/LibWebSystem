using Infraestrutura.Domain.Models.Enum;

namespace Infraestrutura.Domain.Interfaces
{
    public interface IResultado
    {
        statusRetorno Status { get; }

        string Mensagem { get; }
    }

    public interface IResultado<T> : IResultado
    {
        T Valor { get; }
    }
}
