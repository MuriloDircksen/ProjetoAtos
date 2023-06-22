using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [ApiController]
    [Route("api/estoques")]
    public class EstoqueController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public EstoqueController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }
        [HttpPost]
        public async Task<IActionResult> CadastroEstoque(
            [FromBody] Estoque estoque)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }

            try
            {
                await _contexto.Estoques.AddAsync(estoque);
                await _contexto.SaveChangesAsync();
                return Created($"api/estoques/{estoque.Id}", estoque);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> AtualizaDadosEstoques(
            [FromBody] Estoque estoque,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var estoqueAtualizar = await _contexto.Estoques.FirstOrDefaultAsync(x => x.Id == id);
            if (estoqueAtualizar == null) return NotFound("Ingrediente não encontrada");

            try
            {
                estoqueAtualizar.NomeEstoque = estoque.NomeEstoque;

                _contexto.Estoques.Update(estoqueAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(estoqueAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        
        public async Task<IActionResult> ListagemEstoques()
        {
            try
            {
                return Ok(await _contexto.Estoques.AsNoTracking().ToListAsync());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ListarEstoquePorId(
            [FromRoute] int id)
        {
            var estoque = await _contexto.Estoques
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return estoque == null ? NotFound("Estoque não encontrado!") : Ok(estoque);
        }
        [HttpDelete]
        [Route("/api/estoques/{id}")]
        public async Task<IActionResult> ExcluirEstoque(
            [FromRoute] int id)
        {
            var estoqueDeletar = await _contexto.Estoques
                .Include(x => x.Ingredientes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (estoqueDeletar == null) return NotFound("Receita não encontrada");

            if (estoqueDeletar.Ingredientes.FirstOrDefault() != null)
            {
                return BadRequest("Não é permitido exclusão de estoque com ingrediente associada");
            }
            try
            {
                _contexto.Estoques.Remove(estoqueDeletar);
                await _contexto.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }

    }
}
