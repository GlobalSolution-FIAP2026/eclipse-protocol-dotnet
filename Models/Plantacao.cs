using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_PLANTACAO")]
public class Plantacao
{
    [Key]
    [Column("ID_PLANTACAO")]
    public long IdPlantacao { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NM_PLANTACAO")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    [Column("DS_CULTURA")]
    public string Cultura { get; set; } = string.Empty;

    [Range(0.1, 999999)]
    [Column("NR_AREA_HECTARES", TypeName = "FLOAT")]
    public double AreaHectares { get; set; }

    [Required]
    [MaxLength(30)]
    [Column("DS_STATUS")]
    public string Status { get; set; } = "ATIVA";

    [Required]
    [Column("ID_PROPRIEDADE")]
    public long IdPropriedade { get; set; }

    [ForeignKey(nameof(IdPropriedade))]
    public Propriedade Propriedade { get; set; } = null!;

    public ICollection<Sensor> Sensores { get; set; } = new List<Sensor>();
    public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
}