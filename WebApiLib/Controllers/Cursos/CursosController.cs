using Infraestrutura.Domain.Interfaces;
using Infraestrutura.Domain.Models;
using CursoService = Infraestrutura.Domain.Core.Servico;
using Infraestrutura.Domain.Models.Enum;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApiLib.Controllers.Cursos
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Cursos")]
    public class CursosController : ApiController
    {
        CursoService.Cursos bdCursos = new CursoService.Cursos(BdConfig.Connection);

        [Route("RetornarCursos")]
        [HttpGet]
        public IResultado RetornarCursos()
        {
            try
            {
                return new Resultado<List<Curso>>($"Sucesso ao listar cursos relacionados.", statusRetorno.OK, bdCursos.RetornarCursos());
            }
            catch(System.Exception ex)
            {
                return new Resultado($"Ocorreram erros ao listar cursos relacionados. Detalhes {ex.Message}", statusRetorno.Erro);
            }
        }
    }
}
