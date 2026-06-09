using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para autenticação (login).</summary>
public class LoginRequestDto
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Senha { get; set; } = string.Empty;
}

/// <summary>DTO de resposta com o token JWT gerado.</summary>
public class LoginResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiracao { get; set; }
    public string NomeUsuario { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

