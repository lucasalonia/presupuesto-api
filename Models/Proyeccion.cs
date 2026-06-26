using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace presupuesto_api.Models;

public class Proyeccion
{
    [Key]
    [Column("id_proyeccion")]
    public int Id { get; set; }

    [Required]
    [Column("id_presupuesto")]
    public int IdPresupuesto { get; set; }

    [ForeignKey(nameof(IdPresupuesto))]
    public Presupuesto? Presupuesto { get; set; }

    //Relacion de uno a muchos con la entidad Categoria. Esta es opcional.
    [Column("id_categoria")]
    public int? IdCategoria { get; set; }

    [ForeignKey(nameof(IdCategoria))]
    public Categoria? Categoria { get; set; }

    [Required]
    [Column("tipo")]
    public bool Tipo { get; set; } // true = ingreso, false = gasto

    [Required]
    [Column("nombre")]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Column("descripcion")]
    [MaxLength(500)]
    public string Descripcion { get; set; } = string.Empty;

    [Required]
    [Column("monto")]
    public decimal Monto { get; set; }

     [Column("fecha_creacion")]
    public DateTime  FechaCreacion{ get; set; } 
  
    [Column("fecha_modificacion")]
    public DateTime  FechaModificacion{ get; set; } 

    public ICollection<Proyeccion> Proyecciones { get; set; } = new List<Proyeccion>();
}