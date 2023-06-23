using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    public class UsuariosController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public UsuariosController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Salva um usuário no banco de dados 
        /// </summary>
        /// <param name="usuario">Objeto com os campos necessários para criação de um usuario</param>
        /// <returns>Dados do usuario cadastrado</returns>
        /// <response code="201">Caso a inserção de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>
        /// <response code="409">CNPJ ou email já existe no banco de dados</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CadastroUsuario(
            [FromBody] Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            if (usuario.Cnpj.Length < 14 || usuario.Cnpj.Length >14) return BadRequest("Deve possuir 14 caracteres para cnpj!");

            try
            {
                await _contexto.Usuarios.AddAsync(usuario);
                await _contexto.SaveChangesAsync();
                return Created($"api/usuarios/{usuario.Id}", usuario);
            }
            catch (Exception ex)
            {
                return Conflict("CNPJ ou email já existe no banco de dados: " + ex.ToString());
            }
        }

        /// <summary>
        /// Atualiza um usuário no banco de dados usando seu id
        /// </summary>
        /// <param name="usuario">Objeto com os campos necessários para atualização de um usuario</param>
        /// <param name="id">Id do usuario a ser atualizado no banco</param>
        /// <returns>Dados do usuario atualizado</returns>
        /// <response code="200">Caso a atualização de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpPut]
        [Route("/api/usuarios/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizaDadosUsuario(
            [FromBody] Usuario usuario,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var usuarioAtualizar = await _contexto.Usuarios.FirstOrDefaultAsync(x => x.Id == id);
            if (usuarioAtualizar == null) return NotFound("Usuario não encontrada");

            try
            {
                usuarioAtualizar.Nome = usuario.Nome;
                usuarioAtualizar.NomeEmpresa = usuario.NomeEmpresa;
                usuarioAtualizar.Email = usuario.Email;
                usuarioAtualizar.Cnpj = usuario.Cnpj;
                usuarioAtualizar.Senha = usuario.Senha;
                

                _contexto.Usuarios.Update(usuarioAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(usuarioAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma lista de usuários no banco de dados  
        /// </summary>        
        /// <returns>Dados dos usuarios</returns>
        /// <response code="200">Caso a lista de dados seja recuperada com sucesso</response>
        /// <response code="400">Requisição mal sucedida</response>
        [HttpGet]
        [Route("/api/usuarios/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]        
        public async Task<IActionResult> ListagemUsuario()
        {
            try
            {                
                return Ok(await _contexto.Usuarios.AsNoTracking().ToListAsync());           
                                
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma lista de usuários no banco de dados  
        /// </summary>        
        /// <param name="id">Id do usuario a ser recuperado no banco</param>
        /// <returns>Dados dos usuarios</returns>
        /// <response code="200">Caso o usuario seja recuperada com sucesso</response>
        /// <response code="404">Usuario não encontrado no banco de daos</response>
        [HttpGet]
        [Route("/api/usuarios/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarUsuarioPorId(
            [FromRoute] int id)
        {
            var usuario = await _contexto.Usuarios
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return usuario == null ? NotFound("Usuario não encontrado!") : Ok(usuario);
        }
    }
}
