using System;
using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.Enum;

namespace Infraestrutura.Domain.Core.Servico
{
    public class ServicoEmprestimo
    {
        private Emprestimo _bdEmprestimo;

        public ServicoEmprestimo(string connection)
        {
            _bdEmprestimo = new Emprestimo(connection);
        }

        public IResultado RealizaEmprestimo(DadosEmprestimo dadosEmprestimo)
        {
            if(ConsultaDisponibilidadeLivro(dadosEmprestimo.Id_Livro).Dentro == 0)
                return new Resultado("Operação não realizada. Livro não disponível para empréstimo.", statusRetorno.Erro);

            return _bdEmprestimo.InserirEmprestimoBanco(dadosEmprestimo);
        }

        public ControleLivro ConsultaDisponibilidadeLivro(int idLivro)
        {
            return _bdEmprestimo.ConsultaDisponibilidadeLivro(idLivro);
        }

        public IResultado RealizaDevolucao(DadosEmprestimo dadosDevolucao)
        {
            return _bdEmprestimo.InserirDevolucaoBanco(dadosDevolucao);
        }

        //public IResultado ConsultaEmprestimo()
        //{
        //    return _bdEmprestimo.ConsultaEmprestimo();
        //}

        //public IResultado ConsultaEmprestimoPorId()
        //{
        //    return _bdEmprestimo.ConsultaEmprestimoPorId();
        //}
    }
}
