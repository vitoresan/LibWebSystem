using Infraestrutura.Domain.Core.Servico;
using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using Infraestrutura.Domain.Models.Enum;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApiLib.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Emprestimo")]
    public class EmprestimoController : ApiController
    {
        ServicoEmprestimo servicoEmprestimo = new ServicoEmprestimo(BdConfig.Connection);

        [Route("RealizaEmprestimo")]
        [HttpPost]
        public IResultado RealizaEmprestimo([FromBody]DadosEmprestimo dadosEmprestimo)
        {
            return servicoEmprestimo.RealizaEmprestimo(dadosEmprestimo);
        }

        [Route("RealizaDevolucao")]
        [HttpPost]
        public IResultado RealizaDevolucao([FromBody]DadosEmprestimo dadosDevolucao)
        {
            return servicoEmprestimo.RealizaDevolucao(dadosDevolucao);
        }

        [Route("ConsultaDisponibilidadeLivro/{idLivro}")]
        [HttpPost]
        public IResultado ConsultaDisponibilidadeLivro(int idLivro)
        {
            try
            {
                return new Resultado<ControleLivro>("Operação realizada com sucesso", statusRetorno.OK, servicoEmprestimo.ConsultaDisponibilidadeLivro(idLivro));
            }
            catch(System.Exception ex)
            {

                return new Resultado($"Erros ao realizar consulta. {ex.Message}", statusRetorno.Erro);
            }
        }

    }
}
