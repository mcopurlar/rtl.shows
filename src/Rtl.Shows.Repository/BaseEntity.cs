using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rtl.Shows.Repository;

public abstract class BaseEntity
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    public DateTime LastUpdatedAt { get; set; }
}