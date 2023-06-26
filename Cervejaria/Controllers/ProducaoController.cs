using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [ApiController]
    [Route("api/producoes")]
    public class ProducaoController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public ProducaoController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Salva uma produção no banco de dados 
        /// </summary>
        /// <param name="producao">Objeto com os campos necessários para criação de uma produção</param>
        /// <returns>Dados da produção cadastrada</returns>
        /// <response code="201">Caso a inserção de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response> 
        /// <response code="404">Caso o id da receita seja inexistente na base de dados</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CadastroProducao(
            [FromBody] Producao producao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }

            var receita = await _contexto.Receitas.FirstOrDefaultAsync(x => x.Id == producao.ReceitaId);

            if (receita == null) return NotFound("Receita não encontrada");

            try
            {
                await _contexto.Producoes.AddAsync(producao);
                await _contexto.SaveChangesAsync();
                return Created($"api/producoes/{producao.Id}", producao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Atualiza uma produção no banco de dados usando seu id
        /// </summary>
        /// <param name="producao">Objeto com os campos necessários para atualização de uma producao</param>
        /// <param name="id">Id da producao a ser atualizado no banco</param>
        /// <returns>Dados da produção atualizados</returns>
        /// <response code="200">Caso a atualização de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>
        /// <response code="404">Caso o id da produção ou receita seja inexistente na base de dados</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizaDadosProducao(
            [FromBody] Producao producao,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var producaoAtualizar = await _contexto.Producoes.FirstOrDefaultAsync(x => x.Id == id);
            if (producaoAtualizar == null) return NotFound("Produção não encontrada");

            var receita = await _contexto.Receitas.FirstOrDefaultAsync(x => x.Id == producao.ReceitaId);

            if (receita == null) return NotFound("Receita não encontrada");

            try
            {
                producaoAtualizar.ReceitaId = producao.ReceitaId;
                producaoAtualizar.VolumeApronte = producao.VolumeApronte;
                producaoAtualizar.Responsavel = producao.Responsavel;
                producaoAtualizar.DataProducao = producao.DataProducao;

                _contexto.Producoes.Update(producao);
                await _contexto.SaveChangesAsync();
                return Ok(producaoAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma lista de produções no banco de dados  
        /// </summary>        
        /// <returns>Dados das produções</returns>
        /// <response code="200">Caso a lista de dados seja recuperada com sucesso</response>
        /// <response code="400">Requisição mal sucedida</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ListagemProducao()
        {
            try
            {
                return Ok(await _contexto.Producoes.AsNoTracking().ToListAsync());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma produção no banco de dados pelo seu id 
        /// </summary>        
        /// <param name="id">Id da produção a ser recuperado no banco</param>
        /// <returns>Dados da produção</returns>
        /// <response code="200">Caso a produção seja recuperada com sucesso</response>        
        /// <response code="404">Produção não encontrado no banco de dados</response
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarProducaoPorId(
            [FromRoute] int id)
        {
            var producao = await _contexto.Producoes
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return producao == null ? NotFound("Produção não encontrado!") : Ok(producao);
        }
        /// <summary>
        /// Deleta uma produção no banco de dados
        /// </summary>        
        /// <param name="id">Id da produção a ser deletado no banco</param>
        /// <returns>Sem dados para retornar</returns>
        /// <response code="204">Caso a produção seja deletada com sucesso</response>
        /// <response code="400">Erro no pedido de exclusão</response>
        /// <response code="404">Produção não encontrado no banco de dados</response>
        [HttpDelete]
        [Route("/api/producoes/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirProducao(
            [FromRoute] int id)
        {
            var producaoDeletar = await _contexto.Producoes                
                .FirstOrDefaultAsync(x => x.Id == id);

            if (producaoDeletar == null) return NotFound("Produção não encontrada");

            
            try
            {
                _contexto.Producoes.Remove(producaoDeletar);
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
