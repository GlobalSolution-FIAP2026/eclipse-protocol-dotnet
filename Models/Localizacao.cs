using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_LOCALIZACAO")]
public class Localizacao
{
    [Key]
    [Column("ID_LOCALIZACAO")]
    public long IdLocalizacao { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NM_CIDADE")]
    public string Cidade { get; set; } = string.Empty;

    [Required]
    [MaxLength(2)]
    [Column("SG_ESTADO")]
    public string Estado { get; set; } = string.Empty;

    [Required]
    [MaxLength(80)]
    [Column("NM_PAIS")]
    public string Pais { get; set; } = "Brasil";

    [Column("NR_LATITUDE")]
    public double? Latitude { get; set; }

    [Column("NR_LONGITUDE")]
    public double? Longitude { get; set; }

    [MaxLength(20)]
    [Column("NR_CEP")]
    public string? Cep { get; set; }

    public ICollection<Propriedade> Propriedades { get; set; } = new List<Propriedade>();
}