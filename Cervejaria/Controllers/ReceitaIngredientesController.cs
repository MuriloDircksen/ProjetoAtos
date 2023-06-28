using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/receitaingredientes")]
    public class ReceitaIngredientesController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public ReceitaIngredientesController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }

        /// <summary>
        /// Salva uma relação receita ingrediente no banco de dados 
        /// </summary>
        /// <param name="receitaIngrediente">Objeto com os campos necessários para criação de uma receita ingrediente</param>
        /// <returns>Dados da relação receita ingrediente cadastrados</returns>
        /// <response code="201">Caso a inserção de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response> 
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Caso o id de receita e/ou ingrediente seja inexistente na base de dados</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CadastroReceitaIngrediente(
            [FromBody] ReceitaIngrediente receitaIngrediente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }

            var receita = await _contexto.Receitas.FirstOrDefaultAsync(x => x.Id == receitaIngrediente.IdReceita);
            var ingrediente = await _contexto.Ingredientes.FirstOrDefaultAsync(x => x.Id == receitaIngrediente.IdIngrediente);

            if (ingrediente == null || receita == null) return NotFound("Receita ou ingrediente não encontrados");

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
        /// <summary>
        /// Atualiza uma relação receita ingrediente no banco de dados usando seu id
        /// </summary>
        /// <param name="receitaIngrediente">Objeto com os campos necessários para atualização de uma relação receita ingrediente</param>
        /// <param name="id">Id da relação receita ingrediente a ser atualizado no banco</param>
        /// <returns>Dados da relação receita ingrediente atualizado</returns>
        /// <response code="200">Caso a atualização de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response> 
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Caso o id de receita e/ou ingrediente seja inexistente na base de dados</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            var receita = await _contexto.Receitas.FirstOrDefaultAsync(x => x.Id == receitaIngrediente.IdReceita);
            var ingrediente = await _contexto.Ingredientes.FirstOrDefaultAsync(x => x.Id == receitaIngrediente.IdIngrediente);
           
            if (ingrediente == null || receita == null) return NotFound("Receita ou ingrediente não encontrados");

            try
            {
                receitaIngredienteAtualizar.IdIngrediente = receitaIngrediente.IdIngrediente;
                receitaIngredienteAtualizar.IdReceita = receitaIngrediente.IdReceita;
                receitaIngredienteAtualizar.QuantidadeDeIngrediente = receitaIngrediente.QuantidadeDeIngrediente;

                _contexto.ReceitaIngredientes.Update(receitaIngredienteAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(receitaIngredienteAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma lista de relações receita ingrediente no banco de dados  
        /// </summary>        
        /// <returns>Dados das relações receita ingrediente </returns>
        /// <response code="200">Caso a lista de dados seja recuperada com sucesso</response>
        /// <response code="400">Requisição mal sucedida</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Deleta uma relação receita ingrediente no banco de dados conforme validaçoes 
        /// </summary>        
        /// <param name="id">Id da relação receita ingrediente a ser deletada no banco</param>
        /// <returns>Sem dados para retornar</returns>
        /// <response code="204">Caso a relação receita ingrediente seja deletada com sucesso</response>
        /// <response code="400">Proibida a exclusão de relação receita ingrediente com receita associada</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Receita não encontrado no banco de dados</response>
        [HttpDelete]
        [Route("/api/receitaingredientes/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        /// <summary>
        /// Deleta uma relação receita ingrediente no banco de dados quando atualiza receita
        /// </summary>        
        /// <param name="id">Id da relação receita ingrediente a ser deletada no banco</param>
        /// <returns>Sem dados para retornar</returns>
        /// <response code="204">Caso a relação receita ingrediente seja deletada com sucesso</response>
        /// <response code="400">Caso algum erro ocorra na inserção de dados</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Receita não encontrado no banco de dados</response>
        [HttpDelete]
        [Route("/api/receitaingredientes/atualizar/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirEspecialReceitaIngrediente(
            [FromRoute] int id)
        {
            var receitaIngredienteDeletar = await _contexto.ReceitaIngredientes
                .Include(y => y.Receita)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (receitaIngredienteDeletar == null) return NotFound("Receita não encontrada");

            
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
