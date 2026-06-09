using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_SENSOR")]
public class Sensor
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("ID_SENSOR")]
    public long IdSensor { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NM_SENSOR")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [Column("TP_SENSOR")]
    public string Tipo { get; set; } = string.Empty;

    [Column("ST_ATIVO", TypeName = "NUMBER(1)")]
    public bool Ativo { get; set; } = true;

    [Column("DT_INSTALACAO")]
    public DateTime DataInstalacao { get; set; } = DateTime.Now;

    [Required]
    [Column("ID_PLANTACAO")]
    public long IdPlantacao { get; set; }

    [ForeignKey(nameof(IdPlantacao))]
    public Plantacao Plantacao { get; set; } = null!;

    public ICollection<Leitura> Leituras { get; set; } = new List<Leitura>();
}