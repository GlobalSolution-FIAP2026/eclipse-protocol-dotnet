using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Propriedades.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropriedadeController : ControllerBase
{
    private readonly PropriedadeService _service;

    public PropriedadeController(PropriedadeService service) => _service = service;

    /// <summary>Retorna todas as propriedades.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todas as propriedades")]
    [ProducesResponseType(typeof(IEnumerable<PropriedadeResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        if (!lista.Any()) return NoContent();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna uma propriedade pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar propriedade por ID")]
    [ProducesResponseType(typeof(PropriedadeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Propriedade {id} não encontrada." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria uma nova propriedade.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar propriedade")]
    [ProducesResponseType(typeof(PropriedadeResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] PropriedadeCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Propriedade
        {
            Nome = dto.Nome,
            AreaTotal = dto.AreaTotal,
            TipoSolo = dto.TipoSolo,
            IdUsuario = dto.IdUsuario,
            IdLocalizacao = dto.IdLocalizacao
        };

        try
        {
            var criado = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = criado.IdPropriedade }, ToResponse(criado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Atualiza uma propriedade existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar propriedade")]
    [ProducesResponseType(typeof(PropriedadeResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] PropriedadeUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Propriedade
        {
            Nome = dto.Nome,
            AreaTotal = dto.AreaTotal,
            TipoSolo = dto.TipoSolo,
            IdUsuario = dto.IdUsuario,
            IdLocalizacao = dto.IdLocalizacao
        };

        try
        {
            var atualizado = await _service.AtualizarAsync(id, entity);
            if (atualizado is null) return NotFound(new { message = $"Propriedade {id} não encontrada." });
            return Ok(ToResponse(atualizado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Remove uma propriedade pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar propriedade")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var excluido = await _service.ExcluirAsync(id);
            if (!excluido) return NotFound(new { message = $"Propriedade {id} não encontrada." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static PropriedadeResponseDto ToResponse(Propriedade p) => new()
    {
        IdPropriedade = p.IdPropriedade,
        Nome = p.Nome,
        AreaTotal = p.AreaTotal,
        TipoSolo = p.TipoSolo,
        IdUsuario = p.IdUsuario,
        IdLocalizacao = p.IdLocalizacao
    };
}
