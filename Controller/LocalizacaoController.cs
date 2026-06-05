using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Localizações.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LocalizacaoController : ControllerBase
{
    private readonly LocalizacaoService _service;

    public LocalizacaoController(LocalizacaoService service) => _service = service;

    /// <summary>Retorna todas as localizações.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todas as localizações")]
    [ProducesResponseType(typeof(IEnumerable<LocalizacaoResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna uma localização pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar localização por ID")]
    [ProducesResponseType(typeof(LocalizacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Localização {id} não encontrada." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria uma nova localização.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar localização")]
    [ProducesResponseType(typeof(LocalizacaoResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LocalizacaoCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Localizacao
        {
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Pais = dto.Pais,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Cep = dto.Cep
        };

        var criado = await _service.CriarAsync(entity);
        return CreatedAtAction(nameof(GetById), new { id = criado.IdLocalizacao }, ToResponse(criado));
    }

    /// <summary>Atualiza uma localização existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar localização")]
    [ProducesResponseType(typeof(LocalizacaoResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] LocalizacaoUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Localizacao
        {
            Cidade = dto.Cidade,
            Estado = dto.Estado,
            Pais = dto.Pais,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Cep = dto.Cep
        };

        var atualizado = await _service.AtualizarAsync(id, entity);
        if (atualizado is null) return NotFound(new { message = $"Localização {id} não encontrada." });
        return Ok(ToResponse(atualizado));
    }

    /// <summary>Remove uma localização pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar localização")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var excluido = await _service.ExcluirAsync(id);
            if (!excluido) return NotFound(new { message = $"Localização {id} não encontrada." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static LocalizacaoResponseDto ToResponse(Localizacao l) => new()
    {
        IdLocalizacao = l.IdLocalizacao,
        Cidade = l.Cidade,
        Estado = l.Estado,
        Pais = l.Pais,
        Latitude = l.Latitude,
        Longitude = l.Longitude,
        Cep = l.Cep
    };
}
