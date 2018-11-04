using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infraestrutura.Domain.Core.Servico
{
    public class Emprestimo
    {
        IDatabase _database;
        ISessao _sessao;
        IRepositorio _repositorio;

        public Emprestimo(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString));
        }

        internal IResultado InserirEmprestimoBanco(DadosEmprestimo dadosEmprestimo)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);
            try
            {
                _sessao.Begin();
                var Data_Emprestimo = DateTime.Now;
                var idExemplarEmprestado = _repositorio.Query<int>(gerarInsertEmprestimo(), new
                {
                    dadosEmprestimo.Id_Livro,
                    dadosEmprestimo.Id_Pessoa,
                    dadosEmprestimo.Data_Devolucao_Sugerida,
                    dadosEmprestimo.Id_Usuario_Logado,
                    dadosEmprestimo.Se_Renovacao,
                    Data_Emprestimo
                }).FirstOrDefault();

                atualizarControleExemplar(idExemplarEmprestado, Data_Emprestimo, true, _sessao, _repositorio);

                _sessao.Commit();

                return new Resultado($"Empréstimo de livro realizado com sucesso.", statusRetorno.OK);
            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                return new Resultado($"Ocorreram erros ao realizar empréstimo de livro. Detalhes: {ex.Message}", statusRetorno.Erro);
            }
        }


        private void atualizarControleExemplar(int idExemplarEmprestado, DateTime data_Emprestimo, bool seEmprestimo, ISessao sessao, IRepositorio repositorio)
        {
            _repositorio.Execute(gerarUpdateControleExemplar(), new { idExemplarEmprestado, seEmprestimo, data_Emprestimo });
        }

        private string gerarUpdateControleExemplar() => @"
            UPDATE [dbo].[Controle_Exemplares]
               SET [Se_Emprestado] = @seEmprestimo
                  ,[Data_Emprestimo] = @data_Emprestimo
             WHERE [Id] = @idExemplarEmprestado
            ";

        internal IResultado InserirDevolucaoBanco(DadosEmprestimo dadosDevolucao)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);
            try
            {
                _sessao.Begin();

                _repositorio.Execute(gerarInsertEmprestimo(), new
                {
                    dadosDevolucao.Id,
                    dadosDevolucao.Id_Livro,
                    dadosDevolucao.Id_Pessoa,
                    dadosDevolucao.Data_Emprestimo,
                    dadosDevolucao.Data_Devolucao_Sugerida,
                    Data_Devolucao_Realizada = DateTime.Now,
                    dadosDevolucao.Id_Usuario_Logado,
                });

                _sessao.Commit();

                return new Resultado($"Devolução de livro realizada com sucesso.", statusRetorno.OK);
            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                return new Resultado($"Ocorreram erros ao realizar devolução de livro. Detalhes: {ex.Message}", statusRetorno.Erro);
            }
        }

        internal ControleLivro ConsultaDisponibilidadeLivro(int idLivro)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);

            return _repositorio.Query<ControleLivro>(gerarSelectDisponibilidadeLivro(), new
            {
                idLivro
            }).FirstOrDefault();
        }

        private string gerarInsertEmprestimo() => @"
                 DECLARE @idEmprestimo = (SELECT isNull(max(id),0) + 1 FROM [controle].[Emprestimo])
                 DECLARE @idExemplar = (SELECT TOP 1 ID FROM [dbo].[Controle_Exemplares] where Id_Livro = @idLivro and Se_Emprestado = 1)

                 INSERT INTO [controle].[Emprestimo]
                       ([Id]
                       ,[Id_Livro]
                       ,[Id_Pessoa]
                       ,[Data_Emprestimo]
                       ,[Data_Devolucao_Sugerida]
                       ,[Se_Renovacao]
                       ,[Id_Usuario_Logado]
                       ,[Id_Controle_Exemplar])
                 VALUES
                       (@idEmprestimo
                       ,@Id_Livro
                       ,@Id_Pessoa
                       ,@Data_Emprestimo
                       ,@Data_Devolucao_Sugerida
                       ,@Se_Renovacao
                       ,@Id_Usuario_Logado
                       ,@idExemplar)

                SELECT @idExemplar
            ";

        private string gerarInsertDevolucao() => @"

                 INSERT INTO [controle].[Emprestimo]
                       ([Id]
                       ,[Id_Livro]
                       ,[Id_Pessoa]
                       ,[Data_Emprestimo]
                       ,[Data_Devolucao_Sugerida]
                       ,[Data_Devolucao_Realizada]
                       ,[Se_Renovacao]
                       ,[Id_Usuario_Logado]
                       ,[Id_Controle_Exemplar])
                 VALUES
                       (@Id
                       ,@Id_Livro
                       ,@Id_Pessoa
                       ,@Data_Emprestimo
                       ,@Data_Devolucao_Sugerida
                       ,@Data_Devolucao_Realizada
                       ,@Se_Renovacao
                       ,@Id_Usuario_Logado
                       ,@Id_Controle_Exemplar)

                SELECT @idExemplar
            ";

        private string gerarSelectDisponibilidadeLivro() => @"
             ;with LivroAtivo as (
                  SELECT Id, max(data__Cadastro)DataCadastro FROM DBO.LIVROS
                 group by Id
             )
             select livro.Id
            	  , livro.Titulo
            	  , livro.Qtd_Exemplares
            	  , Qtd_Dentro = (select count(*) from dbo.Controle_Exemplares where Id_Livro = @idLivro and Se_Emprestado = 0)
            	  , Qtd_Fora = (select count(*) from dbo.Controle_Exemplares where Id_Livro = @idLivro and Se_Emprestado = 1)
              from LivroAtivo ll
            	inner join dbo.Livros livro on livro.Id = ll.Id and livro.Data__Cadastro = ll.DataCadastro
            where livro.Id = @idLivro
            ";
    }
}
