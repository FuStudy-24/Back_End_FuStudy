using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FuStudy_Repository.Entity;

[Table("Token")]
public class Token
{
    [Key] 
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id;
    
    public long UserId;

    public string TokenValue { get; set; }

    public DateTime Time{ get; set; }

    public int Revoked { get; set; }

    public int IsExpired { get; set; }
    
    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    
    
}