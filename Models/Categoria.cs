using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace presupuesto_api.Models;

    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int IdCategoria { get; set; }

        [Required]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("fecha_modificacion")]
        public DateTime? FechaModificacion { get; set; }

  
    }
