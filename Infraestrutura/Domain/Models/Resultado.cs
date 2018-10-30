using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models.Enum;

namespace Infraestrutura.Domain.Models
{
    public class Resultado<T> : IResultado<T>
    {
        public Resultado(string mensagem, statusRetorno status, T valor)
        {
            Valor = valor;
            Status = status;
            Mensagem = mensagem;
        }

        public string Mensagem
        {
            get;
        }

        public statusRetorno Status
        {
            get;
        }

        public T Valor
        {
            get;
        }
    }

    public class Resultado : IResultado
    {
        public string Mensagem
        {
            get;
        }

        public statusRetorno Status
        {
            get;
        }

        public Resultado(string mensagem, statusRetorno status)
        {
            Status = status;
            Mensagem = mensagem;
        }
    }
}
