using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Sensores.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class SensorController : ControllerBase
{
    private readonly SensorService _service;

    public SensorController(SensorService service) => _service = service;

    /// <summary>Retorna todos os sensores.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todos os sensores")]
    [ProducesResponseType(typeof(IEnumerable<SensorResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna um sensor pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar sensor por ID")]
    [ProducesResponseType(typeof(SensorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Sensor {id} não encontrado." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria um novo sensor.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar sensor")]
    [ProducesResponseType(typeof(SensorResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] SensorCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Sensor
        {
            Nome = dto.Nome,
            Tipo = dto.Tipo,
            Ativo = dto.Ativo,
            IdPlantacao = dto.IdPlantacao
        };

        try
        {
            var criado = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = criado.IdSensor }, ToResponse(criado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Atualiza um sensor existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar sensor")]
    [ProducesResponseType(typeof(SensorResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] SensorUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Sensor
        {
            Nome = dto.Nome,
            Tipo = dto.Tipo,
            Ativo = dto.Ativo,
            IdPlantacao = dto.IdPlantacao
        };

        try
        {
            var atualizado = await _service.AtualizarAsync(id, entity);
            if (atualizado is null) return NotFound(new { message = $"Sensor {id} não encontrado." });
            return Ok(ToResponse(atualizado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Remove um sensor pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar sensor")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var excluido = await _service.ExcluirAsync(id);
            if (!excluido) return NotFound(new { message = $"Sensor {id} não encontrado." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static SensorResponseDto ToResponse(Sensor s) => new()
    {
        IdSensor = s.IdSensor,
        Nome = s.Nome,
        Tipo = s.Tipo,
        Ativo = s.Ativo,
        DataInstalacao = s.DataInstalacao,
        IdPlantacao = s.IdPlantacao
    };
}
