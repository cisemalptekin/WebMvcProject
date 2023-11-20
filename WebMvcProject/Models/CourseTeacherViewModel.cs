using System.ComponentModel.DataAnnotations;

namespace WebMvcProject.Models
{
    public class CourseTeacherViewModel
    {
        [Required(ErrorMessage = "Kurs Seçimi Yapınız")]
        public int? CourseId { get; set; }

        [Required(ErrorMessage = "Öğretmen Seçimi Yapınız")]
        public int? TeacherId { get; set; }
    }
}
