using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [ApiController]
    [Route("api/ingredientes")]
    public class IngredienteController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public IngredienteController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }
        [HttpPost]
        public async Task<IActionResult> CadastroIngrediente(
            [FromBody] Ingrediente ingrediente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }

            try
            {
                await _contexto.Ingredientes.AddAsync(ingrediente);
                await _contexto.SaveChangesAsync();
                return Created($"api/ingredientes/{ingrediente.Id}", ingrediente);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut]
        [Route("api/ingredientes/{id}")]
        public async Task<IActionResult> AtualizaDadosIngrediente(
            [FromBody] Ingrediente ingrediente,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var ingredienteAtualizar = await _contexto.Ingredientes.FirstOrDefaultAsync(x => x.Id == id);
            if (ingredienteAtualizar == null) return NotFound("Ingrediente não encontrada");

            try
            {
                ingredienteAtualizar.NomeIngrediente = ingrediente.NomeIngrediente;
                ingredienteAtualizar.IdEstoque = ingrediente.IdEstoque;
                ingredienteAtualizar.Quantidade = ingrediente.Quantidade;
                ingredienteAtualizar.Tipo = ingrediente.Tipo;   
                ingredienteAtualizar.ValorUnidade = ingrediente.ValorUnidade;
                ingredienteAtualizar.Unidade = ingrediente.Unidade;
                ingredienteAtualizar.ValorTotal = ingrediente.ValorTotal;
                ingredienteAtualizar.Fornecedor = ingrediente.Fornecedor;
                ingredienteAtualizar.Validade = ingrediente.Validade;
                ingredienteAtualizar.DataEntrada = ingrediente.DataEntrada;

                _contexto.Ingredientes.Update(ingredienteAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(ingredienteAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("api/ingredientes/")]
        public async Task<IActionResult> ListagemIngrediente()
        {
            try
            {
                return Ok(await _contexto.Ingredientes.AsNoTracking().ToListAsync());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpGet]
        [Route("api/ingredientes/{id}")]
        public async Task<IActionResult> ListarIngredientePorId(
            [FromRoute] int id)
        {
            var ingrediente = await _contexto.Ingredientes
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return ingrediente == null ? NotFound("Ingrediente não encontrado!") : Ok(ingrediente);
        }
        [HttpDelete]
        [Route("/api/ingredientes/{id}")]
        public async Task<IActionResult> ExcluirReceita(
            [FromRoute] int id)
        {
            var ingredienteDeletar = await _contexto.Ingredientes
                .Include(x => x.ReceitaIngredientes)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (ingredienteDeletar == null) return NotFound("Receita não encontrada");

            if (ingredienteDeletar.ReceitaIngredientes.FirstOrDefault() != null)
            {
                return BadRequest("Não é permitido exclusão de ingrediente com relação receita ingrediente associada");
            }
            try
            {
                _contexto.Ingredientes.Remove(ingredienteDeletar);
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
