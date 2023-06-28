using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/estoques")]

    public class EstoqueController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public EstoqueController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Salva um estoque no banco de dados 
        /// </summary>
        /// <param name="estoque">Objeto com os campos necessários para criação de um estoque</param>
        /// <returns>Dados do estoque cadastrado</returns>
        /// <response code="201">Caso a inserção de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response> 
        /// <response code="401">Acesso não autorizado, token inválido</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Atualiza um estoque no banco de dados usando seu id
        /// </summary>
        /// <param name="estoque">Objeto com os campos necessários para atualização de um estoque</param>
        /// <param name="id">Id do estoque a ser atualizado no banco</param>
        /// <returns>Dados do estoque atualizado</returns>
        /// <response code="200">Caso a atualização de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Caso o id do estoque seja inexistente na base de dados</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizaDadosEstoques(
            [FromBody] Estoque estoque,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var estoqueAtualizar = await _contexto.Estoques.FirstOrDefaultAsync(x => x.Id == id);
            if (estoqueAtualizar == null) return NotFound("Estoque não encontrada");

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
        /// <summary>
        /// Recupera uma lista de estoques no banco de dados  
        /// </summary>        
        /// <returns>Dados dos estoques</returns>
        /// <response code="200">Caso a lista de dados seja recuperada com sucesso</response>
        /// <response code="400">Requisição mal sucedida</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        /// <summary>
        /// Recupera um estoque no banco de dados pelo seu id 
        /// </summary>        
        /// <param name="id">Id do estoque a ser recuperado no banco</param>
        /// <returns>Dados do ingrediente</returns>
        /// <response code="200">Caso o estoque seja recuperada com sucesso</response>  
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Estoque não encontrado no banco de dados</response
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarEstoquePorId(
            [FromRoute] int id)
        {
            var estoque = await _contexto.Estoques
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return estoque == null ? NotFound("Estoque não encontrado!") : Ok(estoque);
        }
        /// <summary>
        /// Deleta um estoque no banco de dados
        /// </summary>        
        /// <param name="id">Id do estoque a ser deletado no banco</param>
        /// <returns>Sem dados para retornar</returns>
        /// <response code="204">Caso o estoque seja deletado com sucesso</response>
        /// <response code="400">Erro no pedido de exclusão, não é permitido exclusão com ingrediente associado</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Estoque não encontrado no banco de dados</response>
        [HttpDelete]
        [Route("/api/estoques/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
