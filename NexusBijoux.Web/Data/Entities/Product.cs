using System.ComponentModel.DataAnnotations;

namespace NexusBijoux.web.Data.Entities
{
    public class Product
    {
        [Key]
        public int Product_ID { get; set; }

        [Display(Name = "Nombre del Producto")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(100, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        public string Product_Name { get; set; }

        [Display(Name = "Descripción del Producto")]
        [StringLength(500, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        public string? Product_Description { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe estar entre {1} y {2} euros.")]
        public decimal Price { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(50, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        public string Category { get; set; }
    }
}
