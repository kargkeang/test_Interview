using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Interview_Test.Models;

[Table("UserTb")]
public class UserModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    [Required]
    [Column(TypeName = "varchar(20)")]
    public string UserId { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string Username { get; set; }
    [Required]
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public UserProfileModel UserProfile { get; set; }
    [Required]
    public ICollection<UserRoleMappingModel> UserRoleMappings { get; set; }
}