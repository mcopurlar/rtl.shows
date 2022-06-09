namespace Rtl.Shows.Repository;

public class Show : BaseEntity
{
    public string Name { get; set; }

    public IList<ShowCast> ShowCasts { get; set; }
}