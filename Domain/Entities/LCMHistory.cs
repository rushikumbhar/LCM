using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LCM.Domain.Entities
{
    public class LCMHistory
    {
        [Key] public int Id { get; set; }

        [Column(TypeName = "varchar(100)")] public string UserId { get; set; }

        [Column(TypeName = "varchar(100)")] public string Input { get; set; }

        [Column(TypeName = "varchar(100)")] public string Result { get; set; }
    }
}