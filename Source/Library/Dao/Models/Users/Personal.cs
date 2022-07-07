using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dao.Models;

public class Personal
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }

    public string? Passport { get; set; }
    public DateTime? PassportExpiryDate { get; set; }

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }
}