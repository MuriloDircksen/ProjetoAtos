using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [ApiController]
    [Route("api/receitangredientes")]
    public class ReceitaIngredientesController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public ReceitaIngredientesController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }

        [HttpPost]
        public async Task<IActionResult> CadastroReceitaIngrediente(
            [FromBody] ReceitaIngrediente receitaIngrediente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }

            try
            {
                await _contexto.ReceitaIngredientes.AddAsync(receitaIngrediente);
                await _contexto.SaveChangesAsync();
                return Created($"api/receitas/{receitaIngrediente.Id}", receitaIngrediente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut]
        [Route("api/receitaingredientes/{id}")]
        public async Task<IActionResult> AtualizaDadosReceitaIgrediente(
            [FromBody] ReceitaIngrediente receitaIngrediente,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var receitaIngredienteAtualizar = await _contexto.ReceitaIngredientes.FirstOrDefaultAsync(x => x.Id == id);
            if (receitaIngredienteAtualizar == null) return NotFound("Relação receita ingrediente não encontrada");

            try
            {
                receitaIngredienteAtualizar = receitaIngrediente;

                _contexto.ReceitaIngredientes.Update(receitaIngredienteAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(receitaIngredienteAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("api/receitangredientes/")]
        public async Task<IActionResult> ListagemReceitaIngrediente()
        {
            try
            {
                return Ok(await _contexto.ReceitaIngredientes.AsNoTracking().ToListAsync());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        
        [HttpDelete]
        [Route("/api/receitangredientes/{id")]
        public async Task<IActionResult> ExcluirReceitaIngrediente(
            [FromRoute] int id)
        {
            var receitaIngredienteDeletar = await _contexto.ReceitaIngredientes
                .Include(y=> y.Receita)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (receitaIngredienteDeletar == null) return NotFound("Receita não encontrada");
            
            if (receitaIngredienteDeletar.Receita != null)
            {
                return BadRequest("Não é permitido exclusão da relação receita ingrediente com receita associada");
            }

            try
            {
                _contexto.ReceitaIngredientes.Remove(receitaIngredienteDeletar);
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
