using Service = Infraestrutura.Domain.Core.Servico;
using Infraestrutura.Domain.Interfaces;
using System.Web.Http;
using System.Web.Http.Cors;
using Infraestrutura.Domain.Models;
using System.Collections.Generic;
using Infraestrutura.Domain.Models.Enum;

namespace WebApiLib.Controllers.Pessoa
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("Usuario")]
    public class UsuarioController : ApiController
    {
        Service.Usuario bdUsuario = new Service.Usuario(BdConfig.Connection);

        [Route("CadastrarUsuario")]
        [HttpPost]
        public IResultado CadastrarUsuario([FromBody]Usuario usuario)
        {
            return bdUsuario.CadastrarUsuario(usuario);
        }

        [Route("EditarUsuario")]
        [HttpPost]
        public IResultado EditarUsuario([FromBody]Usuario usuario)
        {
            return bdUsuario.EditarUsuario(usuario);
        }

        [Route("RetornarUsuariosEmprestimo")]
        [HttpGet]
        public IResultado RetornarUsuariosEmprestimo()
        {
            try
            {
                int idTipoUsuarioEmprestimo = 2;
                return new Resultado<List<Usuario>>($"Sucesso ao listar usuários.", statusRetorno.OK, bdUsuario.RetornarUsuarios(idTipoUsuarioEmprestimo));
            }
            catch(System.Exception ex)
            {
                return new Resultado($"Ocorreram erros ao listar usuários. Detalhes {ex.Message}", statusRetorno.Erro);
            }
        }

        [Route("RetornarUsuariosSistema")]
        [HttpGet]
        public IResultado RetornarUsuariosSistema()
        {
            try
            {
                int idTipoUsuarioEmprestimo = 1;
                return new Resultado<List<Usuario>>($"Sucesso ao listar usuários.", statusRetorno.OK, bdUsuario.RetornarUsuarios(idTipoUsuarioEmprestimo));
            }
            catch(System.Exception ex)
            {
                return new Resultado($"Ocorreram erros ao listar usuários. Detalhes {ex.Message}", statusRetorno.Erro);
            }
        }

        [Route("RetornarUsuarioPorId/{IdUsuario}")]
        [HttpGet]
        public IResultado RetornarUsuarioPorId(int idUsuario)
        {
            try
            {
                return new Resultado<Usuario>($"Sucesso ao listar usuário.", statusRetorno.OK, bdUsuario.RetornarUsuarioPorId(idUsuario));
            }
            catch(System.Exception ex)
            {
                return new Resultado($"Ocorreram erros ao listar usuário. Detalhes {ex.Message}", statusRetorno.Erro);
            }
        }

    }
}
