using System.ComponentModel.DataAnnotations;

namespace NexusBijoux.web.Data.Entities
{
    public class Permission
    {
        [Key]
        public int Permission_ID { get; set; }

        [MaxLength(128, ErrorMessage = "El campo '{0}' debe tener como máximo '{1}' caracteres.")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public string Name { get; set; }

        [MaxLength(256, ErrorMessage = "El campo '{0}' debe tener como máximo '{1}' caracteres.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
