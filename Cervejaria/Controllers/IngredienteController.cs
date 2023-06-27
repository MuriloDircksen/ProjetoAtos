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
        /// <summary>
        /// Salva um ingrediente no banco de dados 
        /// </summary>
        /// <param name="producao">Objeto com os campos necessários para criação de um ingrediente</param>
        /// <returns>Dados do ingrediente cadastrado</returns>
        /// <response code="201">Caso a inserção de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response> 
        /// <response code="404">Caso o id do estoque seja inexistente na base de dados</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CadastroIngrediente(
            [FromBody] Ingrediente ingrediente)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }

            var estoque = await _contexto.Estoques.FirstOrDefaultAsync(x => x.Id == ingrediente.IdEstoque);
            if (estoque == null) return NotFound("Estoque não encontrada");

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
        /// <summary>
        /// Atualiza um ingrediente no banco de dados usando seu id
        /// </summary>
        /// <param name="ingrediente">Objeto com os campos necessários para atualização de um ingrediente</param>
        /// <param name="id">Id do ingrediente a ser atualizado no banco</param>
        /// <returns>Dados do ingrediente atualizado</returns>
        /// <response code="200">Caso a atualização de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>
        /// <response code="404">Caso o id de ingrediente ou estoque seja inexistente na base de dados</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

            var estoque = await _contexto.Estoques.FirstOrDefaultAsync(x => x.Id == ingrediente.IdEstoque);
            if (estoque == null) return NotFound("Estoque não encontrada");

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
        /// <summary>
        /// Recupera uma lista de ingredientes no banco de dados  
        /// </summary>        
        /// <returns>Dados dos ingredientes</returns>
        /// <response code="200">Caso a lista de dados seja recuperada com sucesso</response>
        /// <response code="400">Requisição mal sucedida</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
        /// <summary>
        /// Recupera um ingrediente no banco de dados pelo seu id 
        /// </summary>        
        /// <param name="id">Id do ingrediente a ser recuperado no banco</param>
        /// <returns>Dados do ingrediente</returns>
        /// <response code="200">Caso o ingrediente seja recuperada com sucesso</response>        
        /// <response code="404">Ingrediente não encontrado no banco de dados</response
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarIngredientePorId(
            [FromRoute] int id)
        {
            var ingrediente = await _contexto.Ingredientes
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return ingrediente == null ? NotFound("Ingrediente não encontrado!") : Ok(ingrediente);
        }
        /// <summary>
        /// Deleta um ingrediente no banco de dados
        /// </summary>        
        /// <param name="id">Id do ingrediente a ser deletado no banco</param>
        /// <returns>Sem dados para retornar</returns>
        /// <response code="204">Caso o ingrediente seja deletado com sucesso</response>
        /// <response code="400">Erro no pedido de exclusão, não é permitido exclusão com relação ingrediente ativa</response>
        /// <response code="404">Ingrediente não encontrado no banco de dados</response>
        [HttpDelete]
        [Route("/api/ingredientes/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
