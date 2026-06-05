using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para criação de uma Localização.</summary>
public class LocalizacaoCreateDto
{
    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [MaxLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [MaxLength(2)]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "O país é obrigatório.")]
    [MaxLength(80)]
    public string Pais { get; set; } = "Brasil";

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [MaxLength(20)]
    public string? Cep { get; set; }
}

/// <summary>DTO para atualização de uma Localização.</summary>
public class LocalizacaoUpdateDto
{
    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [MaxLength(100)]
    public string Cidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [MaxLength(2)]
    public string Estado { get; set; } = string.Empty;

    [Required(ErrorMessage = "O país é obrigatório.")]
    [MaxLength(80)]
    public string Pais { get; set; } = "Brasil";

    public double? Latitude { get; set; }

    public double? Longitude { get; set; }

    [MaxLength(20)]
    public string? Cep { get; set; }
}

/// <summary>DTO de resposta com os dados da Localização.</summary>
public class LocalizacaoResponseDto
{
    public long IdLocalizacao { get; set; }
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Pais { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Cep { get; set; }
}
