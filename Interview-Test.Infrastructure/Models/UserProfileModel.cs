using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview_Test.Models;
[Table("UserProfileTb")]
public class UserProfileModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ProfileId { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string FirstName { get; set; }
    [Required]
    [Column(TypeName = "varchar(100)")]
    public string LastName { get; set; }
    public int? Age { get; set; }
    [ForeignKey("Id")]
    [Required]
    public UserModel User { get; set; }
}