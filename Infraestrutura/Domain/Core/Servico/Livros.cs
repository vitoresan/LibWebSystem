using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using System;
using System.Data.SqlClient;
using Infraestrutura.Domain.Models.Enum;
using System.Collections.Generic;
using System.Linq;
using Infraestrutura.Domain.Models.FormularioCadastro;

namespace Infraestrutura.Domain.Core.Servico.Livros
{

    public class Livros
    {
        IDatabase _database;
        ISessao _sessao;
        IRepositorio _repositorio;

        public Livros(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString));
        }

        public Resultado CadastrarLivro(Livro livro)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);

            try
            {
                _sessao.Begin();

                InserirLivroBd(livro, _sessao, _repositorio);

                _sessao.Commit();

                return new Resultado($"O livro {livro.Titulo}, foi cadastrado com sucesso.", statusRetorno.OK);
            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                return new Resultado($"Ocorreram erros ao cadastrar o livro. Detalhes: {ex.Message}", statusRetorno.Erro);
            }
        }

        public IResultado EditarLivro(Livro livro)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);

            try
            {
                _sessao.Begin();

                EditarLivroBd(livro, _sessao, _repositorio);

                _sessao.Commit();

                return new Resultado($"O livro {livro.Titulo}, foi editado com sucesso.", statusRetorno.OK);
            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                return new Resultado($"Ocorreram erros ao editar o livro. Detalhes: {ex.Message}", statusRetorno.Erro);
            }
        }

        private void EditarLivroBd(Livro livro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            _repositorio.Execute(gerarInsertEdicaoLivro(), new
            {
                livro.Id,
                livro.Titulo,
                livro.Autor,
                livro.Endereco,
                livro.Num_Paginas,
                livro.Ano_Publicacao,
                livro.Local_Publicacao,
                livro.CDD,
                livro.Qtd_Exemplares,
                DataCadastro = DateTime.Now
            });

            inserirCamposLivroBD(livro, _sessao, _repositorio);
        }

        private void InserirLivroBd(Livro livro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            livro.Id = _repositorio.Query<int>(gerarInsertLivro(), new
            {
                livro.Titulo,
                livro.Autor,
                livro.Endereco,
                livro.Num_Paginas,
                livro.Ano_Publicacao,
                livro.Local_Publicacao,
                livro.CDD,
                livro.Qtd_Exemplares,
                DataCadastro = DateTime.Now
            }).First();

            inserirControleDeExemplares(livro.Id, livro.Qtd_Exemplares, _sessao, _repositorio);

            inserirCamposLivroBD(livro, _sessao, _repositorio);

        }

        private void inserirControleDeExemplares(int id, int qtd_Exemplares, ISessao sessao, IRepositorio repositorio)
        {
            string scriptInsertControleExemplar = gerarScriptInsertControleExemplar();
            for(int i = 0; i < qtd_Exemplares; i++)
                _repositorio.Execute(scriptInsertControleExemplar, new { idLivro = id });
        }

        private string gerarScriptInsertControleExemplar()
        {
            return @"
                INSERT INTO [dbo].[Controle_Exemplares]
                           ([Id]
                           ,[Id_Livro]
                           ,[Se_Emprestado])
                     VALUES
                           ((select isNull(max(id) + 1, 1) from [dbo].[Controle_Exemplares])
                           ,@idLivro
                           ,0)";
        }

        private void inserirCamposLivroBD(Livro livro, ISessao sessao, IRepositorio repositorio)
        {
            CadastrarCursosRelacionados(livro, _sessao, _repositorio);

            CadastrarBiografiaTipo(livro, _sessao, _repositorio);

            CadastrarIlustracaoTipo(livro, _sessao, _repositorio);

            CadastrarPublicoAlvo(livro, _sessao, _repositorio);
        }

        public Livro RetornarLivroPorID(int id)
        {
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);

            var livro = _repositorio.Query<Livro>(gerarSelectLivroPorID(), new
            {
                id
            }).FirstOrDefault();

            livro.CamposLivro = retornarCamposLivro(livro.Id);

            return livro;
        }

        private string gerarSelectLivroPorID()
        {
            return @"
                ;with LivroAtivo as(
                SELECT Id, max(data__Cadastro) DataCadastro FROM DBO.LIVROS 
                group by Id
                )
                SELECT * FROM dbo.Livros l
                	inner join LivroAtivo on LivroAtivo.Id = l.Id and LivroAtivo.DataCadastro = l.Data__Cadastro
                Where l.id = @id
";
        }

        private void InserirLivroTodosCamposBd(Livro livro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            var idLivro = _repositorio.Query<int>(gerarInsertLivro(), new
            {
                livro.Titulo,
                livro.Autor,
                livro.Endereco,
                livro.Num_Paginas,
                DataCadastro = DateTime.Now
            }).First();

            CadastrarCursosRelacionados(livro, _sessao, _repositorio);

            CadastrarBiografiaTipo(livro, _sessao, _repositorio);

            CadastrarEsquemaCodificacao(idLivro, livro.CamposLivro.Esquemas_Codificacao, _sessao, _repositorio);

            CadastrarFormaCatalogacaoDescritiva(idLivro, livro.CamposLivro.Formas_Catalogacao_Descritiva, _sessao, _repositorio);

            CadastrarFormaItem(idLivro, livro.CamposLivro.Formas_Item, _sessao, _repositorio);

            CadastrarFormaLiteraria(idLivro, livro.CamposLivro.Formas_Literaria, _sessao, _repositorio);

            CadastrarMaterial(idLivro, livro.CamposLivro.Formas_Material, _sessao, _repositorio);

            CadastrarIlustracaoTipo(livro, _sessao, _repositorio);

            CadastrarNaturezaConteudo(idLivro, livro.CamposLivro.Naturezas_Conteudo, _sessao, _repositorio);

            CadastrarNivelBibliografico(idLivro, livro.CamposLivro.Niveis_Bibliograficos, _sessao, _repositorio);

            CadastrarNivelCodificacao(idLivro, livro.CamposLivro.Niveis_Codificacao, _sessao, _repositorio);

            CadastrarNivelVariasPartes(idLivro, livro.CamposLivro.Niveis_Varias_Partes, _sessao, _repositorio);

            CadastrarPublicacaoGovernamental(idLivro, livro.CamposLivro.Publicacao_Governamental_Tipos, _sessao, _repositorio);

            CadastrarPublicoAlvo(livro, _sessao, _repositorio);

            CadastrarStatusRegistro(idLivro, livro.CamposLivro.Status_Registro, _sessao, _repositorio);

            CadastrarTipoControle(idLivro, livro.CamposLivro.Tipos_Controle, _sessao, _repositorio);

            CadastrarTipoRegistro(idLivro, livro.CamposLivro.Tipos_Registro, _sessao, _repositorio);
        }

        public void CadastrarCursosRelacionados(Livro livro, ISessao _sessao = null, IRepositorio _repositorio = null)
        {
            if(livro.CamposLivro.Cursos.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int cursosInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                livro.CamposLivro.Cursos.ForEach(curso =>
                {
                    _repositorio.Execute(gerarInsertCursosRelacionados(), new { idLivro = livro.Id, idCurso = curso.Id });
                    cursosInseridos++;
                });

                if(cursosInseridos != livro.CamposLivro.Cursos.Count())
                {
                    throw new Exception("Erro ao cadastrar cursos relacionados.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public List<Livro> RetornarLivros()
        {

            var queryLivros = $@"
                    ;with LivroAtivo as (
                         SELECT Id, max(data__Cadastro)DataCadastro FROM DBO.LIVROS
                        group by Id
                    )
                   SELECT * FROM dbo.Livros l
                       INNER JOIN LivroAtivo 
                            on LivroAtivo.Id = l.Id 
                            and LivroAtivo.DataCadastro = l.Data__Cadastro order by l.Id desc";

            try
            {
                _sessao = new Sessao(_database);
                _repositorio = new Repositorio(_sessao);


                var livros = _repositorio.Query<Livro>(queryLivros).ToList();

                livros.ForEach(x =>
                {
                    x.CamposLivro = retornarCamposLivro(x.Id);
                });

                return livros;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private CamposCadastroLivro retornarCamposLivro(int idLivro)
        {
            var camposLivro = new CamposCadastroLivro();

            camposLivro.Cursos = _repositorio.Query<Curso>(gerarConsultaCursosRelacionadosPorID(), new { idLivro }).ToList();
            camposLivro.Ilustracoes_Tipo = _repositorio.Query<Ilustracao_Tipo>(gerarConsultaIlustracoesTipoPorID(), new { idLivro }).ToList();
            camposLivro.Publicos_Alvos = _repositorio.Query<Publico_Alvo>(gerarConsultaPublicosAlvosPorID(), new { idLivro }).ToList();
            camposLivro.Biografias_Tipo = _repositorio.Query<Biografia_Tipo>(gerarConsultaBiografiaTipoPorID(), new { idLivro }).ToList();

            return camposLivro;
        }

        private void CadastrarTipoRegistro(int idLivro, List<Tipo_Registro> tiposRegistro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(tiposRegistro.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int tiposRegistroInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                tiposRegistro.ForEach(tipoRegistro =>
                {
                    _repositorio.Execute(gerarInsertTipoRegistroLivro(), new { idLivro, idTipoRegistro = tipoRegistro.Id_Tipo_Registro });
                    tiposRegistroInseridos++;
                });

                if(tiposRegistroInseridos != tiposRegistro.Count())
                {
                    throw new Exception("Erro ao cadastrar tipo de registro.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarTipoControle(int idLivro, List<Tipo_Controle> tiposControle, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(tiposControle.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int tiposControleInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                tiposControle.ForEach(tipoControle =>
                {
                    _repositorio.Execute(gerarInsertTipoControleLivro(), new { idLivro, idCurso = tipoControle.Id_Tipo_Controle });
                    tiposControleInseridos++;
                });

                if(tiposControleInseridos != tiposControle.Count())
                {
                    throw new Exception("Erro ao cadastrar tipo de controle.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarStatusRegistro(int idLivro, List<Status_Registro> statusRegistro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(statusRegistro.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int statusRegistroInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                statusRegistro.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertStatusRegistroLivro(), new { idLivro, idCurso = x.Id_Status_Registro });
                    statusRegistroInseridos++;
                });

                if(statusRegistroInseridos != statusRegistro.Count())
                {
                    throw new Exception("Erro ao cadastrar status do registro.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarPublicoAlvo(Livro livro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(livro.CamposLivro.Publicos_Alvos.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int publicosAlvoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                livro.CamposLivro.Publicos_Alvos.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertPublicoAlvoLivro(), new { idLivro = livro.Id, idPublicoAlvo = x.Id_Publico_Alvo });
                    publicosAlvoInseridos++;
                });

                if(publicosAlvoInseridos != livro.CamposLivro.Publicos_Alvos.Count())
                {
                    throw new Exception("Erro ao cadastrar publico alvo.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarPublicacaoGovernamental(int idLivro, List<Publicacao_Governamental_Tipo> publicacaoGovernamentalTipos, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(publicacaoGovernamentalTipos.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int publicacaoGovernamentalTiposInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                publicacaoGovernamentalTipos.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertPublicacaoGovernamentalTipoLivro(), new { idLivro, idCurso = x.Id_Publicacao_Governamental_Tipo });
                    publicacaoGovernamentalTiposInseridos++;
                });

                if(publicacaoGovernamentalTiposInseridos != publicacaoGovernamentalTipos.Count())
                {
                    throw new Exception("Erro ao cadastrar tipo de publicação governamental.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarNivelVariasPartes(int idLivro, List<Nivel_Varias_Partes> niveisVariasPartes, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(niveisVariasPartes.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int niveisVariasPartesInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                niveisVariasPartes.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertNivelVariasPartesLivro(), new { idLivro, idCurso = x.Id_Nivel_Varias_Partes });
                    niveisVariasPartesInseridos++;
                });

                if(niveisVariasPartesInseridos != niveisVariasPartes.Count())
                {
                    throw new Exception("Erro ao cadastrar nivel de várias partes.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }

        }

        private void CadastrarNivelCodificacao(int idLivro, List<Nivel_Codificacao> niveisCodificacao, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(niveisCodificacao.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int niveisCodificacaoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                niveisCodificacao.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertNivelCodificacaoLivro(), new { idLivro, idCurso = x.Id_Nivel_Codificacao });
                    niveisCodificacaoInseridos++;
                });

                if(niveisCodificacaoInseridos != niveisCodificacao.Count())
                {
                    throw new Exception("Erro ao cadastrar nivel de codificação.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarNivelBibliografico(int idLivro, List<Nivel_Bibliografico> niveisBibliograficos, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(niveisBibliograficos.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int niveisBibliograficosInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                niveisBibliograficos.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertNivelBibliograficoLivro(), new { idLivro, idCurso = x.Id_Nivel_Bibliografico });
                    niveisBibliograficosInseridos++;
                });

                if(niveisBibliograficosInseridos != niveisBibliograficos.Count())
                {
                    throw new Exception("Erro ao cadastrar nivel bibliográfico.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarNaturezaConteudo(int idLivro, List<Natureza_Conteudo> naturezasConteudo, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(naturezasConteudo.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int naturezasConteudoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                naturezasConteudo.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertNaturezaConteudoLivro(), new { idLivro, idCurso = x.Id_Natureza_Conteudo });
                    naturezasConteudoInseridos++;
                });

                if(naturezasConteudoInseridos != naturezasConteudo.Count())
                {
                    throw new Exception("Erro ao cadastrar natureza conteúdo.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarIlustracaoTipo(Livro livro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(livro.CamposLivro.Ilustracoes_Tipo.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int ilustracoesTipoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                livro.CamposLivro.Ilustracoes_Tipo.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertIlustracaoTipoLivro(), new { idLivro = livro.Id, idIlustracaoTipo = x.Id_Ilustracao_Tipo });
                    ilustracoesTipoInseridos++;
                });

                if(ilustracoesTipoInseridos != livro.CamposLivro.Ilustracoes_Tipo.Count())
                {
                    throw new Exception("Erro ao cadastrar tipos de ilustração.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarMaterial(int idLivro, List<Forma_Material> formasMaterial, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(formasMaterial.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int formasMaterialInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                formasMaterial.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertFormaMaterialLivro(), new { idLivro, idCurso = x.Id_Forma_Material });
                    formasMaterialInseridos++;
                });

                if(formasMaterialInseridos != formasMaterial.Count())
                {
                    throw new Exception("Erro ao cadastrar formas do material.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarFormaLiteraria(int idLivro, List<Forma_Literaria> formasLiterariaTipo, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(formasLiterariaTipo.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int formasLiterariaTipoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                formasLiterariaTipo.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertFormaLiterariaTipoLivro(), new { idLivro, idCurso = x.Id_Forma_Literaria });
                    formasLiterariaTipoInseridos++;
                });

                if(formasLiterariaTipoInseridos != formasLiterariaTipo.Count())
                {
                    throw new Exception("Erro ao cadastrar tipo de forma literária.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarFormaItem(int idLivro, List<Forma_Item> formasItem, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(formasItem.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int formasItemInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                formasItem.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertFormaItemLivro(), new { idLivro, idCurso = x.Id_Forma_Item });
                    formasItemInseridos++;
                });

                if(formasItemInseridos != formasItem.Count())
                {
                    throw new Exception("Erro ao cadastrar forma do item.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarFormaCatalogacaoDescritiva(int idLivro, List<Forma_Catalogacao_Descritiva> formasCatalogacaoDescritiva, ISessao sessao, IRepositorio repositorio)
        {
            if(formasCatalogacaoDescritiva.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int formasCatalogacaoDescritivaInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                formasCatalogacaoDescritiva.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertFormaCatalogacaoDescritivaLivro(), new { idLivro, idCurso = x.Id_Forma_Catalogacao_Descritiva });
                    formasCatalogacaoDescritivaInseridos++;
                });

                if(formasCatalogacaoDescritivaInseridos != formasCatalogacaoDescritiva.Count())
                {
                    throw new Exception("Erro ao cadastrar forma de catalogação descritiva.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarEsquemaCodificacao(int idLivro, List<Esquema_Codificacao> esquemasCodificacao, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(esquemasCodificacao.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int esquemasCodificacaoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                esquemasCodificacao.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertEsquemaCodificacaoLivro(), new { idLivro, idCurso = x.Id_Esquema_Codificacao });
                    esquemasCodificacaoInseridos++;
                });

                if(esquemasCodificacaoInseridos != esquemasCodificacao.Count())
                {
                    throw new Exception("Erro ao cadastrar esquema de codificação.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private void CadastrarBiografiaTipo(Livro livro, ISessao sessao = null, IRepositorio repositorio = null)
        {
            if(livro.CamposLivro.Biografias_Tipo.Count() == 0)
            {
                return;
            }

            bool novaSessao = false;
            int biografiasTipoInseridos = 0;

            try
            {

                if(_sessao == null)
                {
                    novaSessao = true;
                    _sessao = new Sessao(_database);
                    _repositorio = new Repositorio(_sessao);
                    _sessao.Begin();
                }

                livro.CamposLivro.Biografias_Tipo.ForEach(x =>
                {
                    _repositorio.Execute(gerarInsertBiografiaTipoLivro(), new { idLivro = livro.Id, idBiografiaTipo = x.Id_Bibliografia_Tipo });
                    biografiasTipoInseridos++;
                });

                if(biografiasTipoInseridos != livro.CamposLivro.Biografias_Tipo.Count())
                {
                    throw new Exception("Erro ao cadastrar tipo de biografia.");
                }

                if(novaSessao)
                    _sessao.Commit();

            }
            catch(Exception ex)
            {
                _sessao.Rollback();
                throw new Exception(ex.Message);
            }
        }

        private string gerarInsertCursosRelacionados()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Cursos_Relacionados_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Curso] = @idCurso)
                 BEGIN
                    INSERT INTO [dbo].[Cursos_Relacionados_Livro]
                          ([Id_Livro]
                          ,[Id_Curso])
                    VALUES
                          (@idLivro
                          ,@idCurso)
                 END";
        }

        private string gerarInsertBiografiaTipoLivro()
        {
            return $@"
                IF NOT EXISTS(
                    SELECT * FROM [dbo].[Biografia_Tipo_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Biografia_Tipo] = @idBiografiaTipo)
                 BEGIN
                    INSERT INTO [dbo].[Biografia_Tipo_Livro]
                          ([Id_Livro]
                          ,[Id_Biografia_Tipo])
                    VALUES
                          (@idLivro
                          ,@idBiografiaTipo)
                 END";
        }

        private string gerarInsertEsquemaCodificacaoLivro()
        {
            return $@"
                IF NOT EXISTS(
                    SELECT * FROM [dbo].[Esquema_Codificacao_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Esquema_Codificacao] = @idEsquemaCodificacao)
 
                 BEGIN
                    INSERT INTO [dbo].[Esquema_Codificacao_Livro]
                       ([Id_Livro]
                       ,[Id_Esquema_Codificacao])
                    VALUES
                       (@idLivro
                       ,@idEsquemaCodificacao)
                 END";
        }

        private string gerarInsertFormaCatalogacaoDescritivaLivro()
        {
            return $@"
                IF NOT EXISTS(
                    SELECT * FROM [dbo].[Forma_Catalogacao_Descritiva_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Forma_Catalogacao_Descritiva] = @idFormaCatalogacaoDescritiva)

                BEGIN
                     INSERT INTO [dbo].[Forma_Catalogacao_Descritiva_Livro]
                       ([Id_Livro]
                       ,[Id_Forma_Catalogacao_Descritiva])
                    VALUES
                       (@idLivro
                       ,@idFormaCatalogacaoDescritiva)
                END";
        }

        private string gerarInsertFormaItemLivro()
        {
            return $@"
                IF NOT EXISTS(
                    SELECT * FROM [dbo].[Forma_Item_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Forma_Item] = @idFormaItem)

                BEGIN
                    INSERT INTO [dbo].[Forma_Item_Livro]
                       ([Id_Livro]
                       ,[Id_Forma_Item])
                    VALUES
                       (@idLivro
                       ,@idFormaItem)
                END";
        }

        private string gerarInsertFormaLiterariaTipoLivro()
        {
            return $@"
                IF NOT EXISTS(
                    SELECT * FROM [dbo].[Forma_Literaria_Tipo_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Forma_Literaria_Tipo] = @idFormaLiterariaTipo)

                BEGIN
                   INSERT INTO [dbo].[Forma_Literaria_Tipo_Livro]
                       ([Id_Livro]
                       ,[Id_Forma_Literaria_Tipo])
                   VALUES
                       (@idLivro
                       ,@idFormaLiterariaTipo)
                END";
        }

        private string gerarInsertFormaMaterialLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Forma_Material_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Forma_Material] = @idFormaMaterial)

                BEGIN
                   INSERT INTO [dbo].[Forma_Material_Livro]
                       ([Id_Livro]
                       ,[Id_Forma_Material])
                   VALUES
                       (@idLivro
                       ,@idFormaMaterial)
                END";
        }

        private string gerarInsertIlustracaoTipoLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Ilustracao_Tipo_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Ilustracao_Tipo] = @idIlustracaoTipo)

                BEGIN
                   INSERT INTO [dbo].[Ilustracao_Tipo_Livro]
                       ([Id_Livro]
                       ,[Id_Ilustracao_Tipo])
                   VALUES
                       (@idLivro
                       ,@idIlustracaoTipo)
                END";
        }

        private string gerarInsertNaturezaConteudoLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Natureza_Conteudo_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Natureza_Conteudo] = @idNaturezaConteudo)

                BEGIN
                   INSERT INTO [dbo].[Natureza_Conteudo_Livro]
                       ([Id_Livro]
                       ,[Id_Natureza_Conteudo])
                   VALUES
                       (@idLivro
                       ,@idNaturezaConteudo)
                END";
        }

        private string gerarInsertNivelBibliograficoLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Nivel_Bibliografico_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Nivel_Bibliografico] = @idNivelBibliografico)

                BEGIN
                   INSERT INTO [dbo].[Nivel_Bibliografico_Livro]
                       ([Id_Livro]
                       ,[Id_Nivel_Bibliografico])
                   VALUES
                       (@idLivro
                       ,@idNivelBibliografico)
                END";

        }

        private string gerarInsertNivelCodificacaoLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Nivel_Codificacao_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Nivel_Codificacao] = @idNivelCodificacao)

                BEGIN
                   INSERT INTO [dbo].[Nivel_Codificacao_Livro]
                       ([Id_Livro]
                       ,[Id_Nivel_Codificacao])
                   VALUES
                       (@idLivro
                       ,@idNivelCodificacao)
                END";
        }

        private string gerarInsertNivelVariasPartesLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Nivel_Varias_Partes_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Nivel_Varias_Partes] = @idNivelVariasPartes)

                BEGIN
                   INSERT INTO [dbo].[Nivel_Varias_Partes_Livro]
                       ([Id_Livro]
                       ,[Id_Nivel_Varias_Partes])
                   VALUES
                       (@idLivro
                       ,@idNivelVariasPartes)
                END";
        }

        private string gerarInsertPublicacaoGovernamentalTipoLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Publicacao_Governamental_Tipo_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Publicacao_Governamental_Tipo] = @idPublicacaoGovernamentalTipo)

                BEGIN
                   INSERT INTO [dbo].[Publicacao_Governamental_Tipo_Livro]
                       ([Id_Livro]
                       ,[Id_Publicacao_Governamental_Tipo])
                   VALUES
                       (@idLivro
                       ,@idPublicacaoGovernamentalTipo)
                END";
        }

        private string gerarInsertPublicoAlvoLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Publico_Alvo_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Publico_Alvo] = @idPublicoAlvo)

                BEGIN
                   INSERT INTO [dbo].[Publico_Alvo_Livro]
                       ([Id_Livro]
                       ,[Id_Publico_Alvo])
                   VALUES
                       (@idLivro
                       ,@idPublicoAlvo)
                END";
        }

        private string gerarInsertStatusRegistroLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Status_Registro_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Status_Registro] = @idStatusRegistro)

                BEGIN
                   INSERT INTO [dbo].[Status_Registro_Livro]
                       ([Id_Livro]
                       ,[Id_Status_Registro])
                   VALUES
                       (@idLivro
                       ,@idStatusRegistro)
                END";
        }

        private string gerarInsertTipoControleLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Tipo_Controle_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Tipo_Controle] = @idTipoControle)

                BEGIN
                   INSERT INTO [dbo].[Tipo_Controle_Livro]
                       ([Id_Livro]
                       ,[Id_Tipo_Controle])
                   VALUES
                       (@idLivro
                       ,@idTipoControle)
                END";
        }

        private string gerarInsertTipoRegistroLivro()
        {
            return $@"
                 IF NOT EXISTS(
                    SELECT * FROM [dbo].[Tipo_Registro_Livro]
                    WHERE [Id_Livro]  = @idLivro
                          and [Id_Tipo_Registro] = @idTipoRegistro)

                BEGIN
                   INSERT INTO [dbo].[Tipo_Registro_Livro]
                       ([Id_Livro]
                       ,[Id_Tipo_Registro])
                   VALUES
                       (@idLivro
                       ,@idTipoRegistro)
                END";
        }

        private string gerarInsertEdicaoLivro()
        {
            return $@"
                 INSERT INTO [dbo].[Livros]
                       ([Id]
                       ,[Titulo]
                       ,[Autor]
                       ,[Num_Paginas]
                       ,[Endereco]
                       ,[Ano_Publicacao]
                       ,[Local_Publicacao]
                       ,[CDD]
                       ,[Qtd_Exemplares]
                       ,[Data__Cadastro])
                 VALUES
                       (@Id
                       ,@Titulo
                       ,@Autor
                       ,@Num_Paginas
                       ,@Endereco
                       ,@Ano_Publicacao
                       ,@Local_Publicacao
                       ,@CDD
                       ,@Qtd_Exemplares
                       ,@DataCadastro)";
        }

        private string gerarInsertLivro()
        {
            return $@"
                 declare @idLivro int = (SELECT isNull(Max(Id),0) + 1 FROM [dbo].[Livros])
                 
                 INSERT INTO [dbo].[Livros]
                       ([Id]
                       ,[Titulo]
                       ,[Autor]
                       ,[Num_Paginas]
                       ,[Endereco]
                       ,[Ano_Publicacao]
                       ,[Local_Publicacao]
                       ,[CDD]
                       ,[Qtd_Exemplares]
                       ,[Data__Cadastro])
                 VALUES
                       (@idLivro
                       ,@Titulo
                       ,@Autor
                       ,@Num_Paginas
                       ,@Endereco
                       ,@Ano_Publicacao
                       ,@Local_Publicacao
                       ,@CDD
                       ,@Qtd_Exemplares
                       ,@DataCadastro)

                SELECT @idLivro";
        }

        private string gerarConsultaCursosRelacionadosPorID()
        {
            return $@"
                SELECT C.* FROM DBO.CURSOS_RELACIONADOS_LIVRO CRL
                    INNER JOIN DBO.CURSOS C ON C.ID = CRL.ID_CURSO
                WHERE ID_LIVRO = @idLivro";
        }

        private string gerarConsultaIlustracoesTipoPorID()
        {
            return $@"
               SELECT IT.* FROM DBO.ILUSTRACAO_TIPO_LIVRO ITL
                  INNER JOIN DBO.ILUSTRACAO_TIPO IT ON IT.ID_ILUSTRACAO_TIPO = ITL.ID_ILUSTRACAO_TIPO
               WHERE ID_LIVRO = @idLivro";
        }

        private string gerarConsultaPublicosAlvosPorID()
        {
            return $@"
               SELECT PA.* FROM DBO.PUBLICO_ALVO_LIVRO PAL
                  INNER JOIN DBO.PUBLICO_ALVO PA ON PA.ID_PUBLICO_ALVO = PAL.ID_PUBLICO_ALVO
               WHERE ID_LIVRO = @idLivro";
        }

        private string gerarConsultaBiografiaTipoPorID()
        {
            return $@"
               SELECT BT.* FROM DBO.BIOGRAFIA_TIPO_LIVRO BTL
                  INNER JOIN DBO.BIBLIOGRAFIA_TIPO BT ON BT.ID_BIBLIOGRAFIA_TIPO = BTL.ID_BIOGRAFIA_TIPO
               WHERE ID_LIVRO = @idLivro";
        }


    }
}
