using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para criação de um Sensor.</summary>
public class SensorCreateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    [Required(ErrorMessage = "A plantação é obrigatória.")]
    public long IdPlantacao { get; set; }
}

/// <summary>DTO para atualização de um Sensor.</summary>
public class SensorUpdateDto
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [Required(ErrorMessage = "O tipo é obrigatório.")]
    [MaxLength(50)]
    public string Tipo { get; set; } = string.Empty;

    public bool Ativo { get; set; } = true;

    [Required(ErrorMessage = "A plantação é obrigatória.")]
    public long IdPlantacao { get; set; }
}

/// <summary>DTO de resposta com os dados do Sensor.</summary>
public class SensorResponseDto
{
    public long IdSensor { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public bool Ativo { get; set; }
    public DateTime DataInstalacao { get; set; }
    public long IdPlantacao { get; set; }
}
