using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.Enum;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infraestrutura.Domain.Core.Servico
{
    public class Usuario
    {
        IDatabase _database;
        ISessao _sessao;
        IRepositorio _repositorio;

        public Usuario(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString));
        }

        public IResultado CadastrarUsuario(Models.Usuario usuario)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);

            try
            {
                _sessao.Begin();

                InserirUsuarioBd(usuario, _sessao, _repositorio);

                _sessao.Commit();

                return new Resultado($"O usuário {usuario.Nome}, foi cadastrado com sucesso.", statusRetorno.OK);
            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                return new Resultado($"Ocorreram erros ao cadastrar o usuário. Detalhes: {ex.Message}", statusRetorno.Erro);
            }
        }

        public List<Models.Usuario> RetornarUsuarios(int idTipoUsuario)
        {
            try
            {
                _sessao = new Sessao(_database);
                _repositorio = new Repositorio(_sessao);

                return _repositorio.Query<Models.Usuario>(gerarSelectUsuarios(), new { idTipoUsuario }).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public List<TipoUsuario> RetornarTiposUsuario()
        {
            try
            {
                _sessao = new Sessao(_database);
                _repositorio = new Repositorio(_sessao);

                return _repositorio.Query<TipoUsuario>(gerarSelectTiposUsuario()).ToList();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string gerarSelectTiposUsuario() => @"
            SELECT * FROM DBO.Tipo_Pessoa
            ";

        private void InserirUsuarioBd(Models.Usuario usuario, ISessao sessao, IRepositorio repositorio)
        {
            usuario.Id = repositorio.Query<int>(gerarInsertUsuario(), new
            {
                usuario.Nome,
                usuario.CPF,
                usuario.Email,
                usuario.Telefone,
                usuario.Se_Ativo,
                DataCadastro = DateTime.Now
            }).First();

            usuario.Tipo.ForEach(x =>
                    repositorio.Execute(gerarInsertUsuarioTipo(), new
                    {
                        Id_Pessoa = usuario.Id,
                        Id_Tipo_Pessoa = x.Id
                    }));
        }

        public Models.Usuario RetornarUsuarioPorId(int idUsuario)
        {
            try
            {
                _sessao = new Sessao(_database);
                _repositorio = new Repositorio(_sessao);

                return _repositorio.Query<Models.Usuario>(gerarSelectUsuarioPorId(), new { idUsuario }).FirstOrDefault();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private string gerarSelectUsuarioPorId() => @"
             ;with UsuarioAtivo as (
             SELECT Id, max(data_Cadastro)DataCadastro FROM DBO.Pessoa
                  group by Id
              )
             SELECT * FROM dbo.Pessoa p
                 INNER JOIN UsuarioAtivo 
                      on UsuarioAtivo.Id = p.Id 
                      and UsuarioAtivo.DataCadastro = p.Data_Cadastro
                 INNER JOIN dbo.Pessoa_Tipo_Pessoa ptp 
                      on ptp.Id_Pessoa = p.Id and Id_Tipo_Pessoa = @idTipoUsuario
             WHERE p.Id = @idUsuario
              order by p.Id desc";

        private string gerarInsertUsuarioTipo() =>
            @"
                INSERT INTO [dbo].[Pessoa_Tipo_Pessoa]
                           ([Id]
                           ,[Id_Pessoa]
                           ,[Id_Tipo_Pessoa])
                     VALUES
                           ((SELECT isNull(max(id),0) + 1 FROM [dbo].[Pessoa_Tipo_Pessoa])
                           ,@Id_Pessoa
                           ,@Id_Tipo_Pessoa)";

        private string gerarInsertUsuario() =>
            @"
                INSERT INTO [dbo].[Pessoa]
                           ([Id]
                           ,[Nome]
                           ,[Telefone]
                           ,[Email]
                           ,[CPF]
                           ,[Se_Ativo]
                           ,[Data_Cadastro])
                     VALUES
                           ((SELECT isNull(max(id),0) + 1 FROM [dbo].[Pessoa])
                           ,@Nome
                           ,@Telefone
                           ,@Email
                           ,@CPF
                           ,@Se_Ativo
                           ,@Data_Cadastro)";

        public IResultado EditarUsuario(Models.Usuario usuario)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);

            try
            {
                _sessao.Begin();

                EditarUsuarioBd(usuario, _sessao, _repositorio);

                _sessao.Commit();

                return new Resultado($"O usuário {usuario.Nome}, foi editado com sucesso.", statusRetorno.OK);
            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                return new Resultado($"Ocorreram erros ao editar o usuário. Detalhes: {ex.Message}", statusRetorno.Erro);
            }
        }

        private void EditarUsuarioBd(Models.Usuario usuario, ISessao sessao, IRepositorio repositorio)
        {
            repositorio.Query<int>(gerarInsertEdicaoUsuario(), new
            {
                usuario.Id,
                usuario.Nome,
                usuario.CPF,
                usuario.Email,
                usuario.Telefone,
                DataCadastro = DateTime.Now
            }).First();

            removerTipoUsuario(usuario.Id, sessao, repositorio);

            usuario.Tipo.ForEach(x =>
                    repositorio.Execute(gerarInsertUsuarioTipo(), new
                    {
                        Id_Pessoa = usuario.Id,
                        Id_Tipo_Pessoa = x.Id
                    }));
        }

        private void removerTipoUsuario(int id, ISessao sessao, IRepositorio repositorio)
        {
            repositorio.Execute(gerarDeleteUsuarioTipo(), new
            {
                id
            });
        }

        private string gerarDeleteUsuarioTipo() => @"
                DELETE [DBO].[Pessoa_Tipo_Pessoa]
                WHERE @Id_Pessoa = @id
                ";

        private string gerarInsertEdicaoUsuario() =>
          @"
                INSERT INTO [dbo].[Pessoa]
                           ([Id]
                           ,[Nome]
                           ,[Telefone]
                           ,[Email]
                           ,[CPF]
                           ,[Se_Ativo]
                           ,[Data_Cadastro])
                     VALUES
                           (@Id)
                           ,@Nome
                           ,@Telefone
                           ,@Email
                           ,@CPF
                           ,@Se_Ativo
                           ,@Data_Cadastro)";

        private string gerarSelectUsuarios() => @"
             ;with UsuarioAtivo as (
             SELECT Id, max(data_Cadastro)DataCadastro FROM DBO.Pessoa
                  group by Id
              )
             SELECT * FROM dbo.Pessoa p
                 INNER JOIN UsuarioAtivo 
                      on UsuarioAtivo.Id = p.Id 
                      and UsuarioAtivo.DataCadastro = p.Data_Cadastro
                 INNER JOIN dbo.Pessoa_Tipo_Pessoa ptp 
                      on ptp.Id_Pessoa = p.Id and Id_Tipo_Pessoa = @idTipoUsuario
              order by p.Id desc";

    }
}
