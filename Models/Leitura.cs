using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_LEITURA")]
public class Leitura
{
    [Key]
    [Column("ID_LEITURA")]
    public long IdLeitura { get; set; }

    [Range(-50, 80)]
    [Column("NR_TEMPERATURA")]
    public double? Temperatura { get; set; }

    [Range(0, 100)]
    [Column("NR_UMIDADE")]
    public double? Umidade { get; set; }

    [Range(0, 1000)]
    [Column("NR_PRECIPITACAO")]
    public double? Precipitacao { get; set; }

    [Range(-1, 1)]
    [Column("NR_NDVI")]
    public double? Ndvi { get; set; }

    [Column("DT_LEITURA")]
    public DateTime DataLeitura { get; set; } = DateTime.Now;

    [Required]
    [Column("ID_SENSOR")]
    public long IdSensor { get; set; }

    [ForeignKey(nameof(IdSensor))]
    public Sensor Sensor { get; set; } = null!;

    public ICollection<Alerta> Alertas { get; set; } = new List<Alerta>();
}