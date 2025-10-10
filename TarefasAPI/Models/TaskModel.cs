using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TarefasAPI.Models
{
    public class TaskModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Titulo { get; set; } = string.Empty;
        [MaxLength(500)]
        public string Descricao { get; set; } = string.Empty;
        public bool Concluida { get; set; }
    }
}
