namespace Rtl.Shows.Repository;

public class Show : BaseEntity
{
    public string Name { get; set; }

    public IList<ShowPerson> ShowPersons { get; set; }
}