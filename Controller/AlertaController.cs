using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Alertas.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AlertaController : ControllerBase
{
    private readonly AlertaService _service;

    public AlertaController(AlertaService service) => _service = service;

    /// <summary>Retorna todos os alertas.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todos os alertas")]
    [ProducesResponseType(typeof(IEnumerable<AlertaResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna um alerta pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar alerta por ID")]
    [ProducesResponseType(typeof(AlertaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Alerta {id} não encontrado." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria um novo alerta.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar alerta")]
    [ProducesResponseType(typeof(AlertaResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] AlertaCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Alerta
        {
            TipoAlerta = dto.TipoAlerta,
            Severidade = dto.Severidade,
            Mensagem = dto.Mensagem,
            Status = dto.Status,
            IdLeitura = dto.IdLeitura,
            IdPlantacao = dto.IdPlantacao
        };

        try
        {
            var criado = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = criado.IdAlerta }, ToResponse(criado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Atualiza um alerta existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar alerta")]
    [ProducesResponseType(typeof(AlertaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] AlertaUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Alerta
        {
            TipoAlerta = dto.TipoAlerta,
            Severidade = dto.Severidade,
            Mensagem = dto.Mensagem,
            Status = dto.Status,
            IdLeitura = dto.IdLeitura,
            IdPlantacao = dto.IdPlantacao
        };

        try
        {
            var atualizado = await _service.AtualizarAsync(id, entity);
            if (atualizado is null) return NotFound(new { message = $"Alerta {id} não encontrado." });
            return Ok(ToResponse(atualizado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Remove um alerta pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar alerta")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(long id)
    {
        var excluido = await _service.ExcluirAsync(id);
        if (!excluido) return NotFound(new { message = $"Alerta {id} não encontrado." });
        return NoContent();
    }

    private static AlertaResponseDto ToResponse(Alerta a) => new()
    {
        IdAlerta = a.IdAlerta,
        TipoAlerta = a.TipoAlerta,
        Severidade = a.Severidade,
        Mensagem = a.Mensagem,
        Status = a.Status,
        DataCriacao = a.DataCriacao,
        IdLeitura = a.IdLeitura,
        IdPlantacao = a.IdPlantacao
    };
}
