using System.ComponentModel.DataAnnotations;

namespace StudentAPI.Model
{
    public class StudentUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Niveau { get; set; }
        [Required]
        [MaxLength(8)]
        public string Matricule { get; set; }
    }
}
