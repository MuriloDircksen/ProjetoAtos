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

        [HttpPost]
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
                return Conflict("CNPJ já existe no banco de dados: " + ex.ToString());
            }
        }
        [HttpPut]
        [Route("/api/usuarios/{id}")]
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
                usuarioAtualizar = usuario;

                _contexto.Usuarios.Update(usuarioAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(usuarioAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("/api/usuarios/")]
        public async Task<IActionResult> ListagemUsuario()
        {
            try
            {
                
                return Ok(await _contexto.Usuarios.AsNoTracking().ToListAsync());              

                
            }
            catch (Exception ex)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados: "+ex.ToString());
            }
        }
        [HttpGet]
        [Route("/api/usuarios/{id}")]
        public async Task<IActionResult> ListarUsuarioPorId(
            [FromRoute] int id)
        {
            var usuario = await _contexto.Usuarios
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return usuario == null ? NotFound("Usuario não encontrado!") : Ok(usuario);
        }
    }
}
