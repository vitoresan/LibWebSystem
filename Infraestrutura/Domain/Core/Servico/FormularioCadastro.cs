using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.FormularioCadastro;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Infraestrutura.Domain.Core.Servico
{
    public class FormularioCadastro
    {
        IDatabase _database;
        ISessao _sessao;
        IRepositorio _repositorio;

        public FormularioCadastro(string connectionString)
        {
            _database = new Database(new SqlConnection(connectionString));
            _sessao = new Sessao(_database);
            _repositorio = new Repositorio(_sessao);
        }

        public List<Biografia_Tipo> RetornarBibliografiaTipo()
        {
            var query = $@"
                SELECT * FROM DBO.Bibliografia_Tipo ORDER BY 3";

            return _repositorio.Query<Biografia_Tipo>(query).ToList();
        }

        public List<Esquema_Codificacao> RetornarEsquemasCodificacao()
        {
            var query = $@"
                SELECT * FROM DBO.Esquema_Codificacao ORDER BY 3";

            return _repositorio.Query<Esquema_Codificacao>(query).ToList();
        }

        public List<Curso> RetornarCursos()
        {
            var query = $@"
                SELECT * FROM DBO.Cursos ORDER BY 2";

            return _repositorio.Query<Curso>(query).ToList();
        }

        public List<Forma_Catalogacao_Descritiva> RetornarFormasCatalogacaoDescritivas()
        {
            var query = $@"
                SELECT * FROM DBO.Forma_Catalogacao_Descritiva ORDER BY 3";

            return _repositorio.Query<Forma_Catalogacao_Descritiva>(query).ToList();
        }

        public List<Forma_Item> RetornarFormasItem()
        {
            var query = $@"
                SELECT * FROM DBO.Forma_Item ORDER BY 3";

            return _repositorio.Query<Forma_Item>(query).ToList();
        }

        public List<Forma_Literaria> RetornarFormasLiterarias()
        {
            var query = $@"
                SELECT * FROM DBO.Forma_Literaria_Tipo ORDER BY 3";

            return _repositorio.Query<Forma_Literaria>(query).ToList();
        }

        public List<Forma_Material> RetornarFormasMaterial()
        {
            var query = $@"
                SELECT * FROM DBO.Forma_Material ORDER BY 3";

            return _repositorio.Query<Forma_Material>(query).ToList();
        }

        public List<Ilustracao_Tipo> RetornarIlustracoesTipo()
        {
            var query = $@"
                SELECT * FROM DBO.Ilustracao_Tipo ORDER BY 3";

            return _repositorio.Query<Ilustracao_Tipo>(query).ToList();
        }

        public List<Natureza_Conteudo> RetornarNaturezasConteudo()
        {
            var query = $@"
                SELECT * FROM DBO.Natureza_Conteudo ORDER BY 3";

            return _repositorio.Query<Natureza_Conteudo>(query).ToList();
        }

        public List<Nivel_Bibliografico> RetornarNiveisBibliograficos()
        {
            var query = $@"
                SELECT * FROM DBO.Nivel_Bibliografico ORDER BY 3";

            return _repositorio.Query<Nivel_Bibliografico>(query).ToList();
        }

        public List<Nivel_Varias_Partes> RetornarNiveisVariasPartes()
        {
            var query = $@"
                SELECT * FROM DBO.Nivel_Varias_Partes ORDER BY 3";

            return _repositorio.Query<Nivel_Varias_Partes>(query).ToList();
        }

        public List<Nivel_Codificacao> RetornarNiveisCodificacao()
        {
            var query = $@"
                SELECT * FROM DBO.Nivel_Codificacao ORDER BY 3";

            return _repositorio.Query<Nivel_Codificacao>(query).ToList();
        }

        public List<Publicacao_Governamental_Tipo> RetornarPublicacaoGovernamentalTipo()
        {
            var query = $@"
                SELECT * FROM DBO.Publicacao_Governamental_Tipo ORDER BY 3";

            return _repositorio.Query<Publicacao_Governamental_Tipo>(query).ToList();
        }

        public List<Publico_Alvo> RetornarPublicoAlvo()
        {
            var query = $@"
                SELECT * FROM DBO.Publico_Alvo ORDER BY 3";

            return _repositorio.Query<Publico_Alvo>(query).ToList();
        }

        public List<Status_Registro> RetornarStatusRegistro()
        {
            var query = $@"
                SELECT * FROM DBO.Status_Registro ORDER BY 3";

            return _repositorio.Query<Status_Registro>(query).ToList();
        }

        public List<Tipo_Controle> RetornarTiposControle()
        {
            var query = $@"
                SELECT * FROM DBO.Tipo_Controle ORDER BY 3";

            return _repositorio.Query<Tipo_Controle>(query).ToList();
        }

        public List<Tipo_Registro> RetornarTiposRegistro()
        {
            var query = $@"
                SELECT * FROM DBO.Tipo_Registro ORDER BY 3";

            return _repositorio.Query<Tipo_Registro>(query).ToList();
        }
    }
}
