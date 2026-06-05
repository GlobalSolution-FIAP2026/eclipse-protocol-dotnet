using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para criação de um Alerta.</summary>
public class AlertaCreateDto
{
    [Required(ErrorMessage = "O tipo de alerta é obrigatório.")]
    [MaxLength(50)]
    public string TipoAlerta { get; set; } = string.Empty;

    [Required(ErrorMessage = "A severidade é obrigatória.")]
    [MaxLength(30)]
    public string Severidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [MaxLength(255)]
    public string Mensagem { get; set; } = string.Empty;

    [MaxLength(30)]
    public string Status { get; set; } = "ABERTO";

    [Required(ErrorMessage = "A leitura é obrigatória.")]
    public long IdLeitura { get; set; }

    [Required(ErrorMessage = "A plantação é obrigatória.")]
    public long IdPlantacao { get; set; }
}

/// <summary>DTO para atualização de um Alerta.</summary>
public class AlertaUpdateDto
{
    [Required(ErrorMessage = "O tipo de alerta é obrigatório.")]
    [MaxLength(50)]
    public string TipoAlerta { get; set; } = string.Empty;

    [Required(ErrorMessage = "A severidade é obrigatória.")]
    [MaxLength(30)]
    public string Severidade { get; set; } = string.Empty;

    [Required(ErrorMessage = "A mensagem é obrigatória.")]
    [MaxLength(255)]
    public string Mensagem { get; set; } = string.Empty;

    [Required(ErrorMessage = "O status é obrigatório.")]
    [MaxLength(30)]
    public string Status { get; set; } = "ABERTO";

    [Required(ErrorMessage = "A leitura é obrigatória.")]
    public long IdLeitura { get; set; }

    [Required(ErrorMessage = "A plantação é obrigatória.")]
    public long IdPlantacao { get; set; }
}

/// <summary>DTO de resposta com os dados do Alerta.</summary>
public class AlertaResponseDto
{
    public long IdAlerta { get; set; }
    public string TipoAlerta { get; set; } = string.Empty;
    public string Severidade { get; set; } = string.Empty;
    public string Mensagem { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public long IdLeitura { get; set; }
    public long IdPlantacao { get; set; }
}
