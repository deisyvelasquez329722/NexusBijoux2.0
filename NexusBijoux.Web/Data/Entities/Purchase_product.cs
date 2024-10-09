using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NexusBijoux.Web.Data.Entities
{
    public class Purchase_product
    {
        [Key]
        public int Purchase_product_ID { get; set; }

        // Clave foránea que referencia a la entidad User
        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [ForeignKey("User")]
        public int User_ID { get; set; }

        // Relación con la entidad User
        public User User { get; set; }

        // Clave foránea que referencia a la entidad Product
        [Display(Name = "Producto")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [ForeignKey("Product")]
        public int Product_ID { get; set; }

        // Relación con la entidad Product
        public Product Product { get; set; }

        [Display(Name = "Cantidad")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre {1} y {2}.")]
        public int Quantity { get; set; }

        [Display(Name = "Precio")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [Range(0.01, 10000.00, ErrorMessage = "El precio debe estar entre {1} y {2} euros.")]
        public decimal Price { get; set; }
    }
}
