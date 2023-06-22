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

        [HttpPost]
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
                return Created($"api/receitas/{producao.Id}", producao);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
        [HttpPut]
        [Route("{id}")]
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
        [HttpGet]
        
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
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ListarProducaoPorId(
            [FromRoute] int id)
        {
            var producao = await _contexto.Producoes
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return producao == null ? NotFound("Produção não encontrado!") : Ok(producao);
        }
        [HttpDelete]
        [Route("/api/producoes/{id}")]
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
