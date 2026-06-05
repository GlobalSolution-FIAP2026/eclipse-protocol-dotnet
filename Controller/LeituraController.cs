using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Leituras.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class LeituraController : ControllerBase
{
    private readonly LeituraService _service;

    public LeituraController(LeituraService service) => _service = service;

    /// <summary>Retorna todas as leituras.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todas as leituras")]
    [ProducesResponseType(typeof(IEnumerable<LeituraResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna uma leitura pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar leitura por ID")]
    [ProducesResponseType(typeof(LeituraResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Leitura {id} não encontrada." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria uma nova leitura.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar leitura")]
    [ProducesResponseType(typeof(LeituraResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] LeituraCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Leitura
        {
            Temperatura = dto.Temperatura,
            Umidade = dto.Umidade,
            Precipitacao = dto.Precipitacao,
            Ndvi = dto.Ndvi,
            IdSensor = dto.IdSensor
        };

        try
        {
            var criado = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = criado.IdLeitura }, ToResponse(criado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Atualiza uma leitura existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar leitura")]
    [ProducesResponseType(typeof(LeituraResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] LeituraUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Leitura
        {
            Temperatura = dto.Temperatura,
            Umidade = dto.Umidade,
            Precipitacao = dto.Precipitacao,
            Ndvi = dto.Ndvi,
            IdSensor = dto.IdSensor
        };

        try
        {
            var atualizado = await _service.AtualizarAsync(id, entity);
            if (atualizado is null) return NotFound(new { message = $"Leitura {id} não encontrada." });
            return Ok(ToResponse(atualizado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Remove uma leitura pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar leitura")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var excluido = await _service.ExcluirAsync(id);
            if (!excluido) return NotFound(new { message = $"Leitura {id} não encontrada." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static LeituraResponseDto ToResponse(Leitura l) => new()
    {
        IdLeitura = l.IdLeitura,
        Temperatura = l.Temperatura,
        Umidade = l.Umidade,
        Precipitacao = l.Precipitacao,
        Ndvi = l.Ndvi,
        DataLeitura = l.DataLeitura,
        IdSensor = l.IdSensor
    };
}
