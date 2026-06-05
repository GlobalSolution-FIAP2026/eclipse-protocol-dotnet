using System.ComponentModel.DataAnnotations;

namespace GlobalSolution.Dto;

/// <summary>DTO para criação de uma Leitura.</summary>
public class LeituraCreateDto
{
    [Range(-50, 80, ErrorMessage = "A temperatura deve estar entre -50 e 80.")]
    public double? Temperatura { get; set; }

    [Range(0, 100, ErrorMessage = "A umidade deve estar entre 0 e 100.")]
    public double? Umidade { get; set; }

    [Range(0, 1000, ErrorMessage = "A precipitação deve estar entre 0 e 1000.")]
    public double? Precipitacao { get; set; }

    [Range(-1, 1, ErrorMessage = "O NDVI deve estar entre -1 e 1.")]
    public double? Ndvi { get; set; }

    [Required(ErrorMessage = "O sensor é obrigatório.")]
    public long IdSensor { get; set; }
}

/// <summary>DTO para atualização de uma Leitura.</summary>
public class LeituraUpdateDto
{
    [Range(-50, 80, ErrorMessage = "A temperatura deve estar entre -50 e 80.")]
    public double? Temperatura { get; set; }

    [Range(0, 100, ErrorMessage = "A umidade deve estar entre 0 e 100.")]
    public double? Umidade { get; set; }

    [Range(0, 1000, ErrorMessage = "A precipitação deve estar entre 0 e 1000.")]
    public double? Precipitacao { get; set; }

    [Range(-1, 1, ErrorMessage = "O NDVI deve estar entre -1 e 1.")]
    public double? Ndvi { get; set; }

    [Required(ErrorMessage = "O sensor é obrigatório.")]
    public long IdSensor { get; set; }
}

/// <summary>DTO de resposta com os dados da Leitura.</summary>
public class LeituraResponseDto
{
    public long IdLeitura { get; set; }
    public double? Temperatura { get; set; }
    public double? Umidade { get; set; }
    public double? Precipitacao { get; set; }
    public double? Ndvi { get; set; }
    public DateTime DataLeitura { get; set; }
    public long IdSensor { get; set; }
}
