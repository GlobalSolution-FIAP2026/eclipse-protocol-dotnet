using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GlobalSolution.Dto;
using GlobalSolution.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Annotations;

namespace GlobalSolution.Controller;

/// <summary>Autenticação – geração de token JWT.</summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly UsuarioService _usuarioService;
    private readonly IConfiguration _config;

    public AuthController(UsuarioService usuarioService, IConfiguration config)
    {
        _usuarioService = usuarioService;
        _config = config;
    }

    /// <summary>Realiza login e retorna o token JWT.</summary>
    [HttpPost("login")]
    [SwaggerOperation(Summary = "Login – obtém o token JWT")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var usuarios = await _usuarioService.ListarTodosAsync();
        var usuario = usuarios.FirstOrDefault(u =>
            u.Email.Equals(dto.Email, StringComparison.OrdinalIgnoreCase) &&
            u.Senha == dto.Senha);

        if (usuario is null)
            return Unauthorized(new { message = "E-mail ou senha inválidos." });

        if (!usuario.Ativo)
            return Unauthorized(new { message = "Usuário inativo." });

        var token = GerarToken(usuario.IdUsuario, usuario.Nome, usuario.Email);

        return Ok(token);
    }

    private LoginResponseDto GerarToken(long id, string nome, string email)
    {
        var key     = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds   = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddHours(double.Parse(_config["Jwt:ExpiresInHours"] ?? "8"));

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub,   id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(JwtRegisteredClaimNames.Name,  nome),
            new Claim(JwtRegisteredClaimNames.Jti,   Guid.NewGuid().ToString())
        };

        var tokenObj = new JwtSecurityToken(
            issuer:             _config["Jwt:Issuer"],
            audience:           _config["Jwt:Audience"],
            claims:             claims,
            expires:            expires,
            signingCredentials: creds
        );

        return new LoginResponseDto
        {
            Token        = new JwtSecurityTokenHandler().WriteToken(tokenObj),
            Expiracao    = expires,
            NomeUsuario  = nome,
            Email        = email
        };
    }
}

