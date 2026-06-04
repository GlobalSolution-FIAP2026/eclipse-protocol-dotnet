using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_PROPRIEDADE")]
public class Propriedade
{
    [Key]
    [Column("ID_PROPRIEDADE")]
    public long IdPropriedade { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NM_PROPRIEDADE")]
    public string Nome { get; set; } = string.Empty;

    [Range(0.1, 999999)]
    [Column("NR_AREA_TOTAL")]
    public double AreaTotal { get; set; }

    [MaxLength(80)]
    [Column("TP_SOLO")]
    public string? TipoSolo { get; set; }

    [Required]
    [Column("ID_USUARIO")]
    public long IdUsuario { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario Usuario { get; set; } = null!;

    [Required]
    [Column("ID_LOCALIZACAO")]
    public long IdLocalizacao { get; set; }

    [ForeignKey(nameof(IdLocalizacao))]
    public Localizacao Localizacao { get; set; } = null!;

    public ICollection<Plantacao> Plantacoes { get; set; } = new List<Plantacao>();
}