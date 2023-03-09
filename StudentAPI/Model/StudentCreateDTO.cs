using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Model
{
    public class StudentCreateDTO
    {   
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Niveau { get; set; }
        [MaxLength(8)]
        public string Matricule { get; set; }
    }
}
