using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace LCM.Domain.Entities
{
    public class LCMUser : IdentityUser
    {
        [Column(TypeName = "varchar(50)")] public string FullName { get; set; }
    }
}