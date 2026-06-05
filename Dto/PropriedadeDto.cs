using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para criação de uma Propriedade.</summary>
public class PropriedadeCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Range(0.1, 999999, ErrorMessage = "A área total deve estar entre 0,1 e 999999.")]
    public double AreaTotal { get; set; }

    [MaxLength(80)]
    public string? TipoSolo { get; set; }

    [Required(ErrorMessage = "O usuário é obrigatório.")]
    public long IdUsuario { get; set; }

    [Required(ErrorMessage = "A localização é obrigatória.")]
    public long IdLocalizacao { get; set; }
}

/// <summary>DTO para atualização de uma Propriedade.</summary>
public class PropriedadeUpdateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Range(0.1, 999999, ErrorMessage = "A área total deve estar entre 0,1 e 999999.")]
    public double AreaTotal { get; set; }

    [MaxLength(80)]
    public string? TipoSolo { get; set; }

    [Required(ErrorMessage = "O usuário é obrigatório.")]
    public long IdUsuario { get; set; }

    [Required(ErrorMessage = "A localização é obrigatória.")]
    public long IdLocalizacao { get; set; }
}

/// <summary>DTO de resposta com os dados da Propriedade.</summary>
public class PropriedadeResponseDto
{
    public long IdPropriedade { get; set; }
    public string Nome { get; set; } = string.Empty;
    public double AreaTotal { get; set; }
    public string? TipoSolo { get; set; }
    public long IdUsuario { get; set; }
    public long IdLocalizacao { get; set; }
}
