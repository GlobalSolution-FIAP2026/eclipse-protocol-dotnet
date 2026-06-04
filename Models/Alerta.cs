using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_ALERTA")]
public class Alerta
{
    [Key]
    [Column("ID_ALERTA")]
    public long IdAlerta { get; set; }

    [Required]
    [MaxLength(50)]
    [Column("TP_ALERTA")]
    public string TipoAlerta { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Column("DS_SEVERIDADE")]
    public string Severidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("DS_MENSAGEM")]
    public string Mensagem { get; set; } = string.Empty;

    [Required]
    [MaxLength(30)]
    [Column("DS_STATUS")]
    public string Status { get; set; } = "ABERTO";

    [Column("DT_CRIACAO")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    [Required]
    [Column("ID_LEITURA")]
    public long IdLeitura { get; set; }

    [ForeignKey(nameof(IdLeitura))]
    public Leitura Leitura { get; set; } = null!;

    [Required]
    [Column("ID_PLANTACAO")]
    public long IdPlantacao { get; set; }

    [ForeignKey(nameof(IdPlantacao))]
    public Plantacao Plantacao { get; set; } = null!;
}