namespace Rtl.Shows.Repository;

public class Cast : BaseEntity
{
    public string Name { get; set; }

    public DateTime? Birthday { get; set; }

    public IList<ShowCast> ShowCasts { get; set; }

}