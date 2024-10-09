using System.ComponentModel.DataAnnotations;

namespace NexusBijoux.web.Data.Entities
{
    public class User
    {
        [Key]
        public int User_ID { get; set; }

        [Display(Name = "Usuario")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(50, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        [RegularExpression(@"^\S*$", ErrorMessage = "El campo '{0}' no puede contener espacios.")]
        public string Name { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(50, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(50, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        public string LastName { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(200, ErrorMessage = "El campo '{0}' no puede tener más de {1} caracteres.")]
        public string? Address { get; set; }

        [Display(Name = "Correo Electrónico")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [EmailAddress(ErrorMessage = "El campo '{0}' no tiene un formato válido.")]
        public string Email { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "El campo '{0}' es requerido.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "El campo '{0}' debe tener entre {2} y {1} caracteres.")]
        public string Password { get; set; }
    }
}
