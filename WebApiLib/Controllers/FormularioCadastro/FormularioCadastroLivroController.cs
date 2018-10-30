using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models.FormularioCadastro;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.Enum;
using FormularioCadastroService = Infraestrutura.Domain.Core.Servico.FormularioCadastro;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApiLib.Controllers.FormularioCadastro
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("FormularioCadastroLivro")]
    public class FormularioCadastroLivroController : ApiController
    {
        FormularioCadastroService bdFormularioCadastroLivro = new FormularioCadastroService(BdConfig.Connection);

        [Route("RetornarCamposFormularioCadastroLivros")]
        [HttpGet]
        public IResultado RetornarLivros()
        {
            try
            {
                var camposFormularioCadastro = new CamposCadastroLivro();
                camposFormularioCadastro.Biografias_Tipo = bdFormularioCadastroLivro.RetornarBibliografiaTipo();
                camposFormularioCadastro.Cursos = bdFormularioCadastroLivro.RetornarCursos();
                camposFormularioCadastro.Esquemas_Codificacao = bdFormularioCadastroLivro.RetornarEsquemasCodificacao();
                camposFormularioCadastro.Formas_Catalogacao_Descritiva = bdFormularioCadastroLivro.RetornarFormasCatalogacaoDescritivas();
                camposFormularioCadastro.Formas_Item = bdFormularioCadastroLivro.RetornarFormasItem();
                camposFormularioCadastro.Formas_Literaria = bdFormularioCadastroLivro.RetornarFormasLiterarias();
                camposFormularioCadastro.Formas_Material = bdFormularioCadastroLivro.RetornarFormasMaterial();
                camposFormularioCadastro.Ilustracoes_Tipo = bdFormularioCadastroLivro.RetornarIlustracoesTipo();
                camposFormularioCadastro.Naturezas_Conteudo = bdFormularioCadastroLivro.RetornarNaturezasConteudo();
                camposFormularioCadastro.Niveis_Bibliograficos = bdFormularioCadastroLivro.RetornarNiveisBibliograficos();
                camposFormularioCadastro.Niveis_Codificacao = bdFormularioCadastroLivro.RetornarNiveisCodificacao();
                camposFormularioCadastro.Niveis_Varias_Partes = bdFormularioCadastroLivro.RetornarNiveisVariasPartes();
                camposFormularioCadastro.Publicacao_Governamental_Tipos = bdFormularioCadastroLivro.RetornarPublicacaoGovernamentalTipo();
                camposFormularioCadastro.Publicos_Alvos = bdFormularioCadastroLivro.RetornarPublicoAlvo();
                camposFormularioCadastro.Status_Registro = bdFormularioCadastroLivro.RetornarStatusRegistro();
                camposFormularioCadastro.Tipos_Controle = bdFormularioCadastroLivro.RetornarTiposControle();
                camposFormularioCadastro.Tipos_Registro = bdFormularioCadastroLivro.RetornarTiposRegistro();

                return new Resultado<CamposCadastroLivro>($"Operação realizada.", statusRetorno.OK, camposFormularioCadastro);
            }
            catch(System.Exception ex)
            {
                return new Resultado($"Ocorreram erros ao listar livros. Detalhes {ex.Message}", statusRetorno.Erro);
            }
        }
    }
}
