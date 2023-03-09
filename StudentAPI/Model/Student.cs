using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StudentAPI.Model
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required]
        public string Niveau { get; set; }
        [Required]
        public string Matricule { get; set; }
        public DateTime Created_date { get; set; }
        public DateTime Updated_date { get; set; }

    }
}
