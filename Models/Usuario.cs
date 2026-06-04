using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GlobalSolution.Models;

[Table("TB_USUARIO")]
public class Usuario
{
    [Key]
    [Column("ID_USUARIO")]
    public long IdUsuario { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("NM_USUARIO")]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    [Column("DS_EMAIL")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("DS_SENHA")]
    public string Senha { get; set; } = string.Empty;

    [Column("ST_ATIVO")]
    public bool Ativo { get; set; } = true;

    [Column("DT_CRIACAO")]
    public DateTime DataCriacao { get; set; } = DateTime.Now;

    public ICollection<Propriedade> Propriedades { get; set; } = new List<Propriedade>();
}