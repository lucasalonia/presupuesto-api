using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace presupuesto_api.Models;

    [Table("Categorias")]
    public class Categoria
    {
        [Key]
        [Column("id_categoria")]
        public int Id { get; set; }
        
        [Required]
        [Column("id_usuario")]
        public int IdUsuario { get; set; }


        //Relacion necesaria para que lo identifique Entity Framework y pueda hacer la relación entre las tablas
       [ForeignKey(nameof(IdUsuario))]
        public Usuario? Usuario { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500)]
        [Column("descripcion")]
        public string? Descripcion { get; set; }

    
        [Column("fecha_creacion")]
        public DateTime FechaCreacion { get; set; }

        [Column("fecha_modificacion")]
        public DateTime? FechaModificacion { get; set; }

  
    }
