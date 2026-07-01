using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace presupuesto_api.Models;

public class Presupuesto
{
    [Key]
    [Column("id_presupuesto")]
    public int Id { get; set; }

    [Required]
    [Column("id_usuario")]
    public int IdUsuario { get; set; }

    [ForeignKey(nameof(IdUsuario))]
    public Usuario? Usuario { get; set; }

    [Required]
    [Column("nombre")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    [MaxLength(500)]
    public string? Descripcion { get; set; }

    [Column("fecha_inicio")]
    public DateTime? FechaInicio { get; set; }

    [Column("fecha_fin")]
    public DateTime? FechaFin { get; set; }

     [Column("fecha_creacion")]
    public DateTime  FechaCreacion{ get; set; } 
  
    [Column("fecha_modificacion")]
    public DateTime  FechaModificacion{ get; set; } 
   
}