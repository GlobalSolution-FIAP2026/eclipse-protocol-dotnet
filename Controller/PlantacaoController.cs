using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Plantações.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PlantacaoController : ControllerBase
{
    private readonly PlantacaoService _service;

    public PlantacaoController(PlantacaoService service) => _service = service;

    /// <summary>Retorna todas as plantações.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todas as plantações")]
    [ProducesResponseType(typeof(IEnumerable<PlantacaoResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        if (!lista.Any()) return NoContent();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna uma plantação pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar plantação por ID")]
    [ProducesResponseType(typeof(PlantacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Plantação {id} não encontrada." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria uma nova plantação.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar plantação")]
    [ProducesResponseType(typeof(PlantacaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] PlantacaoCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Plantacao
        {
            Nome = dto.Nome,
            Cultura = dto.Cultura,
            AreaHectares = dto.AreaHectares,
            Status = dto.Status,
            IdPropriedade = dto.IdPropriedade
        };

        try
        {
            var criado = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = criado.IdPlantacao }, ToResponse(criado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Atualiza uma plantação existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar plantação")]
    [ProducesResponseType(typeof(PlantacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] PlantacaoUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Plantacao
        {
            Nome = dto.Nome,
            Cultura = dto.Cultura,
            AreaHectares = dto.AreaHectares,
            Status = dto.Status,
            IdPropriedade = dto.IdPropriedade
        };

        try
        {
            var atualizado = await _service.AtualizarAsync(id, entity);
            if (atualizado is null) return NotFound(new { message = $"Plantação {id} não encontrada." });
            return Ok(ToResponse(atualizado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Remove uma plantação pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar plantação")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var excluido = await _service.ExcluirAsync(id);
            if (!excluido) return NotFound(new { message = $"Plantação {id} não encontrada." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static PlantacaoResponseDto ToResponse(Plantacao p) => new()
    {
        IdPlantacao = p.IdPlantacao,
        Nome = p.Nome,
        Cultura = p.Cultura,
        AreaHectares = p.AreaHectares,
        Status = p.Status,
        IdPropriedade = p.IdPropriedade
    };
}
