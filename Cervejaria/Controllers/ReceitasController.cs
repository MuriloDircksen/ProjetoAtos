using Cervejaria.Contexto;
using Cervejaria.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cervejaria.Controllers
{
    [ApiController]
    [Route("api/receitas")]
    public class ReceitasController : ControllerBase
    {
        private readonly CervejariaContexto _contexto;

        public ReceitasController(CervejariaContexto contexto)
        {
            _contexto = contexto;
        }
        [HttpPost]
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
        [HttpPut]
        [Route("{id}")]
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
        [HttpGet]
        
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
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> ListarReceitaPorId(
            [FromRoute] int id)
        {
            var receita = await _contexto.Receitas
                .AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return receita == null ? NotFound("Receita não encontrado!") : Ok(receita);
        }
        [HttpDelete]
        [Route("/api/receitas/{id}")]
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
