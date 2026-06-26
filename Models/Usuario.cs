using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace presupuesto_api.Models;
public class Usuario
{
    [Key]
    [Column("id_usuario")]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    [Column("nickname")]
    public string Nickname { get; set; } = null!;

    [Required]
    [MaxLength(150)]
    [EmailAddress]
    [Column("correo")]
    public string Correo { get; set; } = null!;

    [Required]
    [MaxLength(255)]
    [Column("contraseña")]
    public string Contraseña { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    [Column("rol")]
    public string Rol { get; set; } = "Persona";
  
    [Column("fecha_creacion")]
    public DateTime  FechaCreacion{ get; set; } 
  
    [Column("fecha_modificacion")]
    public DateTime  FechaModificacion{ get; set; } 

    // Relación uno a muchos con la entidad Categoria. ENTITY FRAMEWORK NECESITA ESTO PARA PODER RELACIONAR LAS TABLAS
    public ICollection<Categoria> Categorias { get; set; }
    = new List<Categoria>();
}