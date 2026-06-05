using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para criação de uma Plantação.</summary>
public class PlantacaoCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cultura é obrigatória.")]
    [MaxLength(80)]
    public string Cultura { get; set; } = string.Empty;

    [Range(0.1, 999999, ErrorMessage = "A área deve estar entre 0,1 e 999999 hectares.")]
    public double AreaHectares { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    [MaxLength(30)]
    public string Status { get; set; } = "ATIVA";

    [Required(ErrorMessage = "A propriedade é obrigatória.")]
    public long IdPropriedade { get; set; }
}

/// <summary>DTO para atualização de uma Plantação.</summary>
public class PlantacaoUpdateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cultura é obrigatória.")]
    [MaxLength(80)]
    public string Cultura { get; set; } = string.Empty;

    [Range(0.1, 999999, ErrorMessage = "A área deve estar entre 0,1 e 999999 hectares.")]
    public double AreaHectares { get; set; }

    [Required(ErrorMessage = "O status é obrigatório.")]
    [MaxLength(30)]
    public string Status { get; set; } = "ATIVA";

    [Required(ErrorMessage = "A propriedade é obrigatória.")]
    public long IdPropriedade { get; set; }
}

/// <summary>DTO de resposta com os dados da Plantação.</summary>
public class PlantacaoResponseDto
{
    public long IdPlantacao { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Cultura { get; set; } = string.Empty;
    public double AreaHectares { get; set; }
    public string Status { get; set; } = string.Empty;
    public long IdPropriedade { get; set; }
}
