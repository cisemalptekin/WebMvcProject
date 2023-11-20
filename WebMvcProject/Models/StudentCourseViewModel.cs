using System.ComponentModel.DataAnnotations;

namespace WebMvcProject.Models
{
    public class StudentCourseViewModel
    {
        [Required(ErrorMessage = "Kurs Seçimi Yapınız")]
        public int? CourseId { get; set; }
        [Required(ErrorMessage = "Öğrenci Seçimi Yapınız")]
        public int? StudentId { get; set; }
    }
}

