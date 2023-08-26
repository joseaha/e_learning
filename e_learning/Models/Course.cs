using System.ComponentModel.DataAnnotations;

namespace e_learning.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Nombre del curso")]
        public string name { get; set; }
        [Display(Name = "Descripcion del curso")]
        [Required(ErrorMessage = "La descripcion es obligatoria")]
        public string description { get; set; }
        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio del curso")]
        public float price { get; set; }
        [Required(ErrorMessage = "El recurso del curso es obligatorio")]
        [Display(Name = "Material del curso url")]
        public string VideoUrl { get; set; }
        [Display(Name = "Portada del curso url")]
        public string imgUrl { get; set; }

        public bool estado { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
