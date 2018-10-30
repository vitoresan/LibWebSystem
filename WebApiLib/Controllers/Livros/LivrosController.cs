using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.Enum;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using LivroService = Infraestrutura.Domain.Core.Servico.Livros;

namespace WebApiLib.Controllers.Livros
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Livros")]
    public class LivrosController : ApiController
    {
        LivroService.Livros bdLivros = new LivroService.Livros(BdConfig.Connection);

        [Route("CadastrarLivro")]
        [HttpPost]
        public IResultado CadastrarLivro([FromBody]Livro livro)
        {
            return bdLivros.CadastrarLivro(livro);
        }

        [Route("RetornarLivros")]
        [HttpGet]
        public IResultado RetornarLivros()
        {
            try
            {
                return new Resultado<List<Livro>>($"Sucesso ao listar livros.", statusRetorno.OK, bdLivros.RetornarLivros());
            }
            catch(System.Exception ex)
            {
                return new Resultado($"Ocorreram erros ao listar livros. Detalhes {ex.Message}", statusRetorno.Erro);
            }
        }
    }
}
