namespace Rtl.Shows.Repository;

public class Person : BaseEntity
{
    public string Name { get; set; }

    public DateTime? Birthday { get; set; }

    public IList<ShowPerson> ShowPersons { get; set; }

}