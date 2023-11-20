using System.ComponentModel.DataAnnotations;

namespace WebMvcProject.Models
{
    public class LoginViewModel
    {
        //[Display(Name="Kullanıcı Adı", Prompt ="cisema." )]
        [Required(ErrorMessage = "Username is required")]
        [StringLength(30, ErrorMessage = "Username can be max 30 characters")]
        public string UserName { get; set; }

        //[DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password can be min 6 characters")]
        [MaxLength(12, ErrorMessage = "Password can be max 12 characters")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
