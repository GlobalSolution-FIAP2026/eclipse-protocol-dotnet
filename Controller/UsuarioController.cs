using GlobalSolution.Dto;
using GlobalSolution.Models;
using GlobalSolution.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Gerenciamento de Usuários.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class UsuarioController : ControllerBase
{
    private readonly UsuarioService _service;

    public UsuarioController(UsuarioService service) => _service = service;

    /// <summary>Retorna todos os usuários.</summary>
    [HttpGet]
    [SwaggerOperation(Summary = "Listar todos os usuários")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var lista = await _service.ListarTodosAsync();
        return Ok(lista.Select(ToResponse));
    }

    /// <summary>Retorna um usuário pelo ID.</summary>
    [HttpGet("{id:long}")]
    [SwaggerOperation(Summary = "Buscar usuário por ID")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(long id)
    {
        var entity = await _service.BuscarPorIdAsync(id);
        if (entity is null) return NotFound(new { message = $"Usuário {id} não encontrado." });
        return Ok(ToResponse(entity));
    }

    /// <summary>Cria um novo usuário.</summary>
    [HttpPost]
    [SwaggerOperation(Summary = "Criar usuário")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] UsuarioCreateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var entity = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = dto.Senha,
            Ativo = dto.Ativo
        };

        try
        {
            var criado = await _service.CriarAsync(entity);
            return CreatedAtAction(nameof(GetById), new { id = criado.IdUsuario }, ToResponse(criado));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Atualiza um usuário existente.</summary>
    [HttpPut("{id:long}")]
    [SwaggerOperation(Summary = "Atualizar usuário")]
    [ProducesResponseType(typeof(UsuarioResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(long id, [FromBody] UsuarioUpdateDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var existente = await _service.BuscarPorIdAsync(id);
        if (existente is null) return NotFound(new { message = $"Usuário {id} não encontrado." });

        var entity = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Senha = string.IsNullOrWhiteSpace(dto.Senha) ? existente.Senha : dto.Senha,
            Ativo = dto.Ativo
        };

        try
        {
            var atualizado = await _service.AtualizarAsync(id, entity);
            return Ok(ToResponse(atualizado!));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>Remove um usuário pelo ID.</summary>
    [HttpDelete("{id:long}")]
    [SwaggerOperation(Summary = "Deletar usuário")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(long id)
    {
        try
        {
            var excluido = await _service.ExcluirAsync(id);
            if (!excluido) return NotFound(new { message = $"Usuário {id} não encontrado." });
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static UsuarioResponseDto ToResponse(Usuario u) => new()
    {
        IdUsuario = u.IdUsuario,
        Nome = u.Nome,
        Email = u.Email,
        Ativo = u.Ativo,
        DataCriacao = u.DataCriacao
    };
}
