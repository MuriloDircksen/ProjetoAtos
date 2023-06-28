using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/receitas")]
    public class ReceitasController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public ReceitasController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }
        /// <summary>
        /// Salva uma receita no banco de dados 
        /// </summary>
        /// <param name="receita">Objeto com os campos necessários para criação de uma receita</param>
        /// <returns>Dados da receita cadastrado</returns>
        /// <response code="201">Caso a inserção de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>    
        /// <response code="401">Acesso não autorizado, token inválido</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CadastroReceita(
            [FromBody] Receita receita)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }            

            try
            {
                await _contexto.Receitas.AddAsync(receita);
                await _contexto.SaveChangesAsync();
                return Created($"api/receitas/{receita.Id}", receita);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Atualiza uma receita no banco de dados usando seu id
        /// </summary>
        /// <param name="receita">Objeto com os campos necessários para atualização de uma receita</param>
        /// <param name="id">Id da receita a ser atualizado no banco</param>
        /// <returns>Dados da receita atualizado</returns>
        /// <response code="200">Caso a atualização de dados seja feita com sucesso</response>
        /// <response code="400">Dados inválidos inseridos</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Caso o id seja inexistente na base de dados</response>
        [HttpPut]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizaDadosReceita(
            [FromBody] Receita receita,
            [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Dados inválidos, favor verificar o formato obrigatório dos dados!");
            }
            var receitaAtualizar = await _contexto.Receitas.FirstOrDefaultAsync(x => x.Id == id);
            if (receitaAtualizar == null) return NotFound("Receita não encontrada");

            try
            {
                receitaAtualizar.NomeReceita = receita.NomeReceita;
                receitaAtualizar.Responsavel = receita.Responsavel;
                receitaAtualizar.Estilo = receita.Estilo;
                receitaAtualizar.UltimaAtualizacao = receita.UltimaAtualizacao;
                receitaAtualizar.Orcamento = receita.Orcamento;
                receitaAtualizar.VolumeReceita = receita.VolumeReceita;

                _contexto.Receitas.Update(receitaAtualizar);
                await _contexto.SaveChangesAsync();
                return Ok(receitaAtualizar);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma lista de receitas no banco de dados  
        /// </summary>        
        /// <returns>Dados das receitas</returns>
        /// <response code="200">Caso a lista de dados seja recuperada com sucesso</response>
        /// <response code="400">Requisição mal sucedida</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ListagemReceita()
        {
            try
            {
                return Ok(await _contexto.Receitas.AsNoTracking().ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest( ex.ToString());
            }
        }
        /// <summary>
        /// Recupera uma receita no banco de dados pelo seu id 
        /// </summary>        
        /// <param name="id">Id da receita a ser recuperado no banco</param>
        /// <returns>Dados da receita</returns>
        /// <response code="200">Caso a receita seja recuperada com sucesso</response>   
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Receita não encontrado no banco de dados</response>
        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ListarReceitaPorId(
            [FromRoute] int id)
        {
            var receita = await _contexto.Receitas
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return receita == null ? NotFound("Receita não encontrado!") : Ok(receita);
        }
        /// <summary>
        /// Deleta uma receita no banco de dados conforme validaçoes e suas relações ingrediente receita
        /// </summary>        
        /// <param name="id">Id da receita a ser deletada no banco</param>
        /// <returns>Sem dados para retornar</returns>
        /// <response code="204">Caso a receita seja deletada com sucesso</response>
        /// <response code="400">Proibida a exclusão de receita com produção associada</response>
        /// <response code="401">Acesso não autorizado, token inválido</response>
        /// <response code="404">Receita não encontrado no banco de dados</response>
        [HttpDelete]
        [Route("/api/receitas/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]      
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ExcluirReceita(
            [FromRoute] int id)
        {
            var receitaDeletar = await _contexto.Receitas
                .Include(x=>x.Producao)
                .FirstOrDefaultAsync(x => x.Id == id);

            if(receitaDeletar == null) return NotFound("Receita não encontrada");

            if(receitaDeletar.Producao.FirstOrDefault() != null )
            {
                return BadRequest("Não é permitido exclusão de receita com produção associada");
            }
            try
            {
                _contexto.Receitas.Remove(receitaDeletar);
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
